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
using System.Runtime.Serialization;

namespace GitHub.DistributedTask.WebApi
{
    [DataContract]
    public class TimelineAttempt
    {
        /// <summary>
        /// Gets or sets the unique identifier for the record.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public String Identifier
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the attempt of the record.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public Int32 Attempt
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the timeline identifier which owns the record representing this attempt.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public Guid TimelineId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the record identifier located within the specified timeline.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public Guid RecordId
        {
            get;
            set;
        }
    }
}
