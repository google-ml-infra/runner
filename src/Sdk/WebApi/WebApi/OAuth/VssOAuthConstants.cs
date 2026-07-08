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

namespace GitHub.Services.OAuth
{
    /// <summary>
    /// Provides constants used in token exhanges for OAuth 2.0
    /// </summary>
    public static class VssOAuthConstants
    {
        /// <summary>
        /// Assertion parameter for token requests.
        /// </summary>
        public const String Assertion = "assertion";

        /// <summary>
        /// Authorization Code Grant for OAuth 2.0
        /// </summary>
        public const String AuthorizationCodeGrantType = "authorization_code";

        /// <summary>
        /// Client Credentials Grant for OAuth 2.0
        /// </summary>
        public const String ClientCredentialsGrantType = "client_credentials";

        /// <summary>
        /// Client ID parameter for client authentication.
        /// </summary>
        public const String ClientId = "client_id";

        /// <summary>
        /// Client secret parameter for client authentication.
        /// </summary>
        // [SuppressMessage("Microsoft.Security", "CS002:SecretInNextLine")] -- The "Password" that follows is not valid anywhere
        public const String ClientSecret = "client_secret";

        /// <summary>
        /// Client assertion parameter for client authentication.
        /// </summary>
        public const String ClientAssertion = "client_assertion";

        /// <summary>
        /// Client assertion type parameter for client authentication.
        /// </summary>
        public const String ClientAssertionType = "client_assertion_type";

        /// <summary>
        /// Code parameter for authorization code token requests.
        /// </summary>
        public const String Code = "code";

        /// <summary>
        /// Grant type parameter for token requests.
        /// </summary>
        public const String GrantType = "grant_type";

        /// <summary>
        /// JWT Bearer Token Grant Type Profile for OAuth 2.0
        /// </summary>
        /// <remarks>
        /// See http://tools.ietf.org/html/rfc7523
        /// </remarks>
        public const String JwtBearerAuthorizationGrantType = "urn:ietf:params:oauth:grant-type:jwt-bearer";

        /// <summary>
        /// JWT Bearer Token Profile for OAuth 2.0 Client Authentication
        /// </summary>
        /// <remarks>
        /// See http://tools.ietf.org/html/rfc7523
        /// </remarks>
        public const String JwtBearerClientAssertionType = "urn:ietf:params:oauth:client-assertion-type:jwt-bearer";

        /// <summary>
        /// Refresh token parameter for token requests.
        /// </summary>
        public const String RefreshToken = "refresh_token";

        /// <summary>
        /// Refresh Token Grant for OAuth 2.0
        /// </summary>
        public const String RefreshTokenGrantType = "refresh_token";
    }
}
