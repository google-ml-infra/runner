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
using System.ComponentModel;
using System.Runtime.Serialization;
using GitHub.DistributedTask.ObjectTemplating.Tokens;
using Newtonsoft.Json;

namespace GitHub.DistributedTask.Pipelines
{
    [DataContract]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class ActionStep : JobStep
    {
        [JsonConstructor]
        public ActionStep()
        {
        }

        private ActionStep(ActionStep actionToClone)
            : base(actionToClone)
        {
            this.Reference = actionToClone.Reference?.Clone();

            Environment = actionToClone.Environment?.Clone();
            Inputs = actionToClone.Inputs?.Clone();
            ContextName = actionToClone?.ContextName;
            DisplayNameToken = actionToClone.DisplayNameToken?.Clone();
            Background = actionToClone.Background;
        }

        public override StepType Type => StepType.Action;

        [DataMember]
        public ActionStepDefinitionReference Reference
        {
            get;
            set;
        }

        // TODO: After TFS and legacy phases/steps/ect are removed, lets replace the DisplayName in the base class with this value and remove this additional prop
        [DataMember(EmitDefaultValue = false)]
        public TemplateToken DisplayNameToken { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public String ContextName { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public TemplateToken Environment { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public TemplateToken Inputs { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public bool Background { get; set; }

        public override Step Clone()
        {
            return new ActionStep(this);
        }
    }
}
