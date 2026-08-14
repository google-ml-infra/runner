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
    /// Provides data necessary for authorizing the agent using OAuth 2.0 authentication flows.
    /// </summary>
    [DataContract]
    public sealed class TaskAgentAuthorization
    {
        /// <summary>
        /// Initializes a new <c>TaskAgentAuthorization</c> instance with default values.
        /// </summary>
        public TaskAgentAuthorization()
        {
        }

        private TaskAgentAuthorization(TaskAgentAuthorization objectToBeCloned)
        {
            this.AuthorizationUrl = objectToBeCloned.AuthorizationUrl;
            this.ClientId = objectToBeCloned.ClientId;

            if (objectToBeCloned.PublicKey != null)
            {
                this.PublicKey = objectToBeCloned.PublicKey.Clone();
            }
        }

        /// <summary>
        /// Endpoint used to obtain access tokens from the configured token service.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public Uri AuthorizationUrl
        {
            get;
            set;
        }

        /// <summary>
        /// Client identifier for this agent.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public Guid ClientId
        {
            get;
            set;
        }

        /// <summary>
        /// Public key used to verify the identity of this agent.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public TaskAgentPublicKey PublicKey
        {
            get;
            set;
        }

        /// <summary>
        /// Creates a deep copy of the authorization data.
        /// </summary>
        /// <returns>A new <c>TaskAgentAuthorization</c> instance copied from the current instance</returns>
        public TaskAgentAuthorization Clone()
        {
            return new TaskAgentAuthorization(this);
        }
    }
}
