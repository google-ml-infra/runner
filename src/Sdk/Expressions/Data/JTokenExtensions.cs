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
using Newtonsoft.Json.Linq;

namespace GitHub.Actions.Expressions.Data
{
    public static class JTokenExtensions
    {
        public static ExpressionData ToExpressionData(this JToken value)
        {
            return value.ToExpressionData(1, 100);
        }

        public static ExpressionData ToExpressionData(
            this JToken value, 
            Int32 depth, 
            Int32 maxDepth)
        {
            if (depth < maxDepth)
            {
                if (value.Type == JTokenType.String)
                {
                    return new StringExpressionData((String)value);
                }
                else if (value.Type == JTokenType.Boolean)
                {
                    return new BooleanExpressionData((Boolean)value);
                }
                else if (value.Type == JTokenType.Float || value.Type == JTokenType.Integer)
                {
                    return new NumberExpressionData((Double)value);
                }
                else if (value.Type == JTokenType.Object)
                {
                    var subContext = new DictionaryExpressionData();
                    var obj = (JObject)value;
                    foreach (var property in obj.Properties())
                    {
                        subContext[property.Name] = ToExpressionData(property.Value, depth + 1, maxDepth);
                    }
                    return subContext;
                }
                else if (value.Type == JTokenType.Array)
                {
                    var arrayContext = new ArrayExpressionData();
                    var arr = (JArray)value;
                    foreach (var element in arr)
                    {
                        arrayContext.Add(ToExpressionData(element, depth + 1, maxDepth));
                    }
                    return arrayContext;
                }
                else if (value.Type == JTokenType.Null)
                {
                    return null;
                }
            }

            // We don't understand the type or have reached our max, return as string
            return new StringExpressionData(value.ToString());
        }
    }
}
