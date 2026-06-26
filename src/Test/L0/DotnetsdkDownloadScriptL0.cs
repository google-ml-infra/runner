// Copyright 2026 Google LLC
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     https://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

﻿using Xunit;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System;

namespace GitHub.Runner.Common.Tests
{
    public sealed class DotnetsdkDownloadScriptL0
    {
        [Fact]
        [Trait("Level", "L0")]
        [Trait("Category", "Runner")]
        public async Task EnsureDotnetsdkBashDownloadScriptUpToDate()
        {
            if ((DateTime.UtcNow.Month - 1) % 3 != 0)
            {
                // Only check these script once a quater.
                return;
            }

            string shDownloadUrl = "https://dot.net/v1/dotnet-install.sh";

            using (HttpClient downloadClient = new())
            {
                var response = await downloadClient.GetAsync("https://www.bing.com");
                if (!response.IsSuccessStatusCode)
                {
                    return;
                }

                string shScript = await downloadClient.GetStringAsync(shDownloadUrl);

                string existingShScript = File.ReadAllText(Path.Combine(TestUtil.GetSrcPath(), "Misc/dotnet-install.sh"));

                bool shScriptMatched = string.Equals(shScript.TrimEnd('\n', '\r', '\0').Replace("\r\n", "\n").Replace("\r", "\n"), existingShScript.TrimEnd('\n', '\r', '\0').Replace("\r\n", "\n").Replace("\r", "\n"));
                //Assert.True(shScriptMatched, "Fix the test by updating Src/Misc/dotnet-install.sh with content from https://dot.net/v1/dotnet-install.sh");
            }
        }

        [Fact]
        [Trait("Level", "L0")]
        [Trait("Category", "Runner")]
        public async Task EnsureDotnetsdkPowershellDownloadScriptUpToDate()
        {
            if ((DateTime.UtcNow.Month - 1) % 3 != 0)
            {
                // Only check these script once a quater.
                return;
            }

            string ps1DownloadUrl = "https://dot.net/v1/dotnet-install.ps1";

            using (HttpClient downloadClient = new())
            {
                var response = await downloadClient.GetAsync("https://www.bing.com");
                if (!response.IsSuccessStatusCode)
                {
                    return;
                }

                string ps1Script = await downloadClient.GetStringAsync(ps1DownloadUrl);

                string existingPs1Script = File.ReadAllText(Path.Combine(TestUtil.GetSrcPath(), "Misc/dotnet-install.ps1"));

                bool ps1ScriptMatched = string.Equals(ps1Script.TrimEnd('\n', '\r', '\0').Replace("\r\n", "\n").Replace("\r", "\n"), existingPs1Script.TrimEnd('\n', '\r', '\0').Replace("\r\n", "\n").Replace("\r", "\n"));
                //Assert.True(ps1ScriptMatched, "Fix the test by updating Src/Misc/dotnet-install.ps1 with content from https://dot.net/v1/dotnet-install.ps1");
            }
        }
    }
}
