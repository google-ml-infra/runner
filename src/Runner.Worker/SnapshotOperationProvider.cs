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

﻿#nullable enable
using System;
using System.IO;
using System.Threading.Tasks;
using GitHub.DistributedTask.Pipelines;
using GitHub.DistributedTask.WebApi;
using GitHub.Runner.Common;
using GitHub.Runner.Sdk;
using GitHub.Runner.Worker.Handlers;
namespace GitHub.Runner.Worker;

[ServiceLocator(Default = typeof(SnapshotOperationProvider))]
public interface ISnapshotOperationProvider : IRunnerService
{
    Task CreateSnapshotRequestAsync(IExecutionContext executionContext, Snapshot snapshotRequest);
    void RunSnapshotPreflightChecks(IExecutionContext jobContext);
}

public class SnapshotOperationProvider : RunnerService, ISnapshotOperationProvider
{
    public Task CreateSnapshotRequestAsync(IExecutionContext executionContext, Snapshot snapshotRequest)
    {
        var snapshotRequestFilePath = Path.Combine(HostContext.GetDirectory(WellKnownDirectory.Root), ".snapshot", "request.json");
        var snapshotRequestDirectoryPath = Path.GetDirectoryName(snapshotRequestFilePath);
        if (snapshotRequestDirectoryPath != null)
        {
            Directory.CreateDirectory(snapshotRequestDirectoryPath);
        }

        IOUtil.SaveObject(snapshotRequest, snapshotRequestFilePath);
        executionContext.Output($"Image Name: {snapshotRequest.ImageName} Version: {snapshotRequest.Version}");
        executionContext.Output($"Request written to: {snapshotRequestFilePath}");
        executionContext.Output("This request will be processed after the job completes. You will not receive any feedback on the snapshot process within the workflow logs of this job.");
        executionContext.Output("If the snapshot process is successful, you should see a new image with the requested name in the list of available custom images when creating a new GitHub-hosted Runner.");
        return Task.CompletedTask;
    }

    public void RunSnapshotPreflightChecks(IExecutionContext context)
    {
        var shouldCheckRunnerEnvironment = context.Global.Variables.GetBoolean(Constants.Runner.Features.SnapshotPreflightHostedRunnerCheck) ?? false;
        if (shouldCheckRunnerEnvironment &&
             context.Global.Variables.TryGetValue(WellKnownDistributedTaskVariables.RunnerEnvironment, out var runnerEnvironment) &&
             !string.IsNullOrEmpty(runnerEnvironment))
        {
            context.Debug($"Snapshot: RUNNER_ENVIRONMENT={runnerEnvironment}");
            if (!string.Equals(runnerEnvironment, "github-hosted", StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException("Snapshot workflows must be run on a GitHub Hosted Runner");
            }
        }
        var imageGenEnabled = StringUtil.ConvertToBoolean(Environment.GetEnvironmentVariable("GITHUB_ACTIONS_IMAGE_GEN_ENABLED"));
        context.Debug($"Snapshot: GITHUB_ACTIONS_IMAGE_GEN_ENABLED={imageGenEnabled}");
        var shouldCheckImageGenPool = context.Global.Variables.GetBoolean(Constants.Runner.Features.SnapshotPreflightImageGenPoolCheck) ?? false;
        if (shouldCheckImageGenPool && !imageGenEnabled)
        {
            throw new ArgumentException("Snapshot workflows must be run a hosted runner with Image Generation enabled");
        }
    }
}
