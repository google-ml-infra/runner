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

using System;
using System.Runtime.Serialization;

namespace GitHub.DistributedTask.WebApi
{
    /// <summary>
    /// Represents a session for performing message exchanges from an agent.
    /// </summary>
    [DataContract]
    public class TaskAgentSession
    {
        public TaskAgentSession()
        {
        }

        /// <summary>
        /// Initializes a new <c>TaskAgentSession</c> instance with the specified owner name and agent.
        /// </summary>
        /// <param name="ownerName">The name of the owner for this session. This should typically be the agent machine</param>
        /// <param name="agent">The target agent for the session</param>
        public TaskAgentSession(
            String ownerName,
            TaskAgentReference agent)
        {
            this.Agent = agent;
            this.OwnerName = ownerName;
        }

        /// <summary>
        /// Gets the unique identifier for this session.
        /// </summary>
        [DataMember]
        public Guid SessionId
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the key used to encrypt message traffic for this session.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public TaskAgentSessionKey EncryptionKey
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets or sets the owner name of this session. Generally this will be the machine of origination.
        /// </summary>
        [DataMember]
        public String OwnerName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the agent which is the target of the session.
        /// </summary>
        [DataMember]
        public TaskAgentReference Agent
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets whether to use FIPS compliant encryption scheme for job message key
        /// </summary>
        [DataMember]
        public bool UseFipsEncryption
        {
            get;
            set;
        }

        [DataMember(EmitDefaultValue = false, IsRequired = false)]
        public BrokerMigrationMessage BrokerMigrationMessage
        {
            get;
            set;
        }
    }
}
