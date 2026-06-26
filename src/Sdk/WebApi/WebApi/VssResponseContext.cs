// Copyright 2026 Google LLC
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     https://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using GitHub.Services.Common;
using Newtonsoft.Json;

namespace GitHub.Services.WebApi
{
    public class VssResponseContext
    {
        internal VssResponseContext(HttpStatusCode statusCode, HttpResponseHeaders headers)
        {
            if (headers.Contains(Common.Internal.HttpHeaders.ActivityId))
            {
                IEnumerable<string> values = headers.GetValues(Common.Internal.HttpHeaders.ActivityId);
                string activityId = values.FirstOrDefault();
                Guid result;
                Guid.TryParse(activityId, out result);
                ActivityId = result;
            }

            IEnumerable<String> headerValues;
            if (headers.TryGetValues(PerformanceTimerConstants.Header, out headerValues))
            {
                Timings = JsonConvert.DeserializeObject<IDictionary<String, PerformanceTimingGroup>>(headerValues.First());
            }

            HttpStatusCode = statusCode;
            Headers = headers;
        }

        public bool TryGetException(out Exception value)
        {
            value = Exception;
            return Exception != null;
        }

        public bool TryGetErrorCode(out string value)
        {
            value = null;
            if (Exception == null)
            {
                return false;
            }
            var message = Exception.Message;
            var match = Regex.Match(message, @"(TF[0-9]+)");
            if (match.Success)
            {
                value = match.Value;
                return true;
            }
            match = Regex.Match(message, @"(VSS[0-9]+)");
            if (match.Success)
            {
                value = match.Value;
                return true;
            }
            return false;
        }

        public HttpStatusCode HttpStatusCode { get; private set; }

        public Guid ActivityId { get; private set; }

        public Exception Exception { get; internal set; }

        public IDictionary<String, PerformanceTimingGroup> Timings { get; private set; }

        public HttpResponseHeaders Headers { get; private set; }
    }
}
