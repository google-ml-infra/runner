using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GitHub.DistributedTask.Pipelines.ContextData;
using GitHub.Runner.Common;
using GitHub.Runner.Common.Util;
using GitHub.Runner.Sdk;
using k8s;
using k8s.Models;

namespace GitHub.Runner.Worker.Container
{
    [ServiceLocator(Default = typeof(KubernetesManager))]
    public interface IKubernetesManager : IRunnerService
    {
        Task PrepareJobAsync(IExecutionContext context, List<ContainerInfo> containers);
        Task CleanupJobAsync(IExecutionContext context, List<ContainerInfo> containers);
    }

    public class KubernetesManager : RunnerService, IKubernetesManager
    {
        private const string DefaultWorkflowAgentImage = "us-docker.pkg.dev/ml-oss-artifacts-published/ml-public-container/workflow-agent:latest";

        private IKubernetes _k8sClient;
        private string _namespace;
        private readonly object _lock = new object();

        private (IKubernetes Client, string Namespace) GetK8sClient()
        {
            if (_k8sClient == null)
            {
                lock (_lock)
                {
                    if (_k8sClient == null)
                    {
                        var k8sConfig = KubernetesClientConfiguration.InClusterConfig();
                        _k8sClient = new Kubernetes(k8sConfig);
                        _namespace = k8sConfig.Namespace ?? "default";
                    }
                }
            }
            return (_k8sClient, _namespace);
        }

        public async Task PrepareJobAsync(IExecutionContext context, List<ContainerInfo> containers)
        {
            Trace.Entering();
            var jobContainer = containers.Where(c => c.IsJobContainer).SingleOrDefault();
            if (jobContainer == null)
            {
                throw new InvalidOperationException("Job container is required.");
            }

            var runnerPodName = Environment.GetEnvironmentVariable("ACTIONS_RUNNER_POD_NAME");
            string podName;
            if (!string.IsNullOrEmpty(runnerPodName))
            {
                var prefix = runnerPodName.Length > 54 ? runnerPodName.Substring(0, 54) : runnerPodName;
                podName = $"{prefix}-workflow";
            }
            else
            {
                podName = $"runner-{Guid.NewGuid().ToString().Substring(0, 8)}";
            }
            jobContainer.ContainerId = podName;
            context.JobContext.Container["id"] = new StringContextData(podName);

            // Retrieve cached Kubernetes client
            var (client, namespaceVal) = GetK8sClient();

            // Build Pod object
            var pod = BuildPodSpec(context, podName, jobContainer);

            context.Debug($"Creating workflow pod {podName} using Kubernetes C# client...");
            await ExecuteK8sRequestAsync(context, async () =>
            {
                return await client.CoreV1.CreateNamespacedPodAsync(pod, namespaceVal);
            });

            context.Output($"Workflow pod '{podName}' created successfully. Waiting for readiness and Pod IP...");

            // Wait for readiness and resolve IP
            string podIP = await WaitForPodIPAsync(client, podName, namespaceVal, context);

            jobContainer.ContainerIP = podIP;
            jobContainer.IsAlpine = false;
            context.Output("Workflow pod is ready");
            context.Debug($"Workflow pod resolved ContainerIP: {podIP}");
        }

        private V1Pod BuildPodSpec(
            IExecutionContext context,
            string podName,
            ContainerInfo jobContainer)
        {
            V1Pod templatePod = LoadTemplatePod(context);

            var runnerPodName = Environment.GetEnvironmentVariable("ACTIONS_RUNNER_POD_NAME");
            bool isMtlsEnabled = !string.IsNullOrEmpty(runnerPodName) && Directory.Exists("/etc/certs");

            var podVolumes = GetPodVolumes(runnerPodName, isMtlsEnabled, templatePod);

            var pod = new V1Pod
            {
                ApiVersion = "v1",
                Kind = "Pod",
                Metadata = new V1ObjectMeta
                {
                    Name = podName,
                    Labels = new Dictionary<string, string> { { "managed-by", "runner" } },
                    Annotations = new Dictionary<string, string>()
                },
                Spec = new V1PodSpec
                {
                    RestartPolicy = "Never",
                    Volumes = podVolumes,
                    Containers = new List<V1Container>()
                }
            };

            MergeTemplate(pod, templatePod);

            // Find $job container template (prefix '$' or first container)
            V1Container jobTemplate = null;
            if (templatePod?.Spec?.Containers != null)
            {
                jobTemplate = templatePod.Spec.Containers.FirstOrDefault(c => c.Name == "$job")
                              ?? templatePod.Spec.Containers.FirstOrDefault(c => c.Name != null && c.Name.StartsWith("$"))
                              ?? templatePod.Spec.Containers.FirstOrDefault();
            }

            V1ResourceRequirements resources = jobTemplate?.Resources;
            IList<V1VolumeMount> templateVolumeMounts = jobTemplate?.VolumeMounts;
            IList<V1EnvVar> templateEnv = jobTemplate?.Env;

            // Setup Init Container (Workflow Agent Injector)
            var envAgentImage = Environment.GetEnvironmentVariable("ACTIONS_RUNNER_WORKFLOW_AGENT_IMAGE");
            var agentImage = string.IsNullOrEmpty(envAgentImage) ? DefaultWorkflowAgentImage : envAgentImage;
            pod.Spec.InitContainers = new List<V1Container> { CreateInitContainer(agentImage) };

            // Setup Main Job Container
            pod.Spec.Containers.Add(CreateJobContainer(jobContainer, isMtlsEnabled, resources, templateVolumeMounts, templateEnv));

            // Append Sidecar Containers from template (all containers not starting with '$')
            if (templatePod?.Spec?.Containers != null)
            {
                foreach (var container in templatePod.Spec.Containers)
                {
                    if (container.Name != null && !container.Name.StartsWith("$"))
                    {
                        pod.Spec.Containers.Add(container);
                    }
                }
            }

            return pod;
        }

