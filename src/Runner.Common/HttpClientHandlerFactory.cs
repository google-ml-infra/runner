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
using System.Net.Http;
using GitHub.Runner.Sdk;

namespace GitHub.Runner.Common
{
    [ServiceLocator(Default = typeof(HttpClientHandlerFactory))]
    public interface IHttpClientHandlerFactory : IRunnerService
    {
        HttpClientHandler CreateClientHandler(RunnerWebProxy webProxy);
    }

    public class HttpClientHandlerFactory : RunnerService, IHttpClientHandlerFactory
    {
        public HttpClientHandler CreateClientHandler(RunnerWebProxy webProxy)
        {
            var client = new HttpClientHandler() { Proxy = webProxy };

            if (StringUtil.ConvertToBoolean(Environment.GetEnvironmentVariable("GITHUB_ACTIONS_RUNNER_TLS_NO_VERIFY")))
            {
                client.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            }

            return client;
        }
    }
}
