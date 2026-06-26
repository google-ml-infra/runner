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

﻿using GitHub.Services.WebApi;
using System;
using System.Runtime.Serialization;

namespace GitHub.DistributedTask.WebApi
{
    [DataContract]
    public class TaskOrchestrationOwner : ICloneable
    {
        public TaskOrchestrationOwner()
        {
        }

        private TaskOrchestrationOwner(TaskOrchestrationOwner ownerToBeCloned)
        {
            this.Id = ownerToBeCloned.Id;
            this.Name = ownerToBeCloned.Name;
            this.m_links = ownerToBeCloned.Links.Clone();
        }

        [DataMember]
        public Int32 Id
        {
            get;
            set;
        }

        [DataMember]
        public String Name
        {
            get;
            set;
        }

        public ReferenceLinks Links
        {
            get
            {
                if (m_links == null)
                {
                    m_links = new ReferenceLinks();
                }
                return m_links;
            }
        }

        public TaskOrchestrationOwner Clone()
        {
            return new TaskOrchestrationOwner(this);
        }

        Object ICloneable.Clone()
        {
            return this.Clone();
        }

        [DataMember(Name = "_links")]
        private ReferenceLinks m_links;
    }
}
