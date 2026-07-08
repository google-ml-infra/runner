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

﻿using GitHub.Services.Common;
using GitHub.Services.WebApi;
using System;
using System.Runtime.Serialization;

namespace GitHub.DistributedTask.WebApi
{
    [DataContract]
    public class TaskAttachment
    {
        internal TaskAttachment()
        { }

        internal TaskAttachment(String type, String name, ReferenceLinks links)
        {
            ArgumentUtility.CheckStringForNullOrEmpty(type, "type");
            ArgumentUtility.CheckStringForNullOrEmpty(name, "name");
            this.Type = type;
            this.Name = name;
            this.m_links = links;
        }

        public TaskAttachment(String type, String name)
        {
            ArgumentUtility.CheckStringForNullOrEmpty(type, "type");
            ArgumentUtility.CheckStringForNullOrEmpty(name, "name");
            this.Type = type;
            this.Name = name;
        }

        [DataMember]
        public String Type
        {
            get;
            internal set;
        }

        [DataMember]
        public String Name
        {
            get;
            internal set;
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

        [DataMember]
        public DateTime CreatedOn
        {
            get;
            internal set;
        }

        [DataMember]
        public DateTime LastChangedOn
        {
            get;
            internal set;
        }

        [DataMember]
        public Guid LastChangedBy
        {
            get;
            internal set;
        }

        [DataMember]
        public Guid TimelineId
        {
            get;
            set;
        }

        [DataMember]
        public Guid RecordId
        {
            get;
            set;
        }

        [DataMember(Name = "_links", EmitDefaultValue = false)]
        private ReferenceLinks m_links;
    }

    [GenerateAllConstants]
    public class CoreAttachmentType
    {
        public static readonly String Log = "DistributedTask.Core.Log";
        public static readonly String Summary = "DistributedTask.Core.Summary";
        public static readonly String FileAttachment = "DistributedTask.Core.FileAttachment";
        public static readonly String DiagnosticLog = "DistributedTask.Core.DiagnosticLog";
        public static readonly String ResultsLog = "Results.Core.Log";
        public static readonly String ResultsDiagnosticLog = "Results.Core.DiagnosticLog";
    }

    [GenerateAllConstants]
    public class ChecksAttachmentType
    {
        public static readonly String StepSummary = "Checks.Step.Summary";
    }
}
