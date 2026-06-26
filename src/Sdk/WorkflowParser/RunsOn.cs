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

namespace GitHub.Actions.WorkflowParser
{
    [DataContract]
    public class RunsOn
    {
        public HashSet<string> Labels
        {
            get
            {
                if (m_labels == null)
                {
                    m_labels = new HashSet<string>();
                }
                return m_labels;
            }
        }

        [DataMember(EmitDefaultValue = false)]
        public String RunnerGroup { get; set; }

        [OnSerializing]
        private void OnSerializing(StreamingContext context)
        {
            if (m_labels?.Count == 0)
            {
                m_labels = null;
            }
        }

        [DataMember(Name = "Labels", EmitDefaultValue = false)]
        private HashSet<string> m_labels;
    }
}
