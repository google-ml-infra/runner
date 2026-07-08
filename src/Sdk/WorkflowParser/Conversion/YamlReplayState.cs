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

#nullable disable // Consider removing in the future to minimize likelihood of NullReferenceException; refer https://learn.microsoft.com/en-us/dotnet/csharp/nullable-references

using System;

namespace GitHub.Actions.WorkflowParser.Conversion
{
    /// <summary>
    /// Index and depth while replaying a YAML anchor
    /// </summary>
    sealed class YamlReplayState
    {
        /// <summary>
        /// Gets or sets the current node event index that is being replayed.
        /// </summary>
        public Int32 Index { get; set; }

        /// <summary>
        /// Gets or sets the depth within the current anchor that is being replayed.
        /// When the depth reaches zero, the anchor replay is complete.
        /// </summary>
        public Int32 Depth { get; set; }
    }
}
