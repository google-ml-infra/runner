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

﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Xunit;

namespace GitHub.Runner.Common.Tests
{
    public sealed class ProcessExtensionL0
    {
        [Fact]
        [Trait("Level", "L0")]
        [Trait("Category", "Common")]
        public async Task SuccessReadProcessEnv()
        {
            using (TestHostContext hc = new(this))
            {
                Tracing trace = hc.GetTrace();

                string envName = Guid.NewGuid().ToString();
                string envValue = Guid.NewGuid().ToString();

                Process sleep = null;
                try
                {
#if OS_WINDOWS
                    string node = Path.Combine(TestUtil.GetSrcPath(), @"..\_layout\externals\node20\bin\node");
#else
                    string node = Path.Combine(TestUtil.GetSrcPath(), @"../_layout/externals/node20/bin/node");
                    hc.EnqueueInstance<IProcessInvoker>(new ProcessInvokerWrapper());
#endif
                    var startInfo = new ProcessStartInfo(node, "-e \"setTimeout(function(){{}}, 15 * 1000);\"");
                    startInfo.Environment[envName] = envValue;
                    sleep = Process.Start(startInfo);

                    var timeout = Process.GetProcessById(sleep.Id);
                    while (timeout == null)
                    {
                        await Task.Delay(500);
                        timeout = Process.GetProcessById(sleep.Id);
                    }

                    try
                    {
                        trace.Info($"Read env from {timeout.Id}");
                        var value = timeout.GetEnvironmentVariable(hc, envName);
                        if (string.Equals(value, envValue, StringComparison.OrdinalIgnoreCase))
                        {
                            trace.Info($"Find the env.");
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        trace.Error(ex);
                    }

                    Assert.Fail("Failed to retrieve process environment variable.");
                }
                finally
                {
                    sleep?.Kill();
                }
            }
        }
    }
}
