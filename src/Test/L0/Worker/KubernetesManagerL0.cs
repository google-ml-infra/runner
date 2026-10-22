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

using System.Collections.Generic;
using System.Linq;
using GitHub.Runner.Worker.Container;
using k8s.Models;
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
    }
}
