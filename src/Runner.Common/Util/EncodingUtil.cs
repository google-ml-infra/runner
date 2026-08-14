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
using System.Threading;
using System.Threading.Tasks;
using GitHub.Runner.Sdk;
using GitHub.Runner.Common;

namespace GitHub.Runner.Common.Util
{
    public static class EncodingUtil
    {
        public static async Task SetEncoding(IHostContext hostContext, Tracing trace, CancellationToken cancellationToken)
        {
#if OS_WINDOWS
            try
            {
                if (Console.InputEncoding.CodePage != 65001)
                {
                    using (var p = hostContext.CreateService<IProcessInvoker>())
                    {
                        // Use UTF8 code page
                        int exitCode = await p.ExecuteAsync(workingDirectory: hostContext.GetDirectory(WellKnownDirectory.Work),
                                                fileName: WhichUtil.Which("chcp", true, trace),
                                                arguments: "65001",
                                                environment: null,
                                                requireExitCodeZero: false,
                                                outputEncoding: null,
                                                killProcessOnCancel: false,
                                                redirectStandardIn: null,
                                                inheritConsoleHandler: true,
                                                cancellationToken: cancellationToken);
                        if (exitCode == 0)
                        {
                            trace.Info("Successfully returned to code page 65001 (UTF8)");
                        }
                        else
                        {
                            trace.Warning($"'chcp 65001' failed with exit code {exitCode}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                trace.Warning($"'chcp 65001' failed with exception {ex.Message}");
            }
#endif
            // Dummy variable to prevent compiler error CS1998: "This async method lacks 'await' operators and will run synchronously..."
            await Task.CompletedTask;
        }
    }
}
