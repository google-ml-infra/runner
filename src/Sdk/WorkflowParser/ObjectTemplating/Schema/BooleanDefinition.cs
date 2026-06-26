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

using System;
using GitHub.Actions.WorkflowParser.ObjectTemplating.Tokens;

namespace GitHub.Actions.WorkflowParser.ObjectTemplating.Schema
{
    internal sealed class BooleanDefinition : ScalarDefinition
    {
        internal BooleanDefinition()
        {
        }

        internal BooleanDefinition(MappingToken definition)
            : base(definition)
        {
            foreach (var definitionPair in definition)
            {
                var definitionKey = definitionPair.Key.AssertString($"{TemplateConstants.Definition} key");
                switch (definitionKey.Value)
                {
                    case TemplateConstants.Boolean:
                        var mapping = definitionPair.Value.AssertMapping($"{TemplateConstants.Definition} {TemplateConstants.Boolean}");
                        foreach (var mappingPair in mapping)
                        {
                            var mappingKey = mappingPair.Key.AssertString($"{TemplateConstants.Definition} {TemplateConstants.Boolean} key");
                            switch (mappingKey.Value)
                            {
                                default:
                                    mappingKey.AssertUnexpectedValue($"{TemplateConstants.Definition} {TemplateConstants.Boolean} key");
                                    break;
                            }
                        }
                        break;

                    case TemplateConstants.CoerceRaw:
                        continue;

                    default:
                        definitionKey.AssertUnexpectedValue($"{TemplateConstants.Definition} key");
                        break;
                }
            }
        }

        internal override DefinitionType DefinitionType => DefinitionType.Boolean;

        internal override Boolean IsMatch(LiteralToken literal)
        {
            return literal is BooleanToken;
        }

        internal override void Validate(
            TemplateSchema schema,
            String name)
        {
        }
    }
}