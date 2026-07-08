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

namespace GitHub.Services.OAuth
{
    /// <summary>
    /// Provides the properties for the response of a token exchange in OAuth 2.0
    /// </summary>
    [DataContract]
    public class VssOAuthTokenResponse
    {
        /// <summary>
        /// Initializes a new <c>VssOAuthTokenResponse</c> instance with empty values.
        /// </summary>
        public VssOAuthTokenResponse()
        {
        }

        /// <summary>
        /// Gets or sets the access token for the response.
        /// </summary>
        [DataMember(Name = "access_token", EmitDefaultValue = false)]
        public String AccessToken
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the error for the response.
        /// </summary>
        [DataMember(Name = "error", EmitDefaultValue = false)]
        public String Error
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the error description for the response.
        /// </summary>
        [DataMember(Name = "error_description", EmitDefaultValue = false)]
        public String ErrorDescription
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating the remaining duration, in seconds, of the access token.
        /// </summary>
        [DataMember(Name = "expires_in", EmitDefaultValue = false)]
        public Int32 ExpiresIn
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the refresh token for the response, if applicable.
        /// </summary>
        [DataMember(Name = "refresh_token", EmitDefaultValue = false)]
        public String RefreshToken
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the scope or scopes of access for the provided access token.
        /// </summary>
        [DataMember(Name = "scope", EmitDefaultValue = false)]
        public String Scope
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the type of token for the response.
        /// </summary>
        [DataMember(Name = "token_type", EmitDefaultValue = false)]
        public String TokenType
        {
            get;
            set;
        }
    }
}
