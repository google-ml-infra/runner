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
    }
}
