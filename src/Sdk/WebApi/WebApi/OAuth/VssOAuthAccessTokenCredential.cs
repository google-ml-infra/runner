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
using System.Net.Http;
using GitHub.Services.Common;
using GitHub.Services.WebApi.Jwt;

namespace GitHub.Services.OAuth
{
    /// <summary>
    /// Provides authentication for OAuth 2.0 access tokens issued without credentials.
    /// </summary>
    public class VssOAuthAccessTokenCredential : FederatedCredential
    {
        /// <summary>
        /// Initializes a new <c>VssOAuthAccessTokenCredential</c> instance with the specified access token encoded as
        /// a string.
        /// </summary>
        /// <param name="accessToken">The access token value encoded as a string</param>
        public VssOAuthAccessTokenCredential(String accessToken)
            : this(new VssOAuthAccessToken(accessToken))
        {
        }

        /// <summary>
        /// Initializes a new <c>VssOAuthAccessTokenCredential</c> instance with the specified access token encoded as
        /// a JWT.
        /// </summary>
        /// <param name="accessToken">The access token value encoded as a JWT</param>
        public VssOAuthAccessTokenCredential(JsonWebToken accessToken)
            : this(new VssOAuthAccessToken(accessToken))
        {
        }

        /// <summary>
        /// Initializes a new <c>VssOAuthAccessTokenCredential</c> instance with the specified access token.
        /// </summary>
        /// <param name="accessToken">The access token</param>
        public VssOAuthAccessTokenCredential(VssOAuthAccessToken accessToken)
            : base(accessToken)
        {
        }

        /// <summary>
        /// Gets the type of the current credentials.
        /// </summary>
        public override VssCredentialsType CredentialType
        {
            get
            {
                return VssCredentialsType.OAuth;
            }
        }

        /// <summary>
        /// Returns a no-op token provider. This credential does not provide token acquisition functionality.
        /// </summary>
        /// <param name="serverUrl">The server URL from which the challenge originated</param>
        /// <param name="response">The authentication challenge response message</param>
        /// <returns>A no-op token provider for supplying the access token</returns>
        protected override IssuedTokenProvider OnCreateTokenProvider(
            Uri serverUrl,
            IHttpResponse response)
        {
            return new VssOAuthAccessTokenProvider(this, serverUrl, null);
        }

        private class VssOAuthAccessTokenProvider : IssuedTokenProvider
        {
            public VssOAuthAccessTokenProvider(
                IssuedTokenCredential credential,
                Uri serverUrl,
                Uri signInUrl)
                : base(credential, serverUrl, signInUrl)
            {
            }

            public override Boolean GetTokenIsInteractive
            {
                get
                {
                    return false;
                }
            }
        }
    }
}
