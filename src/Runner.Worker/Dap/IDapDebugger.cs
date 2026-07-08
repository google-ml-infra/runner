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

﻿using System.Collections.Generic;
using System.Threading.Tasks;
using GitHub.Runner.Common;

namespace GitHub.Runner.Worker.Dap
{
    public enum DapSessionState
    {
        NotStarted,
        WaitingForConnection,
        Initializing,
        Ready,
        Paused,
        Running,
        Terminated
    }

    [ServiceLocator(Default = typeof(DapDebugger))]
    public interface IDapDebugger : IRunnerService
    {
        Task StartAsync(IExecutionContext jobContext);
        Task WaitUntilReadyAsync();
        Task OnJobStepsInitializedAsync(IEnumerable<IStep> steps, IEnumerable<IStep> initialPostSteps);
        void OnPostStepRegistered(IStep step);
        Task OnStepStartingAsync(IStep step);
        void OnStepCompleted(IStep step);
        Task OnJobCompletedAsync();
        Task StopAsync();
    }
}
