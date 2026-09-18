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

using System.Collections.Generic;
using System.Threading.Tasks;

namespace GitHub.Services.Common
{
    /// <summary>
    /// Provide an interface to get a new token for the credentials.
    /// </summary>
    public interface IVssCredentialPrompt
    {
        /// <summary>
        /// Get a new token using the specified provider and the previously failed token.
        /// </summary>
        /// <param name="provider">The provider for the token to be retrieved</param>
        /// <param name="failedToken">The token which previously failed authentication, if available</param>
        /// <returns>The new token</returns>
        Task<IssuedToken> GetTokenAsync(IssuedTokenProvider provider, IssuedToken failedToken);

        IDictionary<string, string> Parameters { get; set; }
    }

    public interface IVssCredentialPrompts : IVssCredentialPrompt
    {
        IVssCredentialPrompt FederatedPrompt
        {
            get;
        }
    }
}