        private V1Pod LoadTemplatePod(IExecutionContext context)
        {
            var templatePath = Environment.GetEnvironmentVariable("ACTIONS_RUNNER_CONTAINER_HOOK_TEMPLATE") ?? "/etc/config/extension.yaml";

            if (!File.Exists(templatePath))
            {
                context.Debug($"Pod extension configuration template file does not exist at '{templatePath}'. Skipping template loading.");
                return null;
            }

            try
            {
                return k8s.KubernetesYaml.Deserialize<V1Pod>(File.ReadAllText(templatePath));
            }
            catch (Exception ex)
            {
                context.Warning($"Warning: Failed to parse pod extension configuration template from {templatePath}: {ex.Message}");
                return null;
            }
        }

        private List<V1Volume> GetPodVolumes(string runnerPodName, bool isMtlsEnabled, V1Pod templatePod)
        {
            var podVolumes = new List<V1Volume>
            {
                new V1Volume { Name = "work", EmptyDir = new V1EmptyDirVolumeSource() },
                new V1Volume { Name = "externals", EmptyDir = new V1EmptyDirVolumeSource() }
            };

            if (isMtlsEnabled)
            {
                var mtlsSecretName = "certs-" + runnerPodName;
                podVolumes.Add(new V1Volume
                {
                    Name = "certs-volume",
                    Secret = new V1SecretVolumeSource
                    {
                        SecretName = mtlsSecretName
                    }
                });
            }

            if (templatePod?.Spec?.Volumes != null)
            {
                foreach (var v in templatePod.Spec.Volumes)
                {
                    podVolumes.Add(v);
                }
            }

            return podVolumes;
        }

        private void MergeTemplate(V1Pod pod, V1Pod templatePod)
        {
            if (templatePod == null)
            {
                return;
            }

            if (templatePod.Metadata != null)
            {
                if (templatePod.Metadata.Labels != null)
                {
                    foreach (var kvp in templatePod.Metadata.Labels)
                    {
                        pod.Metadata.Labels[kvp.Key] = kvp.Value;
                    }
                }
                if (templatePod.Metadata.Annotations != null)
                {
                    foreach (var kvp in templatePod.Metadata.Annotations)
                    {
                        pod.Metadata.Annotations[kvp.Key] = kvp.Value;
                    }
                }
            }

            if (templatePod.Spec != null)
            {
                foreach (var prop in typeof(V1PodSpec).GetProperties())
                {
                    if (!prop.CanRead || !prop.CanWrite)
                    {
                        continue;
                    }
                    if (prop.Name == nameof(V1PodSpec.Containers) ||
                        prop.Name == nameof(V1PodSpec.InitContainers) ||
                        prop.Name == nameof(V1PodSpec.Volumes) ||
                        prop.Name == nameof(V1PodSpec.RestartPolicy))
                    {
                        continue;
                    }
                    var val = prop.GetValue(templatePod.Spec);
                    if (val != null)
                    {
                        prop.SetValue(pod.Spec, val);
                    }
                }
            }
        }

        private V1Container CreateInitContainer(string agentImage)
        {
            return new V1Container
            {
                Name = "agent-injector",
                Image = agentImage,
                Command = new List<string> { "sh", "-c", "cp /bin/workflow-agent /workflow/workflow-agent && cp -r /bin/externals/. /externals/" },
                VolumeMounts = new List<V1VolumeMount>
                {
                    new V1VolumeMount { Name = "work", MountPath = "/workflow" },
                    new V1VolumeMount { Name = "externals", MountPath = "/externals" }
                }
            };
        }

