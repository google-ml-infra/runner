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
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace GitHub.Services.Common.Internal
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class HttpHeaders
    {
        public const String ActivityId = "ActivityId";
        public const String TfsServiceError = "X-TFS-ServiceError";
        public const String TfsSessionHeader = "X-TFS-Session";
        public const String TfsFedAuthRealm = "X-TFS-FedAuthRealm";
        public const String TfsFedAuthIssuer = "X-TFS-FedAuthIssuer";
        public const String TfsFedAuthRedirect = "X-TFS-FedAuthRedirect";
        public const String VssE2EID = "X-VSS-E2EID";

        public const String VssUserData = "X-VSS-UserData";
        public const String VssAgentHeader = "X-VSS-Agent";
        public const String VssAuthenticateError = "X-VSS-AuthenticateError";

        public const String VssRateLimitDelay = "X-RateLimit-Delay";
        public const String VssRateLimitReset = "X-RateLimit-Reset";

        public const String VssHostOfflineError = "X-VSS-HostOfflineError";

        public const string VssRequestPriority = "X-VSS-RequestPriority";

        public const string Authorization = "Authorization";
        public const string ProxyAuthenticate = "Proxy-Authenticate";
        public const string WwwAuthenticate = "WWW-Authenticate";

        public const string AfdResponseRef = "X-MSEdge-Ref";
    }
}
