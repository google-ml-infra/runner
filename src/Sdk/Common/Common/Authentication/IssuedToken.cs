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
using System.Linq;
using System.Threading;
using GitHub.Services.Common.Internal;

namespace GitHub.Services.Common
{
    /// <summary>
    /// Provides a common base class for issued tokens.
    /// </summary>
    [Serializable]
    public abstract class IssuedToken
    {
        internal IssuedToken()
        {
        }

        /// <summary>
        /// Gets a value indicating whether or not this token has been successfully authenticated with the remote
        /// server.
        /// </summary>
        public bool IsAuthenticated
        {
            get
            {
                return m_authenticated == 1;
            }
        }

        protected internal abstract VssCredentialsType CredentialType
        {
            get;
        }

        /// <summary>
        /// True if the token is retrieved from token storage.
        /// </summary>
        internal bool FromStorage
        {
            get;
            set;
        }

        /// <summary>
        /// Metadata about the token in a collection of properties.
        /// </summary>
        /// <returns></returns>
        public IDictionary<string, string> Properties
        {
            get;
            set;
        }

        /// <summary>
        /// Id of the owner of the token.
        /// </summary>
        internal Guid UserId
        {
            get;
            set;
        }

        /// <summary>
        /// Name of the owner of the token.
        /// </summary>
        internal string UserName
        {
            get;
            set;
        }

        /// <summary>
        /// Invoked when the issued token has been validated by successfully authenticated with the remote server.
        /// </summary>
        internal bool Authenticated()
        {
            return Interlocked.CompareExchange(ref m_authenticated, 1, 0) == 0;
        }

        /// <summary>
        /// Get the value of the <c>HttpHeaders.VssUserData</c> response header and
        /// populate the <c>UserId</c> and <c>UserName</c> properties. 
        /// </summary>
        internal void GetUserData(IHttpResponse response)
        {
            IEnumerable<string> headerValues;
            if (response.Headers.TryGetValues(HttpHeaders.VssUserData, out headerValues))
            {
                string userData = headerValues.FirstOrDefault();

                if (!string.IsNullOrWhiteSpace(userData))
                {
                    string[] split = userData.Split(':');

                    if (split.Length >= 2)
                    {
                        UserId = Guid.Parse(split[0]);
                        UserName = split[1];
                    }
                }
            }
        }

        /// <summary>
        /// Applies the token to the HTTP request message.
        /// </summary>
        /// <param name="request">The HTTP request message</param>
        internal abstract void ApplyTo(IHttpRequest request);

        private int m_authenticated;
    }
}
