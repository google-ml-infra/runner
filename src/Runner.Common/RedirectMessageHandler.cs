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
using System.ComponentModel;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using GitHub.Runner.Sdk;
using GitHub.Services.Common;

namespace GitHub.Runner.Common
{
    /// <summary>
    /// Handles redirects for Http requests
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class RedirectMessageHandler : DelegatingHandler
    {
        public RedirectMessageHandler(ITraceWriter trace)
        {
            Trace = trace;
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            HttpResponseMessage response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);

            if (response != null &&
                IsRedirect(response.StatusCode) &&
                response.Headers.Location != null)
            {
                Trace.Info($"Redirecting to '{response.Headers.Location}'.");

                request = await CloneAsync(request, response.Headers.Location).ConfigureAwait(false);

                response.Dispose();

                response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
            }

            return response;
        }

        private static bool IsRedirect(HttpStatusCode statusCode)
        {
            return (int)statusCode >= 300 && (int)statusCode < 400;
        }

        private static async Task<HttpRequestMessage> CloneAsync(HttpRequestMessage request, Uri requestUri)
        {
            var clone = new HttpRequestMessage(request.Method, requestUri)
            {
                Version = request.Version
            };

            request.Headers.ForEach(header => clone.Headers.TryAddWithoutValidation(header.Key, header.Value));

            request.Options.ForEach(option => clone.Options.Set(new HttpRequestOptionsKey<object>(option.Key), option.Value));

            if (request.Content != null)
            {
                clone.Content = new ByteArrayContent(await request.Content.ReadAsByteArrayAsync().ConfigureAwait(false));

                request.Content.Headers.ForEach(header => clone.Content.Headers.TryAddWithoutValidation(header.Key, header.Value));
            }

            return clone;
        }

        private readonly ITraceWriter Trace;
    }
}
