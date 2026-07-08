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
using System.ComponentModel;
using Newtonsoft.Json.Linq;

namespace GitHub.DistributedTask.Pipelines.ContextData
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class JTokenExtensions
    {
        public static PipelineContextData ToPipelineContextData(this JToken value)
        {
            return value.ToPipelineContextData(1, 100);
        }

        public static PipelineContextData ToPipelineContextData(
            this JToken value,
            Int32 depth,
            Int32 maxDepth)
        {
            if (depth < maxDepth)
            {
                if (value.Type == JTokenType.String)
                {
                    return new StringContextData((String)value);
                }
                else if (value.Type == JTokenType.Boolean)
                {
                    return new BooleanContextData((Boolean)value);
                }
                else if (value.Type == JTokenType.Float || value.Type == JTokenType.Integer)
                {
                    return new NumberContextData((Double)value);
                }
                else if (value.Type == JTokenType.Object)
                {
                    var subContext = new DictionaryContextData();
                    var obj = (JObject)value;
                    foreach (var property in obj.Properties())
                    {
                        subContext[property.Name] = ToPipelineContextData(property.Value, depth + 1, maxDepth);
                    }
                    return subContext;
                }
                else if (value.Type == JTokenType.Array)
                {
                    var arrayContext = new ArrayContextData();
                    var arr = (JArray)value;
                    foreach (var element in arr)
                    {
                        arrayContext.Add(ToPipelineContextData(element, depth + 1, maxDepth));
                    }
                    return arrayContext;
                }
                else if (value.Type == JTokenType.Null)
                {
                    return null;
                }
            }

            // We don't understand the type or have reached our max, return as string
            return new StringContextData(value.ToString());
        }
    }
}
