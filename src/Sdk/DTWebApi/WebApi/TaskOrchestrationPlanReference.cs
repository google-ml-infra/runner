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

namespace GitHub.DistributedTask.WebApi
{
    [DataContract]
    public class TaskOrchestrationPlanReference
    {
        [DataMember]
        public Guid ScopeIdentifier
        {
            get;
            set;
        }

        [DataMember]
        public String PlanType
        {
            get;
            set;
        }

        [DataMember]
        public Int32 Version
        {
            get;
            set;
        }

        [DataMember]
        public Guid PlanId
        {
            get;
            set;
        }

        [DataMember]
        public String PlanGroup
        {
            get;
            set;
        }

        [DataMember]
        public Uri ArtifactUri
        {
            get;
            set;
        }

        [DataMember]
        public Uri ArtifactLocation
        {
            get;
            set;
        }

        [IgnoreDataMember]
        internal Int64 ContainerId
        {
            get;
            set;
        }

        [DataMember]
        public TaskOrchestrationOwner Definition
        {
            get;
            set;
        }

        [DataMember]
        public TaskOrchestrationOwner Owner
        {
            get;
            set;
        }
    }
}
