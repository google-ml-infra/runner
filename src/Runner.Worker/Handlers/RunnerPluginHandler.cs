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
using System.Threading.Tasks;
using GitHub.Runner.Common;
using GitHub.Runner.Sdk;
using Pipelines = GitHub.DistributedTask.Pipelines;

namespace GitHub.Runner.Worker.Handlers
{
    [ServiceLocator(Default = typeof(RunnerPluginHandler))]
    public interface IRunnerPluginHandler : IHandler
    {
        PluginActionExecutionData Data { get; set; }
    }

    public sealed class RunnerPluginHandler : Handler, IRunnerPluginHandler
    {
        public PluginActionExecutionData Data { get; set; }

        public async Task RunAsync(ActionRunStage stage)
        {
            // Validate args.
            Trace.Entering();
            ArgUtil.NotNull(Data, nameof(Data));
            ArgUtil.NotNull(ExecutionContext, nameof(ExecutionContext));
            ArgUtil.NotNull(Inputs, nameof(Inputs));

            string plugin = null;
            if (stage == ActionRunStage.Main)
            {
                plugin = Data.Plugin;
            }
            else if (stage == ActionRunStage.Post)
            {
                plugin = Data.Post;
            }

            ArgUtil.NotNullOrEmpty(plugin, nameof(plugin));
            // Set extra telemetry base on the current context.
            ExecutionContext.StepTelemetry.Type = plugin;

            // Update the env dictionary.
            AddPrependPathToEnvironment();

            // Make sure only particular task get run as runner plugin.
            var runnerPlugin = HostContext.GetService<IRunnerPluginManager>();
            using (var outputManager = new OutputManager(ExecutionContext, ActionCommandManager))
            {
                ActionCommandManager.EnablePluginInternalCommand();
                try
                {
                    await runnerPlugin.RunPluginActionAsync(ExecutionContext, plugin, Inputs, Environment, RuntimeVariables, outputManager.OnDataReceived);
                }
                finally
                {
                    ActionCommandManager.DisablePluginInternalCommand();
                }
            }
        }
    }
}
