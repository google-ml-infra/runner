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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GitHub.DistributedTask.Expressions2;
using GitHub.DistributedTask.Expressions2.Sdk;
using GitHub.DistributedTask.ObjectTemplating;
using GitHub.DistributedTask.WebApi;
using GitHub.Runner.Common;
using GitHub.Runner.Common.Util;
using GitHub.Runner.Sdk;
using ObjectTemplating = GitHub.DistributedTask.ObjectTemplating;
using PipelineTemplateConstants = GitHub.DistributedTask.Pipelines.ObjectTemplating.PipelineTemplateConstants;

namespace GitHub.Runner.Worker.Expressions
{
    public sealed class CancelledFunction : Function
    {
        protected sealed override object EvaluateCore(EvaluationContext evaluationContext, out ResultMemory resultMemory)
        {
            resultMemory = null;
            var templateContext = evaluationContext.State as TemplateContext;
            ArgUtil.NotNull(templateContext, nameof(templateContext));
            var executionContext = templateContext.State[nameof(IExecutionContext)] as IExecutionContext;
            ArgUtil.NotNull(executionContext, nameof(executionContext));
            ActionResult jobStatus = executionContext.JobContext.Status ?? ActionResult.Success;
            return jobStatus == ActionResult.Cancelled;
        }
    }

    public sealed class NewCancelledFunction : GitHub.Actions.Expressions.Sdk.Function
    {
        protected sealed override object EvaluateCore(GitHub.Actions.Expressions.Sdk.EvaluationContext evaluationContext, out GitHub.Actions.Expressions.Sdk.ResultMemory resultMemory)
        {
            resultMemory = null;
            var templateContext = evaluationContext.State as GitHub.Actions.WorkflowParser.ObjectTemplating.TemplateContext;
            ArgUtil.NotNull(templateContext, nameof(templateContext));
            var executionContext = templateContext.State[nameof(IExecutionContext)] as IExecutionContext;
            ArgUtil.NotNull(executionContext, nameof(executionContext));
            ActionResult jobStatus = executionContext.JobContext.Status ?? ActionResult.Success;
            return jobStatus == ActionResult.Cancelled;
        }
    }
}
