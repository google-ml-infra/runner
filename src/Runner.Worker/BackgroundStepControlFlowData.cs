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

namespace GitHub.Runner.Worker
{
    /// <summary>
    /// Pure data for control-flow steps (wait, wait-all, cancel).
    /// Type uses Pipelines.BackgroundControlTypes string constants.
    /// </summary>
    public sealed class BackgroundStepControlFlowData
    {
        public string Type { get; set; }
        public Guid StepId { get; set; }
        public string StepName { get; set; }

        // Target step IDs (for wait: steps to wait for; for cancel: steps to cancel)
        public string[] StepIds { get; set; }

        // Parallel group ID for grouping steps in the UI
        public string ParallelGroupId { get; set; }
    }
}
