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

﻿using System;
using System.Runtime.Serialization;

namespace GitHub.DistributedTask.WebApi
{
    /// <summary>
    /// Represents a symmetric key used for message-level encryption for communication sent to an agent.
    /// </summary>
    [DataContract]
    public sealed class TaskAgentSessionKey
    {
        /// <summary>
        /// Gets or sets a value indicating whether or not the key value is encrypted. If this value is true, the 
        /// <see cref="Value"/> property should be decrypted using the <c>RSA</c> key exchanged with the server during
        /// registration.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public Boolean Encrypted
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the symmetric key value.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public Byte[] Value
        {
            get;
            set;
        }
    }
}
