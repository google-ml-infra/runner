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

﻿using System;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace GitHub.Services.Common.Diagnostics
{
    /// <summary>
    /// Represents a trace activity for correlating diagnostic traces together. 
    /// </summary>
    [DataContract]
    [Serializable]
    public sealed class VssTraceActivity
    {
        private VssTraceActivity()
        {
        }

        private VssTraceActivity(Guid activityId)
        {
            this.Id = activityId;
        }

        /// <summary>
        /// Gets the unique identifier for the trace activity.
        /// </summary>
        [DataMember]
        public Guid Id
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the current trace activity if one is set on the current thread; otherwise, null.
        /// </summary>
        public static VssTraceActivity Current
        {
            get
            {
                return null;
            }
            set { }
        }

        /// <summary>
        /// Gets the empty trace activity.
        /// </summary>
        public static VssTraceActivity Empty
        {
            get
            {
                return s_empty.Value;
            }
        }

        /// <summary>
        /// Creates a disposable trace scope in which the current trace activity is activated for trace correlation. 
        /// The call context state for <see cref="VssTraceActivity.Current"/> is updated within the scope to reference
        /// the activated activity.
        /// </summary>
        /// <returns>A trace scope for correlating multiple traces together</returns>
        public IDisposable EnterCorrelationScope()
        {
            return new CorrelationScope(this);
        }

        /// <summary>
        /// Gets the current activity or, if no activity is active on the current thread, creates a new activity for
        /// trace correlation.
        /// </summary>
        /// <returns>The current trace activity or a new trace activity</returns>
        public static VssTraceActivity GetOrCreate()
        {
            if (VssTraceActivity.Current != null)
            {
                return VssTraceActivity.Current;
            }
            else if (Trace.CorrelationManager.ActivityId == Guid.Empty)
            {
                return new VssTraceActivity(Guid.NewGuid());
            }
            else
            {
                return new VssTraceActivity(Trace.CorrelationManager.ActivityId);
            }
        }

        /// <summary>
        /// Creates a new trace activity optionally using the provided identifier.
        /// </summary>
        /// <param name="activityId">The activity identifier or none to have one generated</param>
        /// <returns>A new trace activity instance</returns>
        public static VssTraceActivity New(Guid activityId = default(Guid))
        {
            return new VssTraceActivity(activityId == default(Guid) ? Guid.NewGuid() : activityId);
        }

        /// <summary>
        /// Gets the property name used to cache this object on extensible objects.
        /// </summary>
        public const String PropertyName = "MS.VSS.Diagnostics.TraceActivity";
        private static Lazy<VssTraceActivity> s_empty = new Lazy<VssTraceActivity>(() => new VssTraceActivity(Guid.Empty));

        private sealed class CorrelationScope : IDisposable
        {
            public CorrelationScope(VssTraceActivity activity)
            {
                m_previousActivity = VssTraceActivity.Current;
                if (m_previousActivity == null || m_previousActivity.Id != activity.Id)
                {
                    m_swap = true;
                    VssTraceActivity.Current = activity;
                }
            }

            public void Dispose()
            {
                if (m_swap)
                {
                    try
                    {
                        m_swap = false;
                    }
                    finally
                    {
                        // Perform in a finally block to ensure consistency between the two variables
                        VssTraceActivity.Current = m_previousActivity;
                    }
                }
            }

            private Boolean m_swap;
            private VssTraceActivity m_previousActivity;
        }
    }
}
