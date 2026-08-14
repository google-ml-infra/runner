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
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Runtime.Serialization;
using System.Security;
using GitHub.Services.Common;

namespace GitHub.Services.WebApi
{
    [Serializable]
    [SuppressMessage("Microsoft.Design", "CA1032:ImplementStandardExceptionConstructors")]
    [ExceptionMapping("0.0", "3.0", "VssServiceResponseException", "GitHub.Services.WebApi.VssServiceResponseException, GitHub.Services.WebApi, Version=14.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
    public class VssServiceResponseException : VssServiceException
    {
        public VssServiceResponseException(HttpStatusCode code, String message, Exception innerException)
            : base(message, innerException)
        {
            this.HttpStatusCode = code;
        }

        protected VssServiceResponseException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            HttpStatusCode = (HttpStatusCode)info.GetInt32("HttpStatusCode");
        }

        [Obsolete]
        [SecurityCritical]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("HttpStatusCode", (int)HttpStatusCode);
        }

        public HttpStatusCode HttpStatusCode { get; private set; }
    }
}
