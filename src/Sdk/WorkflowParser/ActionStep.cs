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

#nullable enable

using System.Runtime.Serialization;
using GitHub.Actions.WorkflowParser.ObjectTemplating.Tokens;

namespace GitHub.Actions.WorkflowParser
{
    [DataContract]
    public sealed class ActionStep : IStep
    {
        [DataMember(Order = 0, Name = "id", EmitDefaultValue = false)]
        public string? Id
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the display name
        /// </summary>
        [DataMember(Order = 1, Name = "name", EmitDefaultValue = false)]
        public ScalarToken? Name
        {
            get;
            set;
        }

        [DataMember(Order = 2, Name = "if", EmitDefaultValue = false)]
        public BasicExpressionToken? If
        {
            get;
            set;
        }

        [DataMember(Order = 3, Name = "continue-on-error", EmitDefaultValue = false)]
        public ScalarToken? ContinueOnError
        {
            get;
            set;
        }

        [DataMember(Order = 4, Name = "timeout-minutes", EmitDefaultValue = false)]
        public ScalarToken? TimeoutMinutes
        {
            get;
            set;
        }

        [DataMember(Order = 5, Name = "env", EmitDefaultValue = false)]
        public TemplateToken? Env
        {
            get;
            set;
        }

        [DataMember(Order = 6, Name = "uses", EmitDefaultValue = false)]
        public StringToken? Uses
        {
            get;
            set;
        }

        [DataMember(Order = 7, Name = "with", EmitDefaultValue = false)]
        public TemplateToken? With
        {
            get;
            set;
        }

        public IStep Clone(bool omitSource)
        {
            return new ActionStep
            {
                ContinueOnError = ContinueOnError?.Clone(omitSource) as ScalarToken,
                Env = Env?.Clone(omitSource),
                Id = Id,
                If = If?.Clone(omitSource) as BasicExpressionToken,
                Name = Name?.Clone(omitSource) as ScalarToken,
                TimeoutMinutes = TimeoutMinutes?.Clone(omitSource) as ScalarToken,
                Uses = Uses?.Clone(omitSource) as StringToken,
                With = With?.Clone(omitSource),
            };
        }
    }
}
