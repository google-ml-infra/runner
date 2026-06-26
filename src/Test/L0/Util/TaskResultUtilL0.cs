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

﻿using GitHub.DistributedTask.WebApi;
using GitHub.Runner.Common.Util;
using Xunit;

namespace GitHub.Runner.Common.Tests.Util
{
    public class TaskResultUtilL0
    {
        [Fact]
        [Trait("Level", "L0")]
        [Trait("Category", "Common")]
        public void TaskResultReturnCodeTranslate()
        {
            // Arrange.
            using (TestHostContext hc = new(this))
            {
                // Act.
                TaskResult abandon = TaskResultUtil.TranslateFromReturnCode(TaskResultUtil.TranslateToReturnCode(TaskResult.Abandoned));
                // Actual
                Assert.Equal(TaskResult.Abandoned, abandon);

                // Act.
                TaskResult canceled = TaskResultUtil.TranslateFromReturnCode(TaskResultUtil.TranslateToReturnCode(TaskResult.Canceled));
                // Actual
                Assert.Equal(TaskResult.Canceled, canceled);

                // Act.
                TaskResult failed = TaskResultUtil.TranslateFromReturnCode(TaskResultUtil.TranslateToReturnCode(TaskResult.Failed));
                // Actual
                Assert.Equal(TaskResult.Failed, failed);

                // Act.
                TaskResult skipped = TaskResultUtil.TranslateFromReturnCode(TaskResultUtil.TranslateToReturnCode(TaskResult.Skipped));
                // Actual
                Assert.Equal(TaskResult.Skipped, skipped);

                // Act.
                TaskResult succeeded = TaskResultUtil.TranslateFromReturnCode(TaskResultUtil.TranslateToReturnCode(TaskResult.Succeeded));
                // Actual
                Assert.Equal(TaskResult.Succeeded, succeeded);

                // Act.
                TaskResult unknowReturnCode1 = TaskResultUtil.TranslateFromReturnCode(0);
                // Actual
                Assert.Equal(TaskResult.Failed, unknowReturnCode1);

                // Act.
                TaskResult unknowReturnCode2 = TaskResultUtil.TranslateFromReturnCode(1);
                // Actual
                Assert.Equal(TaskResult.Failed, unknowReturnCode2);
            }
        }

