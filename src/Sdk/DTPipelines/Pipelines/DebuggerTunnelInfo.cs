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

namespace GitHub.DistributedTask.Pipelines
{
    /// <summary>
    /// Dev Tunnel information the runner needs to host the debugger tunnel.
    /// Matches the run-service <c>DebuggerTunnel</c> contract.
    /// </summary>
    [DataContract]
    public sealed class DebuggerTunnelInfo
    {
        [DataMember(EmitDefaultValue = false)]
        public string TunnelId { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string ClusterId { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string HostToken { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public ushort Port { get; set; }
    }
}
