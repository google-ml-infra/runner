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
using System.Threading.Tasks;
using GitHub.DistributedTask.WebApi;
using Sdk.RSWebApi.Contracts;

namespace GitHub.Actions.RunService.WebApi
{
    [DataContract]
    public class StepResult
    {
        [DataMember(Name = "external_id", EmitDefaultValue = false)]
        public Guid ExternalID { get; set; }

        [DataMember(Name = "number", EmitDefaultValue = false)]
        public int? Number { get; set; }

        // Example: "Run actions/checkout@v3"
        [DataMember(Name = "name", EmitDefaultValue = false)]
        public string Name { get; set; }

        // Example: "actions/checkout"
        [DataMember(Name = "action_name", EmitDefaultValue = false)]
        public string ActionName { get; set; }

        [DataMember(Name = "ref", EmitDefaultValue = false)]
        public string Ref { get; set; }

        [DataMember(Name = "type", EmitDefaultValue = false)]
        public string Type { get; set; }

        [DataMember(Name = "status")]
        public TimelineRecordState? Status { get; set; }

        [DataMember(Name = "conclusion")]
        public TaskResult? Conclusion { get; set; }

        [DataMember(Name = "started_at", EmitDefaultValue = false)]
        public DateTime? StartedAt { get; set; }

        [DataMember(Name = "completed_at", EmitDefaultValue = false)]
        public DateTime? CompletedAt { get; set; }

        [DataMember(Name = "completed_log_url", EmitDefaultValue = false)]
        public string CompletedLogURL { get; set; }

        [DataMember(Name = "completed_log_lines", EmitDefaultValue = false)]
        public long? CompletedLogLines { get; set; }

        [DataMember(Name = "annotations", EmitDefaultValue = false)]
        public List<Annotation> Annotations { get; set; }

        [DataMember(Name = "is_background", EmitDefaultValue = false)]
        public bool IsBackground { get; set; }

        [DataMember(Name = "background_control_type", EmitDefaultValue = false)]
        public string BackgroundControlType { get; set; }

        [DataMember(Name = "background_control_step_ids", EmitDefaultValue = false)]
        public string[] BackgroundControlStepIds { get; set; }
    }
}
