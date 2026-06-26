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

﻿using Moq;
using System.Runtime.CompilerServices;
using Xunit;

namespace GitHub.Runner.Common.Tests
{
    public sealed class CommandLineParserL0
    {
        [Fact]
        [Trait("Level", "L0")]
        [Trait("Category", "Common")]
        public void CanConstruct()
        {
            using (TestHostContext hc = CreateTestContext())
            {
                Tracing trace = hc.GetTrace();

                CommandLineParser clp = new(hc, secretArgNames: new string[0]);
                trace.Info("Constructed");

                Assert.NotNull(clp);
            }
        }

        [Fact]
        [Trait("Level", "L0")]
        [Trait("Category", "Common")]
        public void MasksSecretArgs()
        {
            using (TestHostContext hc = CreateTestContext())
            {
                // Arrange.
                CommandLineParser clp = new(
                    hc,
                    secretArgNames: new[] { "SecretArg1", "SecretArg2" });

                // Assert.
                clp.Parse(new string[]
                {
                    "cmd",
                    "--secretarg1",
                    "secret value 1",
                    "--publicarg",
                    "public arg value",
                    "--secretarg2",
                    "secret value 2",
                });

                // Assert.
                Assert.Equal("***", hc.SecretMasker.MaskSecrets("secret value 1"));
                Assert.Equal("***", hc.SecretMasker.MaskSecrets("secret value 2"));
            }
        }

        [Fact]
        [Trait("Level", "L0")]
        [Trait("Category", "Common")]
        public void ParsesCommands()
        {
            using (TestHostContext hc = CreateTestContext())
            {
                Tracing trace = hc.GetTrace();

                CommandLineParser clp = new(hc, secretArgNames: new string[0]);
                trace.Info("Constructed.");

                clp.Parse(new string[] { "cmd1", "cmd2", "--arg1", "arg1val", "badcmd" });
                trace.Info("Parsed");

                trace.Info("Commands: {0}", clp.Commands.Count);
                Assert.Equal(2, clp.Commands.Count);
            }
        }

        [Fact]
        [Trait("Level", "L0")]
        [Trait("Category", "Common")]
        public void ParsesArgs()
        {
            using (TestHostContext hc = CreateTestContext())
            {
                Tracing trace = hc.GetTrace();

                CommandLineParser clp = new(hc, secretArgNames: new string[0]);
                trace.Info("Constructed.");

                clp.Parse(new string[] { "cmd1", "--arg1", "arg1val", "--arg2", "arg2val" });
                trace.Info("Parsed");

                trace.Info("Args: {0}", clp.Args.Count);
                Assert.Equal(2, clp.Args.Count);
                Assert.True(clp.Args.ContainsKey("arg1"));
                Assert.Equal("arg1val", clp.Args["arg1"]);
                Assert.True(clp.Args.ContainsKey("arg2"));
                Assert.Equal("arg2val", clp.Args["arg2"]);
            }
        }

        [Fact]
        [Trait("Level", "L0")]
        [Trait("Category", "Common")]
        public void ParsesFlags()
        {
            using (TestHostContext hc = CreateTestContext())
            {
                Tracing trace = hc.GetTrace();

                CommandLineParser clp = new(hc, secretArgNames: new string[0]);
                trace.Info("Constructed.");

                clp.Parse(new string[] { "cmd1", "--flag1", "--arg1", "arg1val", "--flag2" });
                trace.Info("Parsed");

                trace.Info("Args: {0}", clp.Flags.Count);
                Assert.Equal(2, clp.Flags.Count);
                Assert.Contains("flag1", clp.Flags);
                Assert.Contains("flag2", clp.Flags);
            }
        }

        private TestHostContext CreateTestContext([CallerMemberName] string testName = "")
        {
            TestHostContext hc = new(this, testName);
            return hc;
        }
    }
}
