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

﻿using System;
using System.Collections.Generic;
using GitHub.DistributedTask.ObjectTemplating;

namespace GitHub.DistributedTask.Pipelines.ContextData
{
    internal static class TemplateMemoryExtensions
    {
        internal static void AddBytes(
            this TemplateMemory memory,
            PipelineContextData value,
            Boolean traverse)
        {
            var bytes = CalculateBytes(memory, value, traverse);
            memory.AddBytes(bytes);
        }

        internal static Int32 CalculateBytes(
            this TemplateMemory memory,
            PipelineContextData value,
            Boolean traverse)
        {
            var enumerable = traverse ? value.Traverse() : new[] { value } as IEnumerable<PipelineContextData>;
            var result = 0;
            foreach (var item in enumerable)
            {
                // This measurement doesn't have to be perfect
                // https://codeblog.jonskeet.uk/2011/04/05/of-memory-and-strings/
                switch (item?.Type)
                {
                    case PipelineContextDataType.String:
                        var str = item.AssertString("string").Value;
                        checked
                        {
                            result += TemplateMemory.MinObjectSize + TemplateMemory.StringBaseOverhead + ((str?.Length ?? 0) * sizeof(Char));
                        }
                        break;

                    case PipelineContextDataType.Array:
                    case PipelineContextDataType.Dictionary:
                    case PipelineContextDataType.Boolean:
                    case PipelineContextDataType.Number:
                        // Min object size is good enough. Allows for base + a few fields.
                        checked
                        {
                            result += TemplateMemory.MinObjectSize;
                        }
                        break;

                    case null:
                        checked
                        {
                            result += IntPtr.Size;
                        }
                        break;

                    default:
                        throw new NotSupportedException($"Unexpected pipeline context data type '{item.Type}'");
                }
            }

            return result;
        }
    }
}
