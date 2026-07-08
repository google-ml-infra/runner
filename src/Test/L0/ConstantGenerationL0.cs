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

﻿using System.Collections.Generic;
using GitHub.Runner.Sdk;
using Xunit;


namespace GitHub.Runner.Common.Tests
{
    public sealed class ConstantGenerationL0
    {
        [Fact]
        [Trait("Level", "L0")]
        [Trait("Category", "Runner")]
        public void BuildConstantGenerateSucceed()
        {
            List<string> validPackageNames = new()
            {
                "win-x64",
                "win-x86",
                "win-arm64",
                "linux-x64",
                "linux-arm",
                "linux-arm64",
                "osx-x64",
                "osx-arm64"
            };

            Assert.True(
                BuildConstants.Source.CommitHash.Length == 40 || BuildConstants.Source.CommitHash.Length == 64,
                "CommitHash should be a 40-char SHA-1 or 64-char SHA-256 hex string");
            Assert.Matches("^[0-9a-f]+$", BuildConstants.Source.CommitHash);
            Assert.True(validPackageNames.Contains(BuildConstants.RunnerPackage.PackageName), $"PackageName should be one of the following '{string.Join(", ", validPackageNames)}', current PackageName is '{BuildConstants.RunnerPackage.PackageName}'");
        }
    }
}
