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

using GitHub.Services.Common;
using System;
using Xunit;
using GitHub.Runner.Sdk;

namespace GitHub.Runner.Common.Tests.Util
{
    public sealed class VssUtilL0
    {
        [Fact]
        [Trait("Level", "L0")]
        [Trait("Category", "Common")]
        public void VerifyOverwriteVssConnectionSetting()
        {
            using (TestHostContext hc = new(this))
            {
                Tracing trace = hc.GetTrace();

                // Act.
                try
                {
                    trace.Info("Set httpretry to 10.");
                    Environment.SetEnvironmentVariable("GITHUB_ACTIONS_RUNNER_HTTP_RETRY", "10");
                    trace.Info("Set httptimeout to 360.");
                    Environment.SetEnvironmentVariable("GITHUB_ACTIONS_RUNNER_HTTP_TIMEOUT", "360");

                    var connect = VssUtil.CreateConnection(new Uri("https://github.com/actions/runner"), new VssCredentials());

                    // Assert.
                    Assert.Equal("10", connect.Settings.MaxRetryRequest.ToString());
                    Assert.Equal("360", connect.Settings.SendTimeout.TotalSeconds.ToString());

                    trace.Info("Set httpretry to 100.");
                    Environment.SetEnvironmentVariable("GITHUB_ACTIONS_RUNNER_HTTP_RETRY", "100");
                    trace.Info("Set httptimeout to 3600.");
                    Environment.SetEnvironmentVariable("GITHUB_ACTIONS_RUNNER_HTTP_TIMEOUT", "3600");

                    connect = VssUtil.CreateConnection(new Uri("https://github.com/actions/runner"), new VssCredentials());

                    // Assert.
                    Assert.Equal("10", connect.Settings.MaxRetryRequest.ToString());
                    Assert.Equal("1200", connect.Settings.SendTimeout.TotalSeconds.ToString());
                }
                finally
                {
                    Environment.SetEnvironmentVariable("GITHUB_ACTIONS_RUNNER_HTTP_RETRY", "");
                    Environment.SetEnvironmentVariable("GITHUB_ACTIONS_RUNNER_HTTP_TIMEOUT", "");
                }
            }
        }
    }
}
