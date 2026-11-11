// Copyright 2026 Google LLC
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using GitHub.Runner.Worker;
using GitHub.Runner.Worker.Container;
using k8s;
using k8s.Autorest;
using k8s.Models;
using Moq;
using Xunit;

namespace GitHub.Runner.Common.Tests.Worker
{
    public sealed class KubernetesManagerL0
    {
        [Fact]
        [Trait("Level", "L0")]
        [Trait("Category", "Worker")]
        public void ApplyRunnerPodLink_AddsLabelAndOwnerReference()
        {
            var pod = new V1Pod { Metadata = new V1ObjectMeta { Labels = new Dictionary<string, string> { { "managed-by", "runner" } } } };
            var runnerPod = new V1Pod { Metadata = new V1ObjectMeta { Name = "pool-abc123", Uid = "uid-1" } };

            KubernetesManager.ApplyRunnerPodLink(pod, "pool-abc123", KubernetesManager.BuildRunnerPodOwnerReference(runnerPod));

            Assert.Equal("pool-abc123", pod.Metadata.Labels[KubernetesManager.RunnerPodLabelKey]);
            Assert.Equal("runner", pod.Metadata.Labels["managed-by"]);
            var ownerRef = Assert.Single(pod.Metadata.OwnerReferences);
            Assert.Equal("v1", ownerRef.ApiVersion);
            Assert.Equal("Pod", ownerRef.Kind);
            Assert.Equal("pool-abc123", ownerRef.Name);
            Assert.Equal("uid-1", ownerRef.Uid);
            Assert.Null(ownerRef.Controller);
            Assert.Null(ownerRef.BlockOwnerDeletion);
        }

        [Fact]
        [Trait("Level", "L0")]
        [Trait("Category", "Worker")]
        public void ApplyRunnerPodLink_SkipsLabelForLongNameAndOwnerRefWhenMissingUid()
        {
            var pod = new V1Pod { Metadata = new V1ObjectMeta() };
            var longName = new string('a', 64);
            var runnerPodWithoutUid = new V1Pod { Metadata = new V1ObjectMeta { Name = longName } };

            KubernetesManager.ApplyRunnerPodLink(pod, longName, KubernetesManager.BuildRunnerPodOwnerReference(runnerPodWithoutUid));

            Assert.True(pod.Metadata.Labels == null || !pod.Metadata.Labels.ContainsKey(KubernetesManager.RunnerPodLabelKey));
            Assert.Null(pod.Metadata.OwnerReferences);
        }

        public static IEnumerable<object[]> JobContainerEnvCases => new List<object[]>
        {
            // Forwards container.env, skips HOME and empty/null values, defaults CI and GITHUB_ACTIONS.
            new object[]
            {
                new Dictionary<string, string> { ["MAXTEXT_PACKAGE_EXTRA"] = "tpu", ["HOME"] = "/github/home", ["TPU_SKIP_MDS_QUERY"] = "", ["NULL_VALUE"] = null },
                null,
                new Dictionary<string, string> { ["MAXTEXT_PACKAGE_EXTRA"] = "tpu", ["CI"] = "true", ["GITHUB_ACTIONS"] = "true" },
            },
            // An explicit CI is kept, and template env wins on name clashes without duplicate entries.
            new object[]
            {
                new Dictionary<string, string> { ["CI"] = "false", ["SHARED"] = "workflow", ["WORKFLOW_ONLY"] = "1" },
                new List<V1EnvVar> { new V1EnvVar { Name = "SHARED", Value = "template" }, new V1EnvVar { Name = "XDG_CACHE_HOME", Value = "/__w/.cache/bazel" } },
                new Dictionary<string, string> { ["CI"] = "false", ["SHARED"] = "template", ["WORKFLOW_ONLY"] = "1", ["GITHUB_ACTIONS"] = "true", ["XDG_CACHE_HOME"] = "/__w/.cache/bazel" },
            },
            // Empty container.env and template env still get the defaults.
            new object[]
            {
                new Dictionary<string, string>(),
                new List<V1EnvVar>(),
                new Dictionary<string, string> { ["CI"] = "true", ["GITHUB_ACTIONS"] = "true" },
            },
            // CI set to '' suppresses the default (and stays unset), like V1.
            new object[]
            {
                new Dictionary<string, string> { ["CI"] = "" },
                null,
                new Dictionary<string, string> { ["GITHUB_ACTIONS"] = "true" },
            },
            // GITHUB_ACTIONS is always forced to true, only an exact HOME is dropped, and the template
            // can still override a default without leaving a duplicate.
            new object[]
            {
                new Dictionary<string, string> { ["GITHUB_ACTIONS"] = "false", ["Home"] = "/x" },
                new List<V1EnvVar> { new V1EnvVar { Name = "CI", Value = "false" } },
                new Dictionary<string, string> { ["Home"] = "/x", ["CI"] = "false", ["GITHUB_ACTIONS"] = "true" },
            },
        };

