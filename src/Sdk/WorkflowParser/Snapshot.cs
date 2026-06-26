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

#nullable enable

using System.Runtime.Serialization;
using GitHub.Actions.WorkflowParser.ObjectTemplating.Tokens;

namespace GitHub.Actions.WorkflowParser
{
    [DataContract]
    public class Snapshot
    {
        [DataMember(EmitDefaultValue = false)]
        public required string ImageName { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public BasicExpressionToken? If
        {
            get;
            set;
        }

        [DataMember(EmitDefaultValue = false)]
        public required string Version  { get; set; }
    }
}
