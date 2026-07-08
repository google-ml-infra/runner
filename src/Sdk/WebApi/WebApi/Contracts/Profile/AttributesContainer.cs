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

﻿using GitHub.Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace GitHub.Services.Profile
{
    /// <summary>
    /// Stores a set of named profile attributes.
    /// </summary>
    [DataContract]
    public class AttributesContainer : IVersioned, ICloneable
    {
        public AttributesContainer(string containerName) : this()
        {
            ContainerName = containerName;
        }

        public AttributesContainer()
        {
            Attributes = new Dictionary<string, ProfileAttribute>(VssStringComparer.AttributesDescriptor);
        }

        /// <summary>
        /// The name of the container.
        /// </summary>
        [DataMember(IsRequired = true, EmitDefaultValue = false)]
        public string ContainerName
        {
            get
            {
                return m_containerName;
            }
            set
            {
                ProfileArgumentValidation.ValidateContainerName(value);
                m_containerName = value;
            }
        }

        public object Clone()
        {
            AttributesContainer newContainer = (AttributesContainer)MemberwiseClone();

            // Deep copy of attributes dictionary
            newContainer.Attributes = Attributes != null ? Attributes.ToDictionary(x => x.Key, x => (ProfileAttribute)x.Value.Clone()) : null;

            return newContainer;
        }

        /// <summary>
        /// The attributes stored by the container.
        /// </summary>
        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        public IDictionary<string, ProfileAttribute> Attributes { get; set; }

        /// <summary>
        /// The maximum revision number of any attribute within the container.
        /// </summary>
        [DataMember(IsRequired = true, EmitDefaultValue = false)]
        public int Revision { get; set; }

        private string m_containerName;
    }
}
