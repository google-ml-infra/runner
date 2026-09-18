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
using Newtonsoft.Json;

namespace GitHub.DistributedTask.Pipelines
{
    [DataContract]
    [KnownType(typeof(ActionStep))]
    [KnownType(typeof(BackgroundStepControl))]
    [JsonConverter(typeof(StepConverter))]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public abstract class Step
    {
        protected Step()
        {
            this.Enabled = true;
        }

        protected Step(Step stepToClone)
        {
            this.Enabled = stepToClone.Enabled;
            this.Id = stepToClone.Id;
            this.Name = stepToClone.Name;
            this.DisplayName = stepToClone.DisplayName;
        }

        [DataMember(EmitDefaultValue = false)]
        public abstract StepType Type
        {
            get;
        }

        [DataMember(EmitDefaultValue = false)]
        public Guid Id
        {
            get;
            set;
        }

        [DataMember(EmitDefaultValue = false)]
        public String Name
        {
            get;
            set;
        }

        [DataMember(EmitDefaultValue = false)]
        public String DisplayName
        {
            get;
            set;
        }

        [DefaultValue(true)]
        [DataMember(EmitDefaultValue = false)]
        public Boolean Enabled
        {
            get;
            set;
        }

        public abstract Step Clone();
    }

    [DataContract]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public enum StepType
    {
        [DataMember]
        Action = 4,
        [DataMember]
        BackgroundStepControl = 5,
    }
}
