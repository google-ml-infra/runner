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
using GitHub.Services.Common;

namespace GitHub.DistributedTask.WebApi
{
    /// <summary>
    /// Represents the public key portion of an RSA asymmetric key.
    /// </summary>
    [DataContract]
    public sealed class TaskAgentPublicKey
    {
        /// <summary>
        /// Initializes a new <c>TaskAgentPublicKey</c> instance with empty exponent and modulus values.
        /// </summary>
        public TaskAgentPublicKey()
        {
        }

        /// <summary>
        /// Initializes a new <c>TaskAgentPublicKey</c> instance with the specified exponent and modulus values.
        /// </summary>
        /// <param name="exponent">The exponent value of the key</param>
        /// <param name="modulus">The modulus value of the key</param>
        public TaskAgentPublicKey(
            Byte[] exponent,
            Byte[] modulus)
        {
            ArgumentUtility.CheckEnumerableForNullOrEmpty(exponent, nameof(exponent));
            ArgumentUtility.CheckEnumerableForNullOrEmpty(modulus, nameof(modulus));

            this.Exponent = exponent;
            this.Modulus = modulus;
        }

        private TaskAgentPublicKey(TaskAgentPublicKey objectToBeCloned)
        {
            if (objectToBeCloned.Exponent != null)
            {
                this.Exponent = new Byte[objectToBeCloned.Exponent.Length];
                Buffer.BlockCopy(objectToBeCloned.Exponent, 0, this.Exponent, 0, objectToBeCloned.Exponent.Length);
            }

            if (objectToBeCloned.Modulus != null)
            {
                this.Modulus = new Byte[objectToBeCloned.Modulus.Length];
                Buffer.BlockCopy(objectToBeCloned.Modulus, 0, this.Modulus, 0, objectToBeCloned.Modulus.Length);
            }
        }

        /// <summary>
        /// Gets or sets the exponent for the public key.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public Byte[] Exponent
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the modulus for the public key.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public Byte[] Modulus
        {
            get;
            set;
        }

        public TaskAgentPublicKey Clone()
        {
            return new TaskAgentPublicKey(this);
        }
    }
}
