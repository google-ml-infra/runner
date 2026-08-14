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
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace GitHub.DistributedTask.WebApi
{
    /// <summary>
    /// Information about a step run on the runner
    /// </summary>
    [DataContract]
    public class ActionsStepTelemetry
    {
        public ActionsStepTelemetry()
        {
            this.ErrorMessages = new List<string>();
        }

        [DataMember(EmitDefaultValue = false)]
        public string Action { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Ref { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Type { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Stage { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public Guid StepId { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string StepContextName { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public bool? HasRunsStep { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public bool? HasUsesStep { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public bool IsEmbedded { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public bool? HasPreStep { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public bool? HasPostStep { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int? StepCount { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public TaskResult? Result { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<string> ErrorMessages { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int? ExecutionTimeInSeconds { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public DateTime? StartTime { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public DateTime? FinishTime { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string ContainerHookData { get; set; }
    }
}