        private V1Container CreateJobContainer(
            ContainerInfo jobContainer,
            bool isMtlsEnabled,
            V1ResourceRequirements resources,
            IList<V1VolumeMount> templateVolumeMounts,
            IList<V1EnvVar> templateEnv)
        {
            var agentPort = Environment.GetEnvironmentVariable("ACTIONS_RUNNER_WORKFLOW_AGENT_PORT") ?? "50051";

            var workflowVolumeMounts = new List<V1VolumeMount>
            {
                new V1VolumeMount { Name = "work", MountPath = "/__w" },
                new V1VolumeMount { Name = "externals", MountPath = "/__e" }
            };

            if (isMtlsEnabled)
            {
                workflowVolumeMounts.Add(new V1VolumeMount
                {
                    Name = "certs-volume",
                    MountPath = "/etc/certs",
                    ReadOnlyProperty = true
                });
            }

            if (templateVolumeMounts != null)
            {
                foreach (var vm in templateVolumeMounts)
                {
                    workflowVolumeMounts.Add(vm);
                }
            }

            var envList = new List<V1EnvVar>();
            if (templateEnv != null)
            {
                foreach (var env in templateEnv)
                {
                    envList.Add(env);
                }
            }

            return new V1Container
            {
                Name = "job",
                Image = jobContainer.ContainerImage,
                Command = new List<string>
                {
                    "sh",
                    "-c",
                    $"mkdir -p /__w/_temp/_github_home /__w/_temp/_github_workflow /github && ln -sfn /__w/_temp/_github_home /github/home && ln -sfn /__w/_temp/_github_workflow /github/workflow && exec /__w/workflow-agent --port {agentPort}"
                },
                VolumeMounts = workflowVolumeMounts,
                Resources = resources,
                Env = envList
            };
        }

        private async Task<string> WaitForPodIPAsync(IKubernetes client, string podName, string namespaceVal, IExecutionContext context)
        {
            string lastPhase = null;
            var timeout = DateTime.UtcNow.AddMinutes(5);
            while (DateTime.UtcNow < timeout)
            {
                await Task.Delay(2000);
                var polledPod = await ExecuteK8sRequestAsync(context, async () =>
                {
                    return await client.CoreV1.ReadNamespacedPodStatusAsync(podName, namespaceVal);
                });
                var phase = polledPod.Status?.Phase;
                var ip = polledPod.Status?.PodIP;

                context.Debug($"Workflow pod {podName} phase: {phase}, IP: {ip}");
                if (phase != lastPhase)
                {
                    context.Output($"Pod status: {phase}");
                    lastPhase = phase;
                }

                if (string.Equals(phase, "Running", StringComparison.OrdinalIgnoreCase) && !string.IsNullOrEmpty(ip))
                {
                    return ip;
                }
                else if (string.Equals(phase, "Failed", StringComparison.OrdinalIgnoreCase) ||
                         string.Equals(phase, "Unknown", StringComparison.OrdinalIgnoreCase))
                {
                    throw new InvalidOperationException($"Workflow pod entered failed phase: {phase}");
                }
            }

            throw new TimeoutException($"Timed out waiting for GKE workflow pod {podName} to come online and resolve IP.");
        }


        private async Task<T> ExecuteK8sRequestAsync<T>(IExecutionContext context, Func<Task<T>> request)
        {
            try
            {
                return await request();
            }
            catch (k8s.Autorest.HttpOperationException ex)
            {
                var details = ex.Response?.Content ?? "No response body content";
                context.Error($"Kubernetes API call failed. Error: {ex.Message}. Response details: {details}");
                throw;
            }
        }

        public async Task CleanupJobAsync(IExecutionContext context, List<ContainerInfo> containers)
        {
            Trace.Entering();
            var jobContainer = containers.Where(c => c.IsJobContainer).SingleOrDefault();
            if (jobContainer == null || string.IsNullOrEmpty(jobContainer.ContainerId))
            {
                return;
            }

            var podName = jobContainer.ContainerId;
            context.Debug($"Native Kubernetes pod cleanup triggered. Deleting workflow pod: {podName}");

            try
            {
                // Retrieve cached Kubernetes client
                var (client, namespaceVal) = GetK8sClient();

                await ExecuteK8sRequestAsync(context, async () =>
                {
                    return await client.CoreV1.DeleteNamespacedPodAsync(podName, namespaceVal);
                });
                context.Debug($"Successfully deleted Kubernetes workflow pod: {podName}");
            }
            catch (Exception ex)
            {
                context.Warning($"Warning: Failed to delete Kubernetes workflow pod {podName}: {ex.Message}");
            }
        }
    }
}
