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

#nullable disable // Consider removing in the future to minimize likelihood of NullReferenceException; refer https://learn.microsoft.com/en-us/dotnet/csharp/nullable-references

using System;
using System.IO;
using GitHub.Actions.Expressions.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GitHub.Actions.Expressions.Sdk.Functions
{
    internal sealed class FromJson : Function
    {
        protected sealed override Object EvaluateCore(
            EvaluationContext context,
            out ResultMemory resultMemory)
        {
            resultMemory = null;
            var json = Parameters[0].Evaluate(context).ConvertToString();

            if (context.Options.StrictJsonParsing)
            {
                try
                {
                    return JsonParser.Parse(json);
                }
                catch (System.Text.Json.JsonException ex)
                {
                    throw new System.Text.Json.JsonException($"Error parsing fromJson: {ex.Message}", ex);
                }
                catch (Exception ex)
                {
                    throw new System.Text.Json.JsonException($"Unexpected error parsing fromJson: {ex.Message}", ex);
                }
            }

            try
            {
                using var stringReader = new StringReader(json);
                using var jsonReader = new JsonTextReader(stringReader) { DateParseHandling = DateParseHandling.None, FloatParseHandling = FloatParseHandling.Double };
                var token = JToken.ReadFrom(jsonReader);
                return token.ToExpressionData();
            }
            catch (JsonReaderException ex)
            {
                throw new JsonReaderException("Error parsing fromJson", ex);
            }
        }
    }
}
