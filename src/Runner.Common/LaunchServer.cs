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
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using GitHub.DistributedTask.WebApi;
using GitHub.Runner.Sdk;
using GitHub.Services.Common;
using GitHub.Services.Launch.Client;

namespace GitHub.Runner.Common
{
    [ServiceLocator(Default = typeof(LaunchServer))]
    public interface ILaunchServer : IRunnerService
    {
        void InitializeLaunchClient(Uri uri, string token);

        Task<ActionDownloadInfoCollection> ResolveActionsDownloadInfoAsync(Guid planId, Guid jobId, ActionReferenceList actionReferenceList, CancellationToken cancellationToken, bool displayHelpfulActionsDownloadErrors);
    }

    public sealed class LaunchServer : RunnerService, ILaunchServer
    {
        private LaunchHttpClient _launchClient;

        public void InitializeLaunchClient(Uri uri, string token)
        {
            // Using default 100 timeout
            RawClientHttpRequestSettings settings = VssUtil.GetHttpRequestSettings(null);

            // Create retry handler
            IEnumerable<DelegatingHandler> delegatingHandlers = new List<DelegatingHandler>();
            if (settings.MaxRetryRequest > 0)
            {
                delegatingHandlers = new DelegatingHandler[] { new VssHttpRetryMessageHandler(settings.MaxRetryRequest) };
            }

            // Setup RawHttpMessageHandler without credentials
            var httpMessageHandler = new RawHttpMessageHandler(new NoOpCredentials(null), settings);
            var pipeline = HttpClientFactory.CreatePipeline(httpMessageHandler, delegatingHandlers);

            this._launchClient = new LaunchHttpClient(uri, pipeline, token, disposeHandler: true);
        }

        public Task<ActionDownloadInfoCollection> ResolveActionsDownloadInfoAsync(Guid planId, Guid jobId, ActionReferenceList actionReferenceList,
            CancellationToken cancellationToken, bool displayHelpfulActionsDownloadErrors)
        {
            if (_launchClient != null)
            {
                if (!displayHelpfulActionsDownloadErrors)
                {
                    return _launchClient.GetResolveActionsDownloadInfoAsync(planId, jobId, actionReferenceList,
                        cancellationToken: cancellationToken);
                }
                return _launchClient.GetResolveActionsDownloadInfoAsyncV2(planId, jobId, actionReferenceList, cancellationToken);
            }

            throw new InvalidOperationException("Launch client is not initialized.");
        }
    }
}
