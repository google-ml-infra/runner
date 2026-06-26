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

﻿using Newtonsoft.Json;
using System;
using System.Runtime.Serialization;


namespace GitHub.DistributedTask.WebApi
{
    [DataContract]
    public sealed class AgentRefreshMessage
    {
        public static readonly String MessageType = "AgentRefresh";

        [JsonConstructor]
        internal AgentRefreshMessage()
        {
        }

        public AgentRefreshMessage(
            ulong agentId,
            String targetVersion,
            TimeSpan? timeout = null)
        {
            this.AgentId = agentId;
            this.Timeout = timeout ?? TimeSpan.FromMinutes(60);
            this.TargetVersion = targetVersion;
        }

        [DataMember]
        public ulong AgentId
        {
            get;
            private set;
        }

        [DataMember]
        public TimeSpan Timeout
        {
            get;
            private set;
        }

        [DataMember]
        public String TargetVersion
        {
            get;
            private set;
        }
    }
}
