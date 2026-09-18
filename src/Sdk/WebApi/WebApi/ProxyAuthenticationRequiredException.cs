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
using GitHub.Services.Common;

namespace GitHub.Services.WebApi
{
    [ExceptionMapping("0.0", "3.0", "ProxyAuthenticationRequiredException", "GitHub.Services.WebApi.ProxyAuthenticationRequiredException, GitHub.Services.WebApi, Version=14.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
    public class ProxyAuthenticationRequiredException : VssException
    {
        public ProxyAuthenticationRequiredException()
            : base(WebApiResources.ProxyAuthenticationRequired())
        {
            this.HelpLink = HelpLinkUrl;
        }

        public ProxyAuthenticationRequiredException(string message, Exception innerException)
            : base(message, innerException)
        {
            this.HelpLink = HelpLinkUrl;
        }

        public ProxyAuthenticationRequiredException(string message)
            : base(message)
        {
            this.HelpLink = HelpLinkUrl;
        }

        private const string HelpLinkUrl = "";
    }
}
