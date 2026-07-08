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
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace GitHub.Services.Common
{
    internal struct HttpRequestMessageWrapper : IHttpRequest, IHttpHeaders
    {
        public HttpRequestMessageWrapper(HttpRequestMessage request)
        {
            m_request = request;
        }

        public IHttpHeaders Headers
        {
            get
            {
                return this;
            }
        }

        public Uri RequestUri
        {
            get
            {
                return m_request.RequestUri;
            }
        }

        IEnumerable<String> IHttpHeaders.GetValues(String name)
        {
            IEnumerable<String> values;
            if (!m_request.Headers.TryGetValues(name, out values))
            {
                values = Enumerable.Empty<String>();
            }
            return values;
        }

        void IHttpHeaders.SetValue(
            String name,
            String value)
        {
            m_request.Headers.Remove(name);
            m_request.Headers.Add(name, value);
        }

        Boolean IHttpHeaders.TryGetValues(
            String name,
            out IEnumerable<String> values)
        {
            return m_request.Headers.TryGetValues(name, out values);
        }

        private readonly HttpRequestMessage m_request;
    }
}
