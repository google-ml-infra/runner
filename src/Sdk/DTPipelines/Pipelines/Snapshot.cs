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
using System.Runtime.Serialization;
using GitHub.DistributedTask.ObjectTemplating.Tokens;
using GitHub.DistributedTask.Pipelines.ObjectTemplating;

namespace GitHub.DistributedTask.Pipelines
{
    [DataContract]
    public class Snapshot
    {
        public Snapshot(string imageName, string condition = null, string version = null)
        {
            ImageName = imageName;
            Condition = condition ?? $"{PipelineTemplateConstants.Success}()";
            Version = version ?? "1.*";
        }

        [DataMember(EmitDefaultValue = false)]
        public String ImageName { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public String Condition { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public String Version { get; set; }
    }
}
