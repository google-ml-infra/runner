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
                return await client.CoreV1.CreateNamespacedPodAsync(pod, namespaceVal, cancellationToken: context.CancellationToken);
            });

            context.Output($"Workflow pod '{podName}' created successfully. Waiting for readiness and Pod IP...");

            // Wait for readiness and resolve IP; clean up pod if wait fails or is canceled
            string podIP;
            try
            {
                podIP = await WaitForPodIPAsync(client, podName, namespaceVal, context);
            }
            catch
            {
                try
                {
                    await client.CoreV1.DeleteNamespacedPodAsync(podName, namespaceVal);
                }
                catch (Exception cleanupEx)
                {
                    context.Warning($"Warning: Failed to clean up workflow pod {podName}: {cleanupEx.Message}");
                }
                throw;
            }

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
                    var existingIdx = podVolumes.FindIndex(x => x.Name == v.Name);
                    if (existingIdx >= 0)
                    {
                        podVolumes[existingIdx] = v;
                    }
                    else
                    {
                        podVolumes.Add(v);
                    }
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
                    var existingIdx = workflowVolumeMounts.FindIndex(x => x.MountPath == vm.MountPath);
                    if (existingIdx >= 0)
                    {
                        workflowVolumeMounts[existingIdx] = vm;
                    }
                    else
                    {
                        workflowVolumeMounts.Add(vm);
                    }
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

        private static TimeSpan GetTimeoutMinutes(string envVar, int defaultMinutes)
        {
            return int.TryParse(Environment.GetEnvironmentVariable(envVar), out var m) && m > 0
                ? TimeSpan.FromMinutes(m)
                : TimeSpan.FromMinutes(defaultMinutes);
        }

        private async Task<string> WaitForPodIPAsync(IKubernetes client, string podName, string namespaceVal, IExecutionContext context)
        {
            var schedulingTimeout = GetTimeoutMinutes("ACTIONS_RUNNER_WORKFLOW_POD_SCHEDULING_TIMEOUT_MINUTES", 60);
            var startupTimeout = GetTimeoutMinutes("ACTIONS_RUNNER_WORKFLOW_POD_STARTUP_TIMEOUT_MINUTES", 10);
            var startTime = DateTime.UtcNow;
            DateTime? scheduledAt = null;
            DateTime? lastLogTime = null;
            string lastPhase = null;
            string lastUnscheduledReason = null;

            while (true)
            {
                var pollDelayMs = (!scheduledAt.HasValue && (DateTime.UtcNow - startTime).TotalSeconds >= 30) ? 10000 : 2000;
                await Task.Delay(pollDelayMs, context.CancellationToken);
                var polledPod = await ExecuteK8sRequestAsync(context, async () =>
                {
                    return await client.CoreV1.ReadNamespacedPodStatusAsync(podName, namespaceVal, cancellationToken: context.CancellationToken);
                });
                var phase = polledPod.Status?.Phase;
                var ip = polledPod.Status?.PodIP;
                var now = DateTime.UtcNow;

                context.Debug($"Workflow pod {podName} phase: {phase}, IP: {ip}");

                if (string.Equals(phase, "Running", StringComparison.OrdinalIgnoreCase) && !string.IsNullOrEmpty(ip))
                {
                    return ip;
                }
                if (string.Equals(phase, "Failed", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(phase, "Unknown", StringComparison.OrdinalIgnoreCase))
                {
                    throw new InvalidOperationException($"Workflow pod {podName} entered failed phase: {phase}");
                }

                // Stage 1: Pod is not scheduled onto a node yet (waiting for cluster capacity / scale-up)
                var scheduledCond = polledPod.Status?.Conditions?.FirstOrDefault(c => c.Type == "PodScheduled");
                bool isScheduled = scheduledAt.HasValue || string.Equals(scheduledCond?.Status, "True", StringComparison.OrdinalIgnoreCase);
                if (!isScheduled)
                {
                    if ((!lastLogTime.HasValue && (now - startTime).TotalSeconds >= 10) || (lastLogTime.HasValue && (now - lastLogTime.Value).TotalSeconds >= 60))
                    {
                        var reason = scheduledCond?.Message ?? scheduledCond?.Reason ?? phase ?? "Pending";
                        var category = "[Pending Scheduling]";
                        try
                        {
                            var events = await client.CoreV1.ListNamespacedEventAsync(
                                namespaceVal,
                                fieldSelector: $"involvedObject.name={podName}",
                                cancellationToken: context.CancellationToken);
                            var scaleEvent = events?.Items?
                                .Where(e => e.Reason == "FailedScaleUp" || e.Reason == "TriggeredScaleUp" || e.Reason == "NotTriggerScaleUp")
                                .OrderBy(e => e.LastTimestamp ?? e.EventTime ?? e.Metadata?.CreationTimestamp ?? DateTime.MinValue)
                                .LastOrDefault();
                            var msg = scaleEvent?.Message;
                            if (!string.IsNullOrWhiteSpace(msg))
                            {
                                if (scaleEvent.Reason == "FailedScaleUp" || msg.Contains("backoff", StringComparison.OrdinalIgnoreCase))
                                {
                                    category = "[GCE Stockout]";
                                }
                                else if (scaleEvent.Reason == "TriggeredScaleUp")
                                {
                                    category = "[Scaling Up Node]";
                                }
                                else if (msg.Contains("max node group size reached", StringComparison.OrdinalIgnoreCase))
                                {
                                    category = "[Max Capacity Reached]";
                                }
                                else
                                {
                                    category = "[Unschedulable - Config Mismatch]";
                                }
                                reason = $"{reason} ({scaleEvent.Reason}: {msg})";
                            }
                        }
                        catch
                        {
                            // Best-effort event lookup; fall back to PodScheduled condition message
                        }

                        lastUnscheduledReason = category;
                        var elapsed = (int)(now - startTime).TotalSeconds;
                        var remainingMin = Math.Max(1, (int)Math.Ceiling((schedulingTimeout - (now - startTime)).TotalMinutes));
                        context.Output($"{category} Waiting for workflow pod {podName} to be scheduled ({elapsed}s elapsed, next update in 60s, timeout in {remainingMin}m)");
                        context.Debug($"{category} Workflow pod {podName} scheduling detail: {reason}");
                        lastLogTime = now;
                    }
                    if (now - startTime >= schedulingTimeout)
                    {
                        throw new TimeoutException($"{lastUnscheduledReason ?? "[Pending Scheduling]"} Timed out after {(int)schedulingTimeout.TotalMinutes}m waiting for workflow pod {podName} to be scheduled.");
                    }
                    continue;
                }

                // Stage 2: Pod is scheduled onto a node; waiting for image pull, init container, and Pod IP
                scheduledAt ??= now;
                if (phase != lastPhase)
                {
                    context.Output($"Pod {podName} scheduled (status: {phase})");
                    lastPhase = phase;
                }
                if (now - scheduledAt.Value >= startupTimeout)
                {
                    throw new TimeoutException($"Timed out after {(int)startupTimeout.TotalMinutes}m waiting for scheduled workflow pod {podName} to start and resolve IP.");
                }
            }
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
