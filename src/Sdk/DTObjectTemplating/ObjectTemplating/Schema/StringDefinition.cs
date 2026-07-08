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
using GitHub.DistributedTask.ObjectTemplating.Tokens;

namespace GitHub.DistributedTask.ObjectTemplating.Schema
{
    internal sealed class StringDefinition : ScalarDefinition
    {
        internal StringDefinition()
        {
        }

        internal StringDefinition(MappingToken definition)
            : base(definition)
        {
            foreach (var definitionPair in definition)
            {
                var definitionKey = definitionPair.Key.AssertString($"{TemplateConstants.Definition} key");
                switch (definitionKey.Value)
                {
                    case TemplateConstants.String:
                        var mapping = definitionPair.Value.AssertMapping($"{TemplateConstants.Definition} {TemplateConstants.String}");
                        foreach (var mappingPair in mapping)
                        {
                            var mappingKey = mappingPair.Key.AssertString($"{TemplateConstants.Definition} {TemplateConstants.String} key");
                            switch (mappingKey.Value)
                            {
                                case TemplateConstants.Constant:
                                    var constantStringToken = mappingPair.Value.AssertString($"{TemplateConstants.Definition} {TemplateConstants.String} {TemplateConstants.Constant}");
                                    Constant = constantStringToken.Value;
                                    break;

                                case TemplateConstants.IgnoreCase:
                                    var ignoreCaseBooleanToken = mappingPair.Value.AssertBoolean($"{TemplateConstants.Definition} {TemplateConstants.String} {TemplateConstants.IgnoreCase}");
                                    IgnoreCase = ignoreCaseBooleanToken.Value;
                                    break;

                                case TemplateConstants.RequireNonEmpty:
                                    var requireNonEmptyBooleanToken = mappingPair.Value.AssertBoolean($"{TemplateConstants.Definition} {TemplateConstants.String} {TemplateConstants.RequireNonEmpty}");
                                    RequireNonEmpty = requireNonEmptyBooleanToken.Value;
                                    break;

                                default:
                                    mappingKey.AssertUnexpectedValue($"{TemplateConstants.Definition} {TemplateConstants.String} key");
                                    break;
                            }
                        }
                        break;

                    default:
                        definitionKey.AssertUnexpectedValue($"{TemplateConstants.Definition} key");
                        break;
                }
            }
        }

        internal override DefinitionType DefinitionType => DefinitionType.String;

        internal String Constant { get; set; }

        internal Boolean IgnoreCase { get; set; }

        internal Boolean RequireNonEmpty { get; set; }

        internal override Boolean IsMatch(LiteralToken literal)
        {
            if (literal is StringToken str)
            {
                var value = str.Value;
                if (!String.IsNullOrEmpty(Constant))
                {
                    var comparison = IgnoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;

                    if (String.Equals(Constant, value, comparison))
                    {
                        return true;
                    }
                }
                else if (RequireNonEmpty)
                {
                    if (!String.IsNullOrEmpty(value))
                    {
                        return true;
                    }
                }
                else
                {
                    return true;
                }
            }

            return false;
        }

        internal override void Validate(
            TemplateSchema schema,
            String name)
        {
            if (!String.IsNullOrEmpty(Constant) && RequireNonEmpty)
            {
                throw new ArgumentException($"Properties '{TemplateConstants.Constant}' and '{TemplateConstants.RequireNonEmpty}' cannot both be set");
            }
        }
    }
}
