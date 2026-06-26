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
using GitHub.Actions.RunService.WebApi;
using GitHub.DistributedTask.WebApi;
using GitHub.Runner.Common.Util;
using GitHub.Runner.Worker.Container;
using GitHub.Runner.Worker.Dap;
using Newtonsoft.Json.Linq;
using Sdk.RSWebApi.Contracts;

namespace GitHub.Runner.Worker
{
    public sealed class GlobalContext
    {
        public ContainerInfo Container { get; set; }
        public List<ServiceEndpoint> Endpoints { get; set; }
        public IDictionary<String, String> EnvironmentVariables { get; set; }
        public PlanFeatures Features { get; set; }
        public IList<String> FileTable { get; set; }
        public IDictionary<String, IDictionary<String, String>> JobDefaults { get; set; }
        public List<ActionsStepTelemetry> StepsTelemetry { get; set; }
        public List<StepResult> StepsResult { get; set; }
        public List<Annotation> JobAnnotations { get; set; }
        public List<JobTelemetry> JobTelemetry { get; set; }
        public TaskOrchestrationPlanReference Plan { get; set; }
        public List<string> PrependPath { get; set; }
        public List<ContainerInfo> ServiceContainers { get; set; }
        public StepsContext StepsContext { get; set; }
        public Variables Variables { get; set; }
        public bool WriteDebug { get; set; }
        public DebuggerConfig Debugger { get; set; }
        public string InfrastructureFailureCategory { get; set; }
        public JObject ContainerHookState { get; set; }
        public bool HasTemplateEvaluatorMismatch { get; set; }
        public bool HasActionManifestMismatch { get; set; }
        public bool HasDeprecatedSetOutput { get; set; }
        public bool HasDeprecatedSaveState { get; set; }
        public HashSet<string> DeprecatedNode20Actions { get; set; }
        public HashSet<string> UpgradedToNode24Actions { get; set; }
        public HashSet<string> Arm32Node20Actions { get; set; }
        public IList<String> ActionsDependencies { get; set; }
    }
}
