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
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace GitHub.DistributedTask.WebApi
{
    public sealed class EndpointAuthorizationSchemes
    {
        public const String OAuth = "OAuth";
    }

    public sealed class EndpointAuthorizationParameters
    {
        public const String AccessToken = "AccessToken";
    }

    [DataContract]
    public sealed class EndpointAuthorization
    {
        public EndpointAuthorization()
        {
        }

        private EndpointAuthorization(EndpointAuthorization authorizationToClone)
        {
            this.Scheme = authorizationToClone.Scheme;
            if (authorizationToClone.m_parameters != null && authorizationToClone.m_parameters.Count > 0)
            {
                m_parameters = new Dictionary<String, String>(authorizationToClone.m_parameters, StringComparer.OrdinalIgnoreCase);
            }
        }

        /// <summary>
        /// Gets or sets the scheme used for service endpoint authentication.
        /// </summary>
        [DataMember]
        public String Scheme
        {
            get;
            set;
        }

        public IDictionary<String, String> Parameters
        {
            get
            {
                if (m_parameters == null)
                {
                    m_parameters = new Dictionary<String, String>(StringComparer.OrdinalIgnoreCase);
                }
                return m_parameters;
            }
        }

        public EndpointAuthorization Clone()
        {
            return new EndpointAuthorization(this);
        }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            SerializationHelper.Copy(ref m_serializedParameters, ref m_parameters, StringComparer.OrdinalIgnoreCase, true);
        }

        [OnSerializing]
        private void OnSerializing(StreamingContext context)
        {
            SerializationHelper.Copy(ref m_parameters, ref m_serializedParameters, StringComparer.OrdinalIgnoreCase);
        }

        [OnSerialized]
        private void OnSerialized(StreamingContext context)
        {
            m_serializedParameters = null;
        }

        private IDictionary<String, String> m_parameters;

        /// <summary>
        /// Gets or sets the parameters for the selected authorization scheme.
        /// </summary>
        [DataMember(Name = "Parameters", EmitDefaultValue = false)]
        private IDictionary<String, String> m_serializedParameters;
    }
}
