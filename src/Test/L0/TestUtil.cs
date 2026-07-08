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

﻿using System.IO;
using Xunit;
using GitHub.Runner.Sdk;
using System.Runtime.CompilerServices;

namespace GitHub.Runner.Common.Tests
{
    public static class TestUtil
    {
        private const string Src = "src";
        private const string TestData = "TestData";

        public static string GetProjectPath(string name = "Test")
        {
            ArgUtil.NotNullOrEmpty(name, nameof(name));
            string projectDir = Path.Combine(
                GetSrcPath(),
                name);
            Assert.True(Directory.Exists(projectDir));
            return projectDir;
        }

        public static string GetTestFilePath([CallerFilePath] string path = null)
        {
            return path;
        }

        public static string GetSrcPath()
        {
            string L0dir = Path.GetDirectoryName(GetTestFilePath());
            string testDir = Path.GetDirectoryName(L0dir);
            string srcDir = Path.GetDirectoryName(testDir);
            ArgUtil.Directory(srcDir, nameof(srcDir));
            Assert.Equal(Src, Path.GetFileName(srcDir));
            return srcDir;
        }

        public static string GetTestDataPath()
        {
            string testDataDir = Path.Combine(GetProjectPath(), TestData);
            Assert.True(Directory.Exists(testDataDir));
            return testDataDir;
        }
    }
}
