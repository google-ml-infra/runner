// Copyright 2026 Google LLC
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     https://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

﻿using System;
using System.Collections.Generic;
using GitHub.Services.Common;

namespace GitHub.Services.OAuth
{
    /// <summary>
    /// Provides client credentials for proof of identity in OAuth 2.0 token exchanges.
    /// </summary>
    public abstract class VssOAuthClientCredential : IVssOAuthTokenParameterProvider, IDisposable
    {
        protected VssOAuthClientCredential(
            VssOAuthClientCredentialType type,
            String clientId)
        {
            ArgumentUtility.CheckStringForNullOrEmpty(clientId, nameof(clientId));

            m_type = type;
            m_clientId = clientId;
        }

        /// <summary>
        /// Gets the client identifier.
        /// </summary>
        public String ClientId
        {
            get
            {
                return m_clientId;
            }
        }

        /// <summary>
        /// Gets the type of credentials for this instance.
        /// </summary>
        public VssOAuthClientCredentialType CredentialType
        {
            get
            {
                return m_type;
            }
        }

        /// <summary>
        /// Disposes of managed resources referenced by the credentials.
        /// </summary>
        public void Dispose()
        {
            if (m_disposed)
            {
                return;
            }

            m_disposed = true;
            Dispose(true);
        }

        protected virtual void Dispose(Boolean disposing)
        {
        }

        /// <summary>
        /// When overridden in a derived class, the corresponding token request parameters should be set for the 
        /// credential type represented by the instance.
        /// </summary>
        /// <param name="parameters">The parameters to post to an authorization server</param>
        protected abstract void SetParameters(IDictionary<String, String> parameters);

        void IVssOAuthTokenParameterProvider.SetParameters(IDictionary<String, String> parameters)
        {
            SetParameters(parameters);
        }

        private Boolean m_disposed;
        private readonly String m_clientId;
        private readonly VssOAuthClientCredentialType m_type;
    }
}
