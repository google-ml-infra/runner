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
using GitHub.Actions.Expressions.Data;
using GitHub.Actions.WorkflowParser.ObjectTemplating.Tokens;

namespace GitHub.Actions.WorkflowParser.Conversion
{
    internal static class TemplateTokenExtensions
    {
        public static ArrayExpressionData ToExpressionData(this SequenceToken sequence)
        {
            var token = sequence as TemplateToken;
            var expressionData = token.ToExpressionData();
            return expressionData.AssertArray("converted sequence token");
        }

        public static DictionaryExpressionData ToExpressionData(this MappingToken mapping)
        {
            var token = mapping as TemplateToken;
            var expressionData = token.ToExpressionData();
            return expressionData.AssertDictionary("converted mapping token");
        }

        public static ExpressionData ToExpressionData(this TemplateToken token)
        {
            switch (token.Type)
            {
                case TokenType.Mapping:
                    var mapping = token as MappingToken;
                    var dictionary = new DictionaryExpressionData();
                    if (mapping.Count > 0)
                    {
                        foreach (var pair in mapping)
                        {
                            var keyLiteral = pair.Key.AssertString("dictionary context data key");
                            var key = keyLiteral.Value;
                            var value = pair.Value.ToExpressionData();
                            dictionary.Add(key, value);
                        }
                    }
                    return dictionary;

                case TokenType.Sequence:
                    var sequence = token as SequenceToken;
                    var array = new ArrayExpressionData();
                    if (sequence.Count > 0)
                    {
                        foreach (var item in sequence)
                        {
                            array.Add(item.ToExpressionData());
                        }
                    }
                    return array;

                case TokenType.Null:
                    return null;

                case TokenType.Boolean:
                    var boolean = token as BooleanToken;
                    return new BooleanExpressionData(boolean.Value);

                case TokenType.Number:
                    var number = token as NumberToken;
                    return new NumberExpressionData(number.Value);

                case TokenType.String:
                    var stringToken = token as StringToken;
                    return new StringExpressionData(stringToken.Value);

                default:
                    throw new NotSupportedException($"Unexpected {nameof(TemplateToken)} type '{token.Type}'");
            }
        }
    }
}
