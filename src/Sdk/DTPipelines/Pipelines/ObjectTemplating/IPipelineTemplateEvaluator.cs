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
using System.Collections.Generic;
using GitHub.DistributedTask.Expressions2;
using GitHub.DistributedTask.ObjectTemplating.Tokens;
using GitHub.DistributedTask.Pipelines.ContextData;

namespace GitHub.DistributedTask.Pipelines.ObjectTemplating
{
    /// <summary>
    /// Evaluates parts of the workflow DOM. For example, a job strategy or step inputs.
    /// </summary>
    public interface IPipelineTemplateEvaluator
    {
        Boolean EvaluateStepContinueOnError(
            TemplateToken token,
            DictionaryContextData contextData,
            IList<IFunctionInfo> expressionFunctions);

        String EvaluateStepDisplayName(
            TemplateToken token,
            DictionaryContextData contextData,
            IList<IFunctionInfo> expressionFunctions);

        Dictionary<String, String> EvaluateStepEnvironment(
            TemplateToken token,
            DictionaryContextData contextData,
            IList<IFunctionInfo> expressionFunctions,
            StringComparer keyComparer);

        Boolean EvaluateStepIf(
            TemplateToken token,
            DictionaryContextData contextData,
            IList<IFunctionInfo> expressionFunctions,
            IEnumerable<KeyValuePair<String, Object>> expressionState);

        Dictionary<String, String> EvaluateStepInputs(
            TemplateToken token,
            DictionaryContextData contextData,
            IList<IFunctionInfo> expressionFunctions);

        Int32 EvaluateStepTimeout(
            TemplateToken token,
            DictionaryContextData contextData,
            IList<IFunctionInfo> expressionFunctions);

        JobContainer EvaluateJobContainer(
            TemplateToken token,
            DictionaryContextData contextData,
            IList<IFunctionInfo> expressionFunctions);

        Dictionary<String, String> EvaluateJobOutput(
            TemplateToken token,
            DictionaryContextData contextData,
            IList<IFunctionInfo> expressionFunctions);

        TemplateToken EvaluateEnvironmentUrl(
            TemplateToken token,
            DictionaryContextData contextData,
            IList<IFunctionInfo> expressionFunctions);

        Dictionary<String, String> EvaluateJobDefaultsRun(
            TemplateToken token,
            DictionaryContextData contextData,
            IList<IFunctionInfo> expressionFunctions);

        IList<KeyValuePair<String, JobContainer>> EvaluateJobServiceContainers(
            TemplateToken token,
            DictionaryContextData contextData,
            IList<IFunctionInfo> expressionFunctions);

        Snapshot EvaluateJobSnapshotRequest(
            TemplateToken token,
            DictionaryContextData contextData,
            IList<IFunctionInfo> expressionFunctions);
    }
}
