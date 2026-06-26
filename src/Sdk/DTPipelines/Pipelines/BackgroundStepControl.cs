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

using System.ComponentModel;
using System.Runtime.Serialization;
using GitHub.DistributedTask.ObjectTemplating.Tokens;
using Newtonsoft.Json;

namespace GitHub.DistributedTask.Pipelines
{
    /// <summary>
    /// Known control-flow types for background step control steps.
    /// Wire values must match run-service constants (wait, wait-all, cancel).
    /// </summary>
    public static class BackgroundControlTypes
    {
        public const string Wait = "wait";
        public const string WaitAll = "wait-all";
        public const string Cancel = "cancel";
    }

    /// <summary>
    /// Represents a unified background step control-flow step (wait, wait-all, cancel).
    /// </summary>
    [DataContract]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class BackgroundStepControl : JobStep
    {
        [JsonConstructor]
        public BackgroundStepControl()
        {
        }

        private BackgroundStepControl(BackgroundStepControl stepToClone)
            : base(stepToClone)
        {
            this.ControlType = stepToClone.ControlType;
            this.StepIds = stepToClone.StepIds != null
                ? (string[])stepToClone.StepIds.Clone()
                : null;
            this.DisplayNameToken = stepToClone.DisplayNameToken?.Clone();
        }

        public override StepType Type => StepType.BackgroundStepControl;

        [DataMember(EmitDefaultValue = false)]
        public string ControlType { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string[] StepIds { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public TemplateToken DisplayNameToken { get; set; }

        public override Step Clone()
        {
            return new BackgroundStepControl(this);
        }
    }
}
