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

            // Tie the workflow pod to the runner pod so it can't outlive it: a runner-pod label for
            // orphan detection and an owner reference so Kubernetes GC deletes it with the runner pod.
            if (!string.IsNullOrEmpty(runnerPodName))
            {
                var ownerRef = await GetRunnerPodOwnerReferenceAsync(context, client, runnerPodName, namespaceVal);
                ApplyRunnerPodLink(pod, runnerPodName, ownerRef);
            }

            // The pod can be removed before it starts, e.g. when the cluster autoscaler deletes the node it was
            // just bound to. That's transient: the node is already cordoned, so recreate the pod (same name, so
            // platform cleanup still finds it) instead of failing the job. The scheduling timeout spans attempts.
            var maxAttempts = ParseMaxWorkflowPodAttempts(Environment.GetEnvironmentVariable("ACTIONS_RUNNER_WORKFLOW_POD_MAX_ATTEMPTS"));
            var startTime = DateTime.UtcNow;
            string podIP = null;
            for (int attempt = 0; attempt < maxAttempts; attempt++)
            {
                if (attempt > 0)
                {
                    await PrepareToRecreateWorkflowPodAsync(context, client, podName, namespaceVal, runnerPodName, startTime);
                }

                try
                {
                    podIP = await CreateWorkflowPodAsync(context, client, pod, podName, namespaceVal, startTime);
                    break;
                }
                catch (WorkflowPodLostException ex) when (attempt + 1 < maxAttempts && !context.CancellationToken.IsCancellationRequested)
                {
                    context.Warning($"{ex.Message} Recreating it (attempt {attempt + 2}/{maxAttempts})...");
                }
            }

            jobContainer.ContainerIP = podIP;
            jobContainer.IsAlpine = false;
            context.Output("Workflow pod is ready");
            context.Debug($"Workflow pod resolved ContainerIP: {podIP}");
        }

        // One attempt: create the pod and wait for its IP. Deletes the pod on any failure.
        private async Task<string> CreateWorkflowPodAsync(IExecutionContext context, IKubernetes client, V1Pod pod, string podName, string namespaceVal, DateTime startTime)
        {
            context.Debug($"Creating workflow pod {podName} using Kubernetes C# client...");
            await ExecuteK8sRequestAsync(context, async () =>
            {
                return await client.CoreV1.CreateNamespacedPodAsync(pod, namespaceVal, cancellationToken: context.CancellationToken);
            });

            context.Output($"Workflow pod '{podName}' created successfully. Waiting for readiness and Pod IP...");

            try
            {
                return await WaitForPodIPAsync(client, podName, namespaceVal, context, startTime);
            }
            catch
            {
                await DeleteWorkflowPodQuietlyAsync(context, client, podName, namespaceVal);
                throw;
            }
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

        internal static V1Container CreateJobContainer(
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

            // Forward the workflow's `container.env` (plus any proxy vars the runner added) the same way
            // V1 runner-container-hooks does in prepare-job.ts. Empty values are skipped so `KEY: ''` stays
            // unset rather than set-but-empty. HOME is skipped because MountWellKnownDirectories sets it to
            // /github/home for docker mode, and V1 keeps the image's HOME instead.
            var envList = new List<V1EnvVar>();
            var containerEnv = jobContainer.ContainerEnvironmentVariables;
            foreach (var env in containerEnv)
            {
                if (string.IsNullOrEmpty(env.Key) || string.IsNullOrEmpty(env.Value) || env.Key == "HOME")
                {
                    continue;
                }

                envList.Add(new V1EnvVar { Name = env.Key, Value = env.Value });
            }

            // Like V1 and docker mode, CI defaults to true unless the workflow sets it (even to ''), and
            // GITHUB_ACTIONS is always true.
            if (!containerEnv.ContainsKey("CI"))
            {
                envList.Add(new V1EnvVar { Name = "CI", Value = "true" });
            }

            envList.RemoveAll(x => x.Name == "GITHUB_ACTIONS");
            envList.Add(new V1EnvVar { Name = "GITHUB_ACTIONS", Value = "true" });

            // The extension.yaml $job env is platform-controlled, so it wins on name clashes. V1 gets the
            // same final values by appending it and letting Kubernetes keep the last duplicate; here clashing
            // entries are replaced in place to avoid duplicates, so only $(VAR) references could resolve
            // differently.
            if (templateEnv != null)
            {
                foreach (var env in templateEnv)
                {
                    var existingIdx = envList.FindIndex(x => x.Name == env.Name);
                    if (existingIdx >= 0)
                    {
                        envList[existingIdx] = env;
                    }
                    else
                    {
                        envList.Add(env);
                    }
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

        private async Task<string> WaitForPodIPAsync(IKubernetes client, string podName, string namespaceVal, IExecutionContext context, DateTime startTime)
        {
            var schedulingTimeout = GetTimeoutMinutes("ACTIONS_RUNNER_WORKFLOW_POD_SCHEDULING_TIMEOUT_MINUTES", 60);
            var startupTimeout = GetTimeoutMinutes("ACTIONS_RUNNER_WORKFLOW_POD_STARTUP_TIMEOUT_MINUTES", 10);
            var attemptStartTime = DateTime.UtcNow;
            DateTime? scheduledAt = null;
            DateTime? lastLogTime = null;
            string lastPhase = null;
            string lastUnscheduledReason = null;

            while (true)
            {
                var pollDelay = (!scheduledAt.HasValue && (DateTime.UtcNow - attemptStartTime).TotalSeconds >= 30) ? UnscheduledPollInterval : PollInterval;
                await Task.Delay(pollDelay, context.CancellationToken);
                var polledPod = await ReadPodStatusOrNullAsync(context, client, podName, namespaceVal);
                if (polledPod == null)
                {
                    throw new WorkflowPodLostException($"Workflow pod {podName} was deleted before it started (its node was likely removed).");
                }
                if (IsWorkflowPodLost(polledPod, out var lostReason))
                {
                    throw new WorkflowPodLostException($"Workflow pod {podName} was removed before it started ({lostReason}).");
                }

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


        internal const string RunnerPodLabelKey = "runner-pod";
        private const int MaxLabelValueLength = 63;

        // Looks up the runner pod's UID so the workflow pod can reference it as its owner. The UID must come
        // from the API: an owner reference to a non-existent UID makes Kubernetes GC delete the workflow pod
        // immediately. On any lookup failure we fall back to no owner reference rather than failing the job.
        private async Task<V1OwnerReference> GetRunnerPodOwnerReferenceAsync(IExecutionContext context, IKubernetes client, string runnerPodName, string namespaceVal)
        {
            try
            {
                var runnerPod = await client.CoreV1.ReadNamespacedPodAsync(runnerPodName, namespaceVal, cancellationToken: context.CancellationToken);
                var ownerRef = BuildRunnerPodOwnerReference(runnerPod);
                if (ownerRef == null)
                {
                    context.Warning($"Warning: Runner pod {runnerPodName} has no UID; workflow pod will not be garbage-collected with the runner pod.");
                }
                return ownerRef;
            }
            catch (OperationCanceledException) when (context.CancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (Exception ex)
            {
                context.Warning($"Warning: Failed to look up runner pod {runnerPodName} for owner reference; workflow pod will not be garbage-collected with the runner pod: {ex.Message}");
                return null;
            }
        }

        internal static V1OwnerReference BuildRunnerPodOwnerReference(V1Pod runnerPod)
        {
            if (string.IsNullOrEmpty(runnerPod?.Metadata?.Uid) || string.IsNullOrEmpty(runnerPod.Metadata.Name))
            {
                return null;
            }

            // Controller and BlockOwnerDeletion are intentionally left unset: the runner pod is not a controller,
            // and blocking owner deletion would need extra RBAC and delay runner pod deletion.
            return new V1OwnerReference
            {
                ApiVersion = "v1",
                Kind = "Pod",
                Name = runnerPod.Metadata.Name,
                Uid = runnerPod.Metadata.Uid
            };
        }

        // Adds the runner-pod=<runnerPodName> label (same key as runner-container-hooks) and, if available,
        // an owner reference to the runner pod. Pod names can exceed the 63-char label value limit, in which
        // case the label is skipped rather than failing pod creation.
        internal static void ApplyRunnerPodLink(V1Pod pod, string runnerPodName, V1OwnerReference ownerRef)
        {
            pod.Metadata ??= new V1ObjectMeta();
            if (runnerPodName.Length <= MaxLabelValueLength)
            {
                pod.Metadata.Labels ??= new Dictionary<string, string>();
                pod.Metadata.Labels[RunnerPodLabelKey] = runnerPodName;
            }

            if (ownerRef != null)
            {
                pod.Metadata.OwnerReferences ??= new List<V1OwnerReference>();
                pod.Metadata.OwnerReferences.Add(ownerRef);
            }
        }

        private async Task<T> ExecuteK8sRequestAsync<T>(IExecutionContext context, Func<Task<T>> request, bool logNotFound = true)
        {
            try
            {
                return await request();
            }
            catch (k8s.Autorest.HttpOperationException ex) when (logNotFound || !IsNotFound(ex))
            {
                var details = ex.Response?.Content ?? "No response body content";
                context.Error($"Kubernetes API call failed. Error: {ex.Message}. Response details: {details}");
                throw;
            }
        }

        internal const int DefaultMaxWorkflowPodAttempts = 3;
        internal const int MaxWorkflowPodAttemptsCap = 10;
        private static readonly TimeSpan WorkflowPodGoneTimeout = TimeSpan.FromMinutes(2);

        // Delays are fields so tests can set them to zero.
        internal TimeSpan PollInterval { get; set; } = TimeSpan.FromSeconds(2);
        internal TimeSpan UnscheduledPollInterval { get; set; } = TimeSpan.FromSeconds(10);
        internal TimeSpan RecreateDelay { get; set; } = TimeSpan.FromSeconds(5);

        // Lets tests inject a mock client instead of the in-cluster config.
        internal void SetK8sClient(IKubernetes client, string namespaceVal)
        {
            lock (_lock)
            {
                _k8sClient = client;
                _namespace = namespaceVal;
            }
        }

        // Pod failure reasons that only the node/kubelet sets: the node went away, kicked the pod out, or rejected
        // it at admission (e.g. a new GPU/TPU node whose device plugin isn't ready). None of them mean the job failed.
        private static readonly HashSet<string> LostPodFailureReasons = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "Evicted", "NodeShutdown", "Terminated", "UnexpectedAdmissionError", "NodeAffinity"
        };

        // Thrown when the workflow pod is removed by someone else before it starts; PrepareJobAsync recreates it.
        internal sealed class WorkflowPodLostException : Exception
        {
            public WorkflowPodLostException(string message) : base(message) { }
        }

        internal static int ParseMaxWorkflowPodAttempts(string value)
        {
            if (!int.TryParse(value, out var attempts) || attempts <= 0)
            {
                return DefaultMaxWorkflowPodAttempts;
            }
            return Math.Min(attempts, MaxWorkflowPodAttemptsCap);
        }

        // Only called while waiting for the pod to start, so no job step has run yet and recreating it is safe.
        // The pod is "lost" if something else is deleting it, its node is unreachable (Unknown), or the node
        // failed it with one of LostPodFailureReasons (exact match, or the kubelet's OutOf<resource> prefix).
        internal static bool IsWorkflowPodLost(V1Pod pod, out string reason)
        {
            reason = null;
            var phase = pod?.Status?.Phase;
            if (pod?.Metadata?.DeletionTimestamp != null)
            {
                reason = "pod is being deleted";
                return true;
            }
            if (string.Equals(phase, "Unknown", StringComparison.OrdinalIgnoreCase))
            {
                reason = "node is unreachable";
                return true;
            }
            var failureReason = pod?.Status?.Reason;
            if (string.Equals(phase, "Failed", StringComparison.OrdinalIgnoreCase) && failureReason != null &&
                (LostPodFailureReasons.Contains(failureReason) || failureReason.StartsWith("OutOf", StringComparison.Ordinal)))
            {
                reason = $"pod failed with reason {failureReason}";
                return true;
            }
            return false;
        }

        private static bool IsNotFound(Exception ex)
        {
            return ex is k8s.Autorest.HttpOperationException httpEx && httpEx.Response?.StatusCode == System.Net.HttpStatusCode.NotFound;
        }

        // Returns null if the pod doesn't exist (404), without logging an error.
        private async Task<V1Pod> ReadPodOrNullAsync(IExecutionContext context, Func<Task<V1Pod>> read)
        {
            try
            {
                return await ExecuteK8sRequestAsync(context, read, logNotFound: false);
            }
            catch (Exception ex) when (IsNotFound(ex))
            {
                return null;
            }
        }

        private Task<V1Pod> ReadPodStatusOrNullAsync(IExecutionContext context, IKubernetes client, string podName, string namespaceVal)
        {
            return ReadPodOrNullAsync(context, () => client.CoreV1.ReadNamespacedPodStatusAsync(podName, namespaceVal, cancellationToken: context.CancellationToken));
        }

        // Runs before each recreate. Gives up if the scheduling budget is spent or the runner pod itself is being
        // torn down (the platform deletes the runner pod, then the workflow pod; recreating after that would leak).
        // Then force-deletes the lost pod and waits for its name to be free.
        private async Task PrepareToRecreateWorkflowPodAsync(IExecutionContext context, IKubernetes client, string podName, string namespaceVal, string runnerPodName, DateTime startTime)
        {
            var schedulingTimeout = GetTimeoutMinutes("ACTIONS_RUNNER_WORKFLOW_POD_SCHEDULING_TIMEOUT_MINUTES", 60);
            if (DateTime.UtcNow - startTime >= schedulingTimeout)
            {
                throw new TimeoutException($"Timed out after {(int)schedulingTimeout.TotalMinutes}m; not recreating workflow pod {podName}.");
            }

            if (!string.IsNullOrEmpty(runnerPodName))
            {
                var runnerPod = await ReadPodOrNullAsync(context, () => client.CoreV1.ReadNamespacedPodAsync(runnerPodName, namespaceVal, cancellationToken: context.CancellationToken));
                if (runnerPod == null || runnerPod.Metadata?.DeletionTimestamp != null)
                {
                    throw new InvalidOperationException($"Runner pod {runnerPodName} is being deleted; not recreating workflow pod {podName}.");
                }
            }

            await WaitForPodGoneAsync(context, client, podName, namespaceVal);

            // Not needed for correctness: if the old pod's ephemeral PVC (same name) still exists, the new pod just
            // waits in Pending until Kubernetes GC removes it. The pause only avoids that noise in the job log.
            await Task.Delay(RecreateDelay, context.CancellationToken);
        }

        // Force-deletes a lost pod (if it still exists) and waits until its name is free. Force is fine because
        // it never started, and a graceful delete on a dead node only finishes once PodGC runs.
        private async Task WaitForPodGoneAsync(IExecutionContext context, IKubernetes client, string podName, string namespaceVal)
        {
            try
            {
                await ExecuteK8sRequestAsync(context, async () =>
                {
                    return await client.CoreV1.DeleteNamespacedPodAsync(podName, namespaceVal, gracePeriodSeconds: 0, cancellationToken: context.CancellationToken);
                }, logNotFound: false);
            }
            catch (Exception ex) when (IsNotFound(ex))
            {
                return;
            }

            var deadline = DateTime.UtcNow + WorkflowPodGoneTimeout;
            while (await ReadPodStatusOrNullAsync(context, client, podName, namespaceVal) != null)
            {
                if (DateTime.UtcNow >= deadline)
                {
                    throw new TimeoutException($"Timed out after {(int)WorkflowPodGoneTimeout.TotalMinutes}m waiting for removed workflow pod {podName} to be deleted before recreating it.");
                }
                await Task.Delay(PollInterval, context.CancellationToken);
            }
        }

        private async Task DeleteWorkflowPodQuietlyAsync(IExecutionContext context, IKubernetes client, string podName, string namespaceVal)
        {
            try
            {
                await client.CoreV1.DeleteNamespacedPodAsync(podName, namespaceVal);
            }
            catch (Exception ex) when (IsNotFound(ex))
            {
                // Already gone.
            }
            catch (Exception cleanupEx)
            {
                context.Warning($"Warning: Failed to clean up workflow pod {podName}: {cleanupEx.Message}");
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
                }, logNotFound: false);
                context.Debug($"Successfully deleted Kubernetes workflow pod: {podName}");
            }
            catch (Exception ex) when (IsNotFound(ex))
            {
                context.Debug($"Kubernetes workflow pod {podName} was already deleted.");
            }
            catch (Exception ex)
            {
                context.Warning($"Warning: Failed to delete Kubernetes workflow pod {podName}: {ex.Message}");
            }
        }
    }
}