        [Fact]
        [Trait("Level", "L0")]
        [Trait("Category", "Common")]
        public void TaskResultsMerge()
        {
            // Arrange.
            using (TestHostContext hc = new(this))
            {
                TaskResult merged;

                //
                // No current result merge.
                //
                // Act.
                merged = TaskResultUtil.MergeTaskResults(null, TaskResult.Succeeded);
                // Actual
                Assert.Equal(TaskResult.Succeeded, merged);
                // Act.
                merged = TaskResultUtil.MergeTaskResults(null, TaskResult.Abandoned);
                // Actual
                Assert.Equal(TaskResult.Abandoned, merged);
                // Act.
                merged = TaskResultUtil.MergeTaskResults(null, TaskResult.Canceled);
                // Actual
                Assert.Equal(TaskResult.Canceled, merged);
                // Act.
                merged = TaskResultUtil.MergeTaskResults(null, TaskResult.Failed);
                // Actual
                Assert.Equal(TaskResult.Failed, merged);
                // Act.
                merged = TaskResultUtil.MergeTaskResults(null, TaskResult.Skipped);
                // Actual
                Assert.Equal(TaskResult.Skipped, merged);

                //
                // Same result merge.
                //
                // Act.
                merged = TaskResultUtil.MergeTaskResults(TaskResult.Succeeded, TaskResult.Succeeded);
                // Actual
                Assert.Equal(TaskResult.Succeeded, merged);
                // Act.
                merged = TaskResultUtil.MergeTaskResults(TaskResult.Abandoned, TaskResult.Abandoned);
                // Actual
                Assert.Equal(TaskResult.Abandoned, merged);
                // Act.
                merged = TaskResultUtil.MergeTaskResults(TaskResult.Canceled, TaskResult.Canceled);
                // Actual
                Assert.Equal(TaskResult.Canceled, merged);
                // Act.
                merged = TaskResultUtil.MergeTaskResults(TaskResult.Failed, TaskResult.Failed);
                // Actual
                Assert.Equal(TaskResult.Failed, merged);
                // Act.
                merged = TaskResultUtil.MergeTaskResults(TaskResult.Skipped, TaskResult.Skipped);
                // Actual
                Assert.Equal(TaskResult.Skipped, merged);

                //
                // Forward result merge
                //
                // Act.
                merged = TaskResultUtil.MergeTaskResults(TaskResult.Succeeded, TaskResult.Abandoned);
                // Actual
                Assert.Equal(TaskResult.Abandoned, merged);
                // Act.
                merged = TaskResultUtil.MergeTaskResults(TaskResult.Succeeded, TaskResult.Canceled);
                // Actual
                Assert.Equal(TaskResult.Canceled, merged);
                // Act.
                merged = TaskResultUtil.MergeTaskResults(TaskResult.Succeeded, TaskResult.Failed);
                // Actual
                Assert.Equal(TaskResult.Failed, merged);
                // Act.
                merged = TaskResultUtil.MergeTaskResults(TaskResult.Succeeded, TaskResult.Skipped);
                // Actual
                Assert.Equal(TaskResult.Skipped, merged);

                //
                // No backward merge
                //
                // Act.
                merged = TaskResultUtil.MergeTaskResults(TaskResult.Abandoned, TaskResult.Succeeded);
                // Actual
                Assert.Equal(TaskResult.Abandoned, merged);
                // Act.
                merged = TaskResultUtil.MergeTaskResults(TaskResult.Canceled, TaskResult.Succeeded);
                // Actual
                Assert.Equal(TaskResult.Canceled, merged);
                // Act.
                merged = TaskResultUtil.MergeTaskResults(TaskResult.Failed, TaskResult.Succeeded);
                // Actual
                Assert.Equal(TaskResult.Failed, merged);
                // Act.
                merged = TaskResultUtil.MergeTaskResults(TaskResult.Skipped, TaskResult.Succeeded);
                // Actual
                Assert.Equal(TaskResult.Skipped, merged);

                //
                // Worst result no change
                //
                // Act.
                merged = TaskResultUtil.MergeTaskResults(TaskResult.Abandoned, TaskResult.Canceled);
                // Actual
                Assert.Equal(TaskResult.Abandoned, merged);
                // Act.
                merged = TaskResultUtil.MergeTaskResults(TaskResult.Abandoned, TaskResult.Failed);
                // Actual
                Assert.Equal(TaskResult.Abandoned, merged);
                // Act.
                merged = TaskResultUtil.MergeTaskResults(TaskResult.Abandoned, TaskResult.Skipped);
                // Actual
                Assert.Equal(TaskResult.Abandoned, merged);
                // Act.
                merged = TaskResultUtil.MergeTaskResults(TaskResult.Canceled, TaskResult.Abandoned);
                // Actual
                Assert.Equal(TaskResult.Canceled, merged);
                // Act.
                merged = TaskResultUtil.MergeTaskResults(TaskResult.Canceled, TaskResult.Failed);
                // Actual
                Assert.Equal(TaskResult.Canceled, merged);
                // Act.
                merged = TaskResultUtil.MergeTaskResults(TaskResult.Canceled, TaskResult.Skipped);
                // Actual
                Assert.Equal(TaskResult.Canceled, merged);
                // Act.
                merged = TaskResultUtil.MergeTaskResults(TaskResult.Failed, TaskResult.Abandoned);
                // Actual
                Assert.Equal(TaskResult.Abandoned, merged);
                // Act.
                merged = TaskResultUtil.MergeTaskResults(TaskResult.Failed, TaskResult.Canceled);
                // Actual
                Assert.Equal(TaskResult.Canceled, merged);
                // Act.
                merged = TaskResultUtil.MergeTaskResults(TaskResult.Failed, TaskResult.Skipped);
                // Actual
                Assert.Equal(TaskResult.Skipped, merged);
                // Act.
                merged = TaskResultUtil.MergeTaskResults(TaskResult.Skipped, TaskResult.Abandoned);
                // Actual
                Assert.Equal(TaskResult.Skipped, merged);
                // Act.
                merged = TaskResultUtil.MergeTaskResults(TaskResult.Skipped, TaskResult.Canceled);
                // Actual
                Assert.Equal(TaskResult.Skipped, merged);
                // Act.
                merged = TaskResultUtil.MergeTaskResults(TaskResult.Skipped, TaskResult.Failed);
                // Actual
                Assert.Equal(TaskResult.Skipped, merged);
            }
        }
    }
}
