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

﻿using System.Security.Claims;

namespace GitHub.Services.WebApi.Jwt
{
    public static class JsonWebTokenClaims
    {
        public const string ActorToken = "actort";
        public const string Audience = "aud";
        public const string IssuedAt = "iat";
        public const string Issuer = "iss";
        public const string NameId = "nameid";
        public const string IdentityProvider = "identityprovider";
        public const string ValidTo = "exp";
        public const string ValidFrom = "nbf";
        public const string Scopes = "scp";
        public const string RefreshToken = "ret";
        public const string Source = "src";
        public const string Subject = "sub";
        public const string TrustedForDelegation = "trustedfordelegation";
        public const string NameIdLongName = ClaimTypes.NameIdentifier;
        public const string IdentityProviderLongName = "http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider";
        public const string TenantId = "tid";
        public const string TenantIdLongName = "http://schemas.microsoft.com/identity/claims/tenantid";
        public const string TokenId = "jti";
        public const string AppId = "appid";
    }

    internal static class JsonWebTokenHeaderParameters
    {
        internal const string Algorithm = "alg";
        internal const string Type = "typ";
        internal const string X509CertificateThumbprint = "x5t";
        internal const string JWTType = "JWT";
        internal const string JWTURNType = "urn:ietf:params:oauth:token-type:jwt";
    }
}
