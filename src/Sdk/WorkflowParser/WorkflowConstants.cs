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

using System;

namespace GitHub.Actions.WorkflowParser
{
    public static class WorkflowConstants
    {
        /// <summary>
        /// The default job cancel timeout in minutes.
        /// </summary>
        internal const Int32 DefaultJobCancelTimeoutInMinutes = 5;

        /// <summary>
        /// The default job name. This job name is used when a job does not leverage multipliers
        /// or slicing and only has one implicit job.
        /// </summary>
        internal const String DefaultJobName = "__default";

        /// <summary>
        /// The default job timeout in minutes.
        /// </summary>
        internal const Int32 DefaultJobTimeoutInMinutes = 360;

        /// <summary>
        /// The max length for a node within a workflow - e.g. a job ID or a matrix configuration ID.
        /// </summary>
        internal const Int32 MaxNodeNameLength = 100;

        /// <summary>
        /// Alias for the self repository.
        /// </summary>
        internal const String SelfAlias = "self";

        public static class PermissionsPolicy
        {
            public const string LimitedRead = "LimitedRead";
            public const string Write = "Write";
        }
    }
}
