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
using System.Linq;
using System.Net;
using GitHub.Services.Common.Internal;

namespace GitHub.Services.Common
{
    /// <summary>
    /// Provides a common implementation for federated credentials.
    /// </summary>
    [Serializable]
    public abstract class FederatedCredential : IssuedTokenCredential
    {
        protected FederatedCredential(IssuedToken initialToken)
            : base(initialToken)
        {
        }

        public override bool IsAuthenticationChallenge(IHttpResponse webResponse)
        {
            if (webResponse == null)
            {
                return false;
            }

            if (webResponse.StatusCode == HttpStatusCode.Found ||
                webResponse.StatusCode == HttpStatusCode.Redirect)
            {
                return webResponse.Headers.GetValues(HttpHeaders.TfsFedAuthRealm).Any();
            }

            return webResponse.StatusCode == HttpStatusCode.Unauthorized;
        }
    }
}
