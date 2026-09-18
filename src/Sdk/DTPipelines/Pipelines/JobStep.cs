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
using System.ComponentModel;
using System.Runtime.Serialization;
using GitHub.DistributedTask.ObjectTemplating.Tokens;
using Newtonsoft.Json;

namespace GitHub.DistributedTask.Pipelines
{
    [DataContract]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public abstract class JobStep : Step
    {
        [JsonConstructor]
        public JobStep()
        {
            this.Enabled = true;
        }

        protected JobStep(JobStep stepToClone)
            : base(stepToClone)
        {
            this.Condition = stepToClone.Condition;
            this.ContinueOnError = stepToClone.ContinueOnError?.Clone();
            this.TimeoutInMinutes = stepToClone.TimeoutInMinutes?.Clone();
            this.ParallelGroupId = stepToClone.ParallelGroupId;
        }

        [DataMember(EmitDefaultValue = false)]
        public String Condition
        {
            get;
            set;
        }

        [DataMember(EmitDefaultValue = false)]
        public TemplateToken ContinueOnError
        {
            get;
            set;
        }

        [DataMember(EmitDefaultValue = false)]
        public TemplateToken TimeoutInMinutes
        {
            get;
            set;
        }

        [DataMember(EmitDefaultValue = false)]
        public string ParallelGroupId { get; set; }
    }
}
