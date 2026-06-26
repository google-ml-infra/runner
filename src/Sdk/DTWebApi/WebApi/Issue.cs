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
using System.Runtime.Serialization;

namespace GitHub.DistributedTask.WebApi
{
    [DataContract]
    public class Issue
    {

        public Issue()
        {
        }

        private Issue(Issue issueToBeCloned)
        {
            this.Type = issueToBeCloned.Type;
            this.Category = issueToBeCloned.Category;
            this.Message = issueToBeCloned.Message;
            this.IsInfrastructureIssue = issueToBeCloned.IsInfrastructureIssue;

            if (issueToBeCloned.m_data != null)
            {
                foreach (var item in issueToBeCloned.m_data)
                {
                    this.Data.Add(item);
                }
            }
        }

        [DataMember(Order = 1)]
        public IssueType Type
        {
            get;
            set;
        }

        [DataMember(Order = 2)]
        public String Category
        {
            get;
            set;
        }

        [DataMember(Order = 3)]
        public String Message
        {
            get;
            set;
        }

        [DataMember(Order = 4)]
        public bool? IsInfrastructureIssue
        {
            get;
            set;
        }

        public IDictionary<String, String> Data
        {
            get
            {
                if (m_data == null)
                {
                    m_data = new Dictionary<String, String>(StringComparer.OrdinalIgnoreCase);
                }
                return m_data;
            }
        }

        public Issue Clone()
        {
            return new Issue(this);
        }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            SerializationHelper.Copy(ref m_serializedData, ref m_data, StringComparer.OrdinalIgnoreCase, true);
        }

        [OnSerializing]
        private void OnSerializing(StreamingContext context)
        {
            SerializationHelper.Copy(ref m_data, ref m_serializedData, StringComparer.OrdinalIgnoreCase);
        }

        [OnSerialized]
        private void OnSerialized(StreamingContext context)
        {
            m_serializedData = null;
        }

        [DataMember(Name = "Data", EmitDefaultValue = false, Order = 4)]
        private IDictionary<String, String> m_serializedData;

        private IDictionary<String, String> m_data;
    }
}
