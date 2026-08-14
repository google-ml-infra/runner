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

using System.Runtime.Serialization;

namespace GitHub.DistributedTask.WebApi
{
    [DataContract]
    public sealed class DiagnosticLogMetadata
    {
        public DiagnosticLogMetadata(string agentName, ulong agentId, int poolId, string phaseName, string fileName, string phaseResult)
        {
            AgentName = agentName;
            AgentId = agentId;
            PoolId = poolId;
            PhaseName = phaseName;
            FileName = fileName;
            PhaseResult = phaseResult;
        }

        [DataMember]
        public string AgentName { get; set; }

        [DataMember]
        public ulong AgentId { get; set; }

        [DataMember]
        public int PoolId { get; set; }

        [DataMember]
        public string PhaseName { get; set; }

        [DataMember]
        public string FileName { get; set; }

        [DataMember]
        public string PhaseResult { get; set; }
    }
}