        [Theory]
        [Trait("Level", "L0")]
        [Trait("Category", "Worker")]
        [MemberData(nameof(JobContainerEnvCases))]
        public void CreateJobContainer_BuildsEnvFromContainerEnvAndTemplate(Dictionary<string, string> containerEnv, List<V1EnvVar> templateEnv, Dictionary<string, string> expected)
        {
            var jobContainer = new ContainerInfo { ContainerImage = "image" };
            foreach (var env in containerEnv)
            {
                jobContainer.ContainerEnvironmentVariables[env.Key] = env.Value;
            }

            var container = KubernetesManager.CreateJobContainer(jobContainer, isMtlsEnabled: false, resources: null, templateVolumeMounts: null, templateEnv: templateEnv);

            // ToDictionary throws on duplicate names, so this also checks there are none.
            Assert.Equal(expected, container.Env.ToDictionary(e => e.Name, e => e.Value));
        }

        [Theory]
        [Trait("Level", "L0")]
        [Trait("Category", "Worker")]
        [InlineData("Pending", null, false, false)]
        [InlineData("Pending", null, true, true)]
        [InlineData("Running", null, true, true)]
        [InlineData("Unknown", null, false, true)]
        [InlineData("Failed", "Evicted", false, true)]
        [InlineData("Failed", "evicted", false, true)]
        [InlineData("Failed", "NodeShutdown", false, true)]
        [InlineData("Failed", "Terminated", false, true)]
        [InlineData("Failed", "UnexpectedAdmissionError", false, true)]
        [InlineData("Failed", "OutOfnvidia.com/gpu", false, true)]
        [InlineData("Failed", "Error", false, false)]
        [InlineData("Failed", null, false, false)]
        public void IsWorkflowPodLost_OnlyForNodeLevelFailures(string phase, string reason, bool deleting, bool expected)
        {
            var pod = new V1Pod
            {
                Metadata = new V1ObjectMeta { DeletionTimestamp = deleting ? DateTime.UtcNow : null },
                Status = new V1PodStatus { Phase = phase, Reason = reason }
            };

            Assert.Equal(expected, KubernetesManager.IsWorkflowPodLost(pod, out var why));
            Assert.Equal(expected, !string.IsNullOrEmpty(why));
        }

        [Theory]
        [Trait("Level", "L0")]
        [Trait("Category", "Worker")]
        [InlineData(null, 3)]
        [InlineData("0", 3)]
        [InlineData("-1", 3)]
        [InlineData("abc", 3)]
        [InlineData("1", 1)]
        [InlineData("5", 5)]
        [InlineData("50", 10)]
        public void ParseMaxWorkflowPodAttempts_DefaultsToThreeAndCapsAtTen(string value, int expected)
        {
            Assert.Equal(expected, KubernetesManager.ParseMaxWorkflowPodAttempts(value));
        }

