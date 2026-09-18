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

namespace GitHub.DistributedTask.Logging
{
    internal sealed class ReplacementPosition
    {
        public ReplacementPosition(Int32 start, Int32 length)
        {
            Start = start;
            Length = length;
        }

        public ReplacementPosition(ReplacementPosition copy)
        {
            Start = copy.Start;
            Length = copy.Length;
        }

        public Int32 Start { get; set; }
        public Int32 Length { get; set; }
        public Int32 End
        {
            get
            {
                return Start + Length;
            }
        }
    }
}
