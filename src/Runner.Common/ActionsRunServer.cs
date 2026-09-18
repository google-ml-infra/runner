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
using System.Threading;
using System.Threading.Tasks;
using GitHub.DistributedTask.Pipelines;
using GitHub.DistributedTask.WebApi;
using GitHub.Services.Common;
using GitHub.Services.WebApi;

namespace GitHub.Runner.Common
{
    [ServiceLocator(Default = typeof(ActionsRunServer))]
    public interface IActionsRunServer : IRunnerService
    {
        Task ConnectAsync(Uri serverUrl, VssCredentials credentials);

        Task<AgentJobRequestMessage> GetJobMessageAsync(string id, CancellationToken token);
    }

    public sealed class ActionsRunServer : RunnerService, IActionsRunServer
    {
        private bool _hasConnection;
        private VssConnection _connection;
        private ActionsRunServerHttpClient _actionsRunServerClient;

        public async Task ConnectAsync(Uri serverUrl, VssCredentials credentials)
        {
            _connection = await EstablishVssConnection(serverUrl, credentials, TimeSpan.FromSeconds(100));
            _actionsRunServerClient = _connection.GetClient<ActionsRunServerHttpClient>();
            _hasConnection = true;
        }

        private void CheckConnection()
        {
            if (!_hasConnection)
            {
                throw new InvalidOperationException($"SetConnection");
            }
        }

        public Task<AgentJobRequestMessage> GetJobMessageAsync(string id, CancellationToken cancellationToken)
        {
            CheckConnection();
            var jobMessage = RetryRequest<AgentJobRequestMessage>(async () =>
                                                    {
                                                        return await _actionsRunServerClient.GetJobMessageAsync(id, cancellationToken);
                                                    }, cancellationToken);

            return jobMessage;
        }
    }
}
