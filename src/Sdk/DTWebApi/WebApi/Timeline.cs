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
using System.Linq;
using System.Runtime.Serialization;

namespace GitHub.DistributedTask.WebApi
{
    [DataContract]
    public sealed class Timeline : TimelineReference
    {
        public Timeline()
        {
        }

        public Timeline(Guid timelineId)
        {
            this.Id = timelineId;
        }

        private Timeline(Timeline timelineToBeCloned)
        {
            this.ChangeId = timelineToBeCloned.ChangeId;
            this.Id = timelineToBeCloned.Id;
            this.LastChangedBy = timelineToBeCloned.LastChangedBy;
            this.LastChangedOn = timelineToBeCloned.LastChangedOn;
            this.Location = timelineToBeCloned.Location;

            if (timelineToBeCloned.m_records != null)
            {
                m_records = timelineToBeCloned.m_records.Select(x => x.Clone()).ToList();
            }
        }

        [DataMember]
        public Guid LastChangedBy
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

        public List<TimelineRecord> Records
        {
            get
            {
                if (m_records == null)
                {
                    m_records = new List<TimelineRecord>();
                }
                return m_records;
            }
        }

        public Timeline Clone()
        {
            return new Timeline(this);
        }

        [OnSerializing]
        private void OnSerializing(StreamingContext context)
        {
            if (m_records?.Count == 0)
            {
                m_records = null;
            }
        }

        [DataMember(Name = "Records", EmitDefaultValue = false, Order = 4)]
        private List<TimelineRecord> m_records;
    }
}
