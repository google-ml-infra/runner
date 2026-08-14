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

namespace GitHub.Actions.Expressions
{
    public sealed class EvaluationOptions
    {
        public EvaluationOptions()
        {
        }

        public EvaluationOptions(EvaluationOptions copy)
        {
            if (copy != null)
            {
                MaxMemory = copy.MaxMemory;
                MaxCacheMemory = copy.MaxCacheMemory;
                StrictJsonParsing = copy.StrictJsonParsing;
                AlwaysTraceExpanded = copy.AlwaysTraceExpanded;
            }
        }

        /// <summary>
        /// Maximum memory (in bytes) allowed during expression evaluation.
        /// Memory is tracked across the entire expression tree evaluation to protect against DOS attacks.
        /// Default is 1 MB (1048576 bytes) if not specified.
        /// </summary>
        public Int32 MaxMemory { get; set; }

        /// <summary>
        /// Maximum memory (in bytes) allowed for caching expanded expression results during tracing.
        /// When exceeded, the cache is cleared and expressions may not be fully expanded in trace output.
        /// Default is 1 MB (1048576 bytes) if not specified.
        /// </summary>
        public Int32 MaxCacheMemory { get; set; }

        /// <summary>
        /// Whether to enforce strict JSON parsing in the fromJson function.
        /// When true, rejects JSON with comments, trailing commas, single quotes, and other non-standard features.
        /// Default is false if not specified.
        /// </summary>
        public Boolean StrictJsonParsing { get; set; }

        /// <summary>
        /// Whether to always include the expanded expression in trace output.
        /// When true, the expanded expression is always traced even if it matches the original expression or result.
        /// Default is false if not specified.
        /// </summary>
        public Boolean AlwaysTraceExpanded { get; set; }
    }
}
