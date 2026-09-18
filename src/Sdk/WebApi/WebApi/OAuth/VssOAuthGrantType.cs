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

namespace GitHub.Services.OAuth
{
    /// <summary>
    /// Lists the supported authorization grant types
    /// </summary>
    public enum VssOAuthGrantType
    {
        /// <summary>
        /// Authorization Code Grant for OAuth 2.0
        /// </summary>
        AuthorizationCode,

        /// <summary>
        /// Client Credentials Grant for OAuth 2.0
        /// </summary>
        ClientCredentials,

        /// <summary>
        /// JWT Bearer Token Grant Type Profile for OAuth 2.0
        /// </summary>
        JwtBearer,

        /// <summary>
        /// Refresh Token Grant for OAuth 2.0
        /// </summary>
        RefreshToken,
    }
}
