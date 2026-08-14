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
using System.Runtime.Serialization;

namespace GitHub.Services.Common
{
    /// <summary>
    /// A set of performance timings all keyed off of the same string
    /// </summary>
    [DataContract]
    public class PerformanceTimingGroup
    {
        public PerformanceTimingGroup()
        {
            this.Timings = new List<PerformanceTimingEntry>();
        }

        /// <summary>
        /// Overall duration of all entries in this group in ticks
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public long ElapsedTicks { get; set; }

        /// <summary>
        /// The total number of timing entries associated with this group
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public int Count { get; set; }

        /// <summary>
        /// A list of timing entries in this group. Only the first few entries in each group are collected.
        /// </summary>
        [DataMember]
        public List<PerformanceTimingEntry> Timings { get; private set; }
    }

    /// <summary>
    /// A single timing consisting of a duration and start time
    /// </summary>
    [DataContract]
    public struct PerformanceTimingEntry
    {
        /// <summary>
        /// Duration of the entry in ticks
        /// </summary>
        [DataMember]
        public long ElapsedTicks { get; set; }

        /// <summary>
        /// Offset from Server Request Context start time in microseconds
        /// </summary>
        [DataMember]
        public long StartOffset { get; set; }

        /// <summary>
        /// Properties to distinguish timings within the same group or to provide data to send with telemetry
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public IDictionary<String, Object> Properties { get; set; }
    }
}
