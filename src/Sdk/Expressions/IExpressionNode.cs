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

#nullable enable

using System;


namespace GitHub.Actions.Expressions
{
    public interface IExpressionNode
    {
        /// <summary>
        /// Evaluates the expression and returns the result, wrapped in a helper
        /// for converting, comparing, and traversing objects.
        /// </summary>
        /// <param name="trace">Optional trace writer</param>
        /// <param name="secretMasker">Optional secret masker</param>
        /// <param name="state">State object for custom evaluation function nodes and custom named-value nodes</param>
        /// <param name="options">Evaluation options</param>
        EvaluationResult Evaluate(
            ITraceWriter trace,
            ISecretMasker? secretMasker,
            Object state,
            EvaluationOptions options);
    }
}
