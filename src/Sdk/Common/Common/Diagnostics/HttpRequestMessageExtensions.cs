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
using System.ComponentModel;
using System.Net.Http;

namespace GitHub.Services.Common.Diagnostics
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal static class HttpRequestMessageExtensions
    {
        public static VssHttpMethod GetHttpMethod(this HttpRequestMessage message)
        {
            String methodName = message.Method.Method;
            VssHttpMethod httpMethod = VssHttpMethod.UNKNOWN;
            if (!Enum.TryParse<VssHttpMethod>(methodName, true, out httpMethod))
            {
                httpMethod = VssHttpMethod.UNKNOWN;
            }
            return httpMethod;
        }

        public static VssTraceActivity GetActivity(this HttpRequestMessage message)
        {
            Object traceActivity;
            if (!message.Options.TryGetValue(VssTraceActivity.PropertyName, out traceActivity))
            {
                return VssTraceActivity.Empty;
            }
            return (VssTraceActivity)traceActivity;
        }
    }
}
