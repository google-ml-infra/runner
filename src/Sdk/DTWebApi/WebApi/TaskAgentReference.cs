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

﻿using GitHub.Services.WebApi;
using System;
using System.Runtime.Serialization;

namespace GitHub.DistributedTask.WebApi
{
    /// <summary>
    /// A reference to an agent.
    /// </summary>
    [DataContract]
    public class TaskAgentReference : ICloneable
    {
        public TaskAgentReference()
        {
        }

        protected TaskAgentReference(TaskAgentReference referenceToBeCloned)
        {
            this.Id = referenceToBeCloned.Id;
            this.Name = referenceToBeCloned.Name;
            this.Version = referenceToBeCloned.Version;
            this.Enabled = referenceToBeCloned.Enabled;
            this.Status = referenceToBeCloned.Status;
            this.OSDescription = referenceToBeCloned.OSDescription;
            this.ProvisioningState = referenceToBeCloned.ProvisioningState;
            this.AccessPoint = referenceToBeCloned.AccessPoint;
            this.Ephemeral = referenceToBeCloned.Ephemeral;
            this.DisableUpdate = referenceToBeCloned.DisableUpdate;

            if (referenceToBeCloned.m_links != null)
            {
                m_links = referenceToBeCloned.m_links.Clone();
            }
        }

        /// <summary>
        /// Identifier of the agent.
        /// </summary>
        [DataMember]
        public ulong Id
        {
            get;
            set;
        }

        /// <summary>
        /// Name of the agent.
        /// </summary>
        [DataMember]
        public String Name
        {
            get;
            set;
        }

        /// <summary>
        /// Agent version.
        /// </summary>
        [DataMember]
        public String Version
        {
            get;
            set;
        }

        /// <summary>
        /// Agent OS.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public String OSDescription
        {
            get;
            set;
        }

        /// <summary>
        /// Whether or not this agent should run jobs.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public Boolean? Enabled
        {
            get;
            set;
        }

        /// <summary>
        /// Signifies that this Agent can only run one job and will be removed by the server after that one job finish.
        /// </summary>
        [DataMember]
        public bool? Ephemeral
        {
            get;
            set;
        }

        /// <summary>
        /// Whether or not this agent should auto-update to latest version.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public bool? DisableUpdate
        {
            get;
            set;
        }

        /// <summary>
        /// Whether or not the agent is online.
        /// </summary>
        [DataMember]
        public TaskAgentStatus Status
        {
            get;
            set;
        }

        /// <summary>
        /// Provisioning state of this agent.
        /// </summary>
        [DataMember]
        public String ProvisioningState
        {
            get;
            set;
        }

        /// <summary>
        /// This agent's access point.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public String AccessPoint
        {
            get;
            set;
        }

        /// <summary>
        /// Other details about the agent.
        /// </summary>
        public ReferenceLinks Links
        {
            get
            {
                if (m_links == null)
                {
                    m_links = new ReferenceLinks();
                }
                return m_links;
            }
            internal set
            {
                m_links = value;
            }
        }

        Object ICloneable.Clone()
        {
            return this.Clone();
        }

        public TaskAgentReference Clone()
        {
            return new TaskAgentReference(this);
        }

        [DataMember(Name = "_links", EmitDefaultValue = false)]
        private ReferenceLinks m_links;
    }
}
