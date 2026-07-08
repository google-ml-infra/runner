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
    /// Provides a contract for receiving messages from the task orchestrator.
    /// </summary>
    [DataContract]
    public sealed class TaskAgentMessage
    {
        /// <summary>
        /// Initializes an empty <c>TaskAgentMessage</c> instance.
        /// </summary>
        public TaskAgentMessage()
        {
        }

        /// <summary>
        /// Gets or sets the message identifier.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public Int64 MessageId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the message type, describing the data contract found in <c>TaskAgentMessage.Body</c>.
        /// </summary>
        [DataMember]
        public String MessageType
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the intialization vector used to encrypt this message.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public Byte[] IV
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the body of the message. If the <c>IV</c> property is provided the body will need to be
        /// decrypted using the <c>TaskAgentSession.EncryptionKey</c> value in addition to the <c>IV</c>.
        /// </summary>
        [DataMember]
        public String Body
        {
            get;
            set;
        }
    }
}