        // Each status read is "404", "deleting" (Running + deletionTimestamp) or "running".
        [Theory]
        [Trait("Level", "L0")]
        [Trait("Category", "Worker")]
        [InlineData("404,running", false, 2, null)]
        [InlineData("deleting,running", false, 2, null)]
        [InlineData("404,404,404", false, 3, "Gave up after 3 attempt(s)")]
        [InlineData("404", true, 1, "Runner pod pool-abc123 is being deleted")]
        public async Task PrepareJobAsync_RecreatesLostWorkflowPod(string reads, bool runnerPodGoneOnRetry, int expectedCreates, string expectedError)
        {
            var coreV1 = new Mock<ICoreV1Operations>();
            var client = new Mock<IKubernetes>();
            client.Setup(c => c.CoreV1).Returns(coreV1.Object);
            coreV1.Setup(c => c.CreateNamespacedPodWithHttpMessagesAsync(It.IsAny<V1Pod>(), "ns", It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool?>(), It.IsAny<IReadOnlyDictionary<string, IReadOnlyList<string>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new HttpOperationResponse<V1Pod> { Body = new V1Pod() });
            coreV1.Setup(c => c.DeleteNamespacedPodWithHttpMessagesAsync(It.IsAny<string>(), "ns", It.IsAny<V1DeleteOptions>(), It.IsAny<string>(), It.IsAny<int?>(), It.IsAny<bool?>(), It.IsAny<bool?>(), It.IsAny<string>(), It.IsAny<bool?>(), It.IsAny<IReadOnlyDictionary<string, IReadOnlyList<string>>>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(NotFound());
            var runnerPodReads = 0;
            coreV1.Setup(c => c.ReadNamespacedPodWithHttpMessagesAsync("pool-abc123", "ns", It.IsAny<bool?>(), It.IsAny<IReadOnlyDictionary<string, IReadOnlyList<string>>>(), It.IsAny<CancellationToken>()))
                .Returns(() => runnerPodGoneOnRetry && runnerPodReads++ > 0
                    ? throw NotFound()
                    : Task.FromResult(new HttpOperationResponse<V1Pod> { Body = new V1Pod { Metadata = new V1ObjectMeta { Name = "pool-abc123", Uid = "uid-1" } } }));
            var statusReads = new Queue<string>(reads.Split(','));
            coreV1.Setup(c => c.ReadNamespacedPodStatusWithHttpMessagesAsync(It.IsAny<string>(), "ns", It.IsAny<bool?>(), It.IsAny<IReadOnlyDictionary<string, IReadOnlyList<string>>>(), It.IsAny<CancellationToken>()))
                .Returns(() => statusReads.Dequeue() switch
                {
                    "404" => throw NotFound(),
                    var read => Task.FromResult(new HttpOperationResponse<V1Pod>
                    {
                        Body = new V1Pod
                        {
                            Metadata = new V1ObjectMeta { DeletionTimestamp = read == "deleting" ? DateTime.UtcNow : null },
                            Status = new V1PodStatus { Phase = "Running", PodIP = "10.0.0.1" }
                        }
                    }),
                });

            var ec = new Mock<IExecutionContext>();
            ec.Setup(x => x.Global).Returns(new GlobalContext());
            ec.Setup(x => x.JobContext).Returns(new JobContext());
            ec.Setup(x => x.CancellationToken).Returns(CancellationToken.None);
            using var hc = new TestHostContext(this);
            var manager = new KubernetesManager { PollInterval = TimeSpan.Zero, RecreateDelay = TimeSpan.Zero };
            manager.Initialize(hc);
            manager.SetK8sClient(client.Object, "ns");
            var jobContainer = new ContainerInfo { ContainerImage = "image", IsJobContainer = true };

            Environment.SetEnvironmentVariable("ACTIONS_RUNNER_POD_NAME", "pool-abc123");
            Environment.SetEnvironmentVariable("ACTIONS_RUNNER_CONTAINER_HOOK_TEMPLATE", "/nonexistent/extension.yaml");
            try
            {
                var prepare = manager.PrepareJobAsync(ec.Object, new List<ContainerInfo> { jobContainer });
                if (expectedError == null)
                {
                    await prepare;
                    Assert.Equal("10.0.0.1", jobContainer.ContainerIP);
                }
                else
                {
                    var ex = await Assert.ThrowsAnyAsync<Exception>(() => prepare);
                    Assert.Contains(expectedError, ex.Message);
                }
            }
            finally
            {
                Environment.SetEnvironmentVariable("ACTIONS_RUNNER_POD_NAME", null);
                Environment.SetEnvironmentVariable("ACTIONS_RUNNER_CONTAINER_HOOK_TEMPLATE", null);
            }

            coreV1.Verify(c => c.CreateNamespacedPodWithHttpMessagesAsync(It.IsAny<V1Pod>(), "ns", It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool?>(), It.IsAny<IReadOnlyDictionary<string, IReadOnlyList<string>>>(), It.IsAny<CancellationToken>()), Times.Exactly(expectedCreates));
        }

        private static HttpOperationException NotFound()
        {
            return new HttpOperationException { Response = new HttpResponseMessageWrapper(new HttpResponseMessage(HttpStatusCode.NotFound), string.Empty) };
        }
    }
}
