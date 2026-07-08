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

﻿using GitHub.DistributedTask.WebApi;
using System;

namespace GitHub.Runner.Common.Util
{
    public static class TaskResultUtil
    {
        private static readonly int _returnCodeOffset = 100;

        public static bool IsValidReturnCode(int returnCode)
        {
            int resultInt = returnCode - _returnCodeOffset;
            return Enum.IsDefined(typeof(TaskResult), resultInt);
        }

        public static int TranslateToReturnCode(TaskResult result)
        {
            return _returnCodeOffset + (int)result;
        }

        public static TaskResult TranslateFromReturnCode(int returnCode)
        {
            int resultInt = returnCode - _returnCodeOffset;
            if (Enum.IsDefined(typeof(TaskResult), resultInt))
            {
                return (TaskResult)resultInt;
            }
            else
            {
                return TaskResult.Failed;
            }
        }

        // Merge 2 TaskResults get the worst result.
        // Succeeded -> Failed/Canceled/Skipped/Abandoned
        // Failed -> Failed/Canceled
        // Canceled -> Canceled
        // Skipped -> Skipped
        // Abandoned -> Abandoned
        public static TaskResult MergeTaskResults(TaskResult? currentResult, TaskResult comingResult)
        {
            if (currentResult == null)
            {
                return comingResult;
            }

            // current result is Canceled/Skip/Abandoned
            if (currentResult > TaskResult.Failed)
            {
                return currentResult.Value;
            }

            // comming result is bad than current result
            if (comingResult >= currentResult)
            {
                return comingResult;
            }

            return currentResult.Value;
        }

        public static ActionResult ToActionResult(this TaskResult result)
        {
            switch (result)
            {
                case TaskResult.Succeeded:
                    return ActionResult.Success;
                case TaskResult.Failed:
                    return ActionResult.Failure;
                case TaskResult.Canceled:
                    return ActionResult.Cancelled;
                case TaskResult.Skipped:
                    return ActionResult.Skipped;
                default:
                    throw new NotSupportedException(result.ToString());
            }
        }
    }
}
