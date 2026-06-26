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

﻿using System.IO;

namespace GitHub.Runner.Sdk
{
    public static class PathUtil
    {
#if OS_WINDOWS
        public static readonly string PathVariable = "Path";
#else
        public static readonly string PathVariable = "PATH";
#endif

        public static string PrependPath(string path, string currentPath)
        {
            ArgUtil.NotNullOrEmpty(path, nameof(path));
            if (string.IsNullOrEmpty(currentPath))
            {
                // Careful not to add a trailing separator if the PATH is empty.
                // On OSX/Linux, a trailing separator indicates that "current directory"
                // is added to the PATH, which is considered a security risk.
                return path;
            }

            // Not prepend path if it is already the first path in %PATH%
            if (currentPath.StartsWith(path + Path.PathSeparator, IOUtil.FilePathStringComparison))
            {
                return currentPath;
            }
            else
            {
                return path + Path.PathSeparator + currentPath;
            }
        }
    }
}
