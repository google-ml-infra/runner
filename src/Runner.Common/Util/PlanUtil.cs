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
using GitHub.DistributedTask.WebApi;
using GitHub.Runner.Sdk;

namespace GitHub.Runner.Common.Util
{
    public static class PlanUtil
    {
        public static PlanFeatures GetFeatures(TaskOrchestrationPlanReference plan)
        {
            ArgUtil.NotNull(plan, nameof(plan));
            PlanFeatures features = PlanFeatures.None;
            if (plan.Version >= 8)
            {
                features |= PlanFeatures.JobCompletedPlanEvent;
            }

            return features;
        }
    }

    [Flags]
    public enum PlanFeatures
    {
        None = 0,
        JobCompletedPlanEvent = 1,
    }
}
