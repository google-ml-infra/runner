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
using System.Collections.Generic;
using System.Runtime.Serialization;
using GitHub.Actions.Expressions.Data;

namespace GitHub.Actions.WorkflowParser
{
    [DataContract]
    public sealed class StrategyConfiguration
    {
        /// <summary>
        /// Gets or sets the display name
        /// </summary>
        [DataMember(Name = "name", EmitDefaultValue = false)]
        public String Name { get; set; }

        [DataMember(Name = "id", EmitDefaultValue = false)]
        public String Id { get; set; }

        [IgnoreDataMember]
        public Dictionary<String, ExpressionData> ExpressionData
        {
            get
            {
                if (m_expressionData is null)
                {
                    m_expressionData = new Dictionary<String, ExpressionData>(StringComparer.Ordinal);
                }
                return m_expressionData;
            }
        }

        [DataMember(Name = "expressionData", EmitDefaultValue = false)]
        private Dictionary<String, ExpressionData> m_expressionData;
    }
}
