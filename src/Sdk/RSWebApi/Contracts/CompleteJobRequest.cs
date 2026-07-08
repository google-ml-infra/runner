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
using System.Runtime.Serialization;
using GitHub.DistributedTask.WebApi;
using Sdk.RSWebApi.Contracts;

namespace GitHub.Actions.RunService.WebApi
{
    [DataContract]
    public class CompleteJobRequest
    {
        [DataMember(Name = "planId", EmitDefaultValue = false)]
        public Guid PlanID { get; set; }

        [DataMember(Name = "jobId", EmitDefaultValue = false)]
        public Guid JobID { get; set; }

        [DataMember(Name = "conclusion")]
        public TaskResult Conclusion { get; set; }

        [DataMember(Name = "outputs", EmitDefaultValue = false)]
        public Dictionary<string, VariableValue> Outputs { get; set; }

        [DataMember(Name = "stepResults", EmitDefaultValue = false)]
        public IList<StepResult> StepResults { get; set; }

        [DataMember(Name = "annotations", EmitDefaultValue = false)]
        public IList<Annotation> Annotations { get; set; }

        [DataMember(Name = "telemetry", EmitDefaultValue = false)]
        public IList<Telemetry> Telemetry { get; set; }

        [DataMember(Name = "environmentUrl", EmitDefaultValue = false)]
        public string EnvironmentUrl { get; set; }

        [DataMember(Name = "billingOwnerId", EmitDefaultValue = false)]
        public string BillingOwnerId { get; set; }

        [DataMember(Name = "infrastructureFailureCategory", EmitDefaultValue = false)]
        public string InfrastructureFailureCategory { get; set; }
    }
}
