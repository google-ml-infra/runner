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
using System.Runtime.Serialization;

namespace GitHub.Services.WebApi
{
    /// <summary>
    /// The class to represent a REST reference link.
    /// 
    /// RFC: http://tools.ietf.org/html/draft-kelly-json-hal-06
    /// 
    /// The RFC is not fully implemented, additional properties are allowed on the
    /// reference link but as of yet we don't have a need for them.
    /// </summary>
    [DataContract]
    public class ReferenceLink : ISecuredObject
    {
        public ReferenceLink() { }

        internal ReferenceLink(ISecuredObject securedObject)
        {
            m_securedObject = securedObject;
        }

        [DataMember]
        public string Href { get; set; }

        Guid ISecuredObject.NamespaceId
        {
            get
            {
                ArgumentUtility.CheckForNull(m_securedObject, nameof(m_securedObject));
                return m_securedObject.NamespaceId;
            }
        }

        int ISecuredObject.RequiredPermissions
        {
            get
            {
                ArgumentUtility.CheckForNull(m_securedObject, nameof(m_securedObject));
                return m_securedObject.RequiredPermissions;
            }
        }

        string ISecuredObject.GetToken()
        {
            ArgumentUtility.CheckForNull(m_securedObject, nameof(m_securedObject));
            return m_securedObject.GetToken();
        }

        private readonly ISecuredObject m_securedObject;
    }
}
