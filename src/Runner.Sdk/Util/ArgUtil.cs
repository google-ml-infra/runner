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
using System.IO;

namespace GitHub.Runner.Sdk
{
    public static class ArgUtil
    {
        public static void Directory(string directory, string name)
        {
            ArgUtil.NotNullOrEmpty(directory, name);
            if (!System.IO.Directory.Exists(directory))
            {
                throw new DirectoryNotFoundException(
                    message: $"Directory not found: '{directory}'");
            }
        }

        public static void Equal<T>(T expected, T actual, string name)
        {
            if (object.ReferenceEquals(expected, actual))
            {
                return;
            }

            if (object.ReferenceEquals(expected, null) ||
                !expected.Equals(actual))
            {
                throw new ArgumentOutOfRangeException(
                    paramName: name,
                    actualValue: actual,
                    message: $"{name} does not equal expected value. Expected '{expected}'. Actual '{actual}'.");
            }
        }

        public static void File(string fileName, string name)
        {
            ArgUtil.NotNullOrEmpty(fileName, name);
            if (!System.IO.File.Exists(fileName))
            {
                throw new FileNotFoundException(
                    message: $"File not found: '{fileName}'",
                    fileName: fileName);
            }
        }

        public static void NotNull(object value, string name)
        {
            if (object.ReferenceEquals(value, null))
            {
                throw new ArgumentNullException(name);
            }
        }

        public static void NotNullOrEmpty(string value, string name)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException(name);
            }
        }

        public static void NotEmpty(Guid value, string name)
        {
            if (value == Guid.Empty)
            {
                throw new ArgumentNullException(name);
            }
        }

        public static void Null(object value, string name)
        {
            if (!object.ReferenceEquals(value, null))
            {
                throw new ArgumentException(message: $"{name} should be null.", paramName: name);
            }
        }
    }
}
