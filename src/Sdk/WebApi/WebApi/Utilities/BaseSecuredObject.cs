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

namespace GitHub.Services.WebApi
{
    [DataContract]
    public abstract class BaseSecuredObject : ISecuredObject
    {
        protected BaseSecuredObject()
        {
        }

        protected BaseSecuredObject(ISecuredObject securedObject)
        {
            if (securedObject != null)
            {
                this.m_namespaceId = securedObject.NamespaceId;
                this.m_requiredPermissions = securedObject.RequiredPermissions;
                this.m_token = securedObject.GetToken();
            }
        }

        Guid ISecuredObject.NamespaceId
        {
            get
            {
                return m_namespaceId;
            }
        }

        int ISecuredObject.RequiredPermissions
        {
            get
            {
                return m_requiredPermissions;
            }
        }

        string ISecuredObject.GetToken()
        {
            return m_token;
        }

        internal Guid m_namespaceId;
        internal int m_requiredPermissions;
        internal string m_token;
    }
}
