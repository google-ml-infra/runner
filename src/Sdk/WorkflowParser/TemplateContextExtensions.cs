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
using GitHub.Actions.WorkflowParser.Conversion;
using GitHub.Actions.WorkflowParser.ObjectTemplating;

namespace GitHub.Actions.WorkflowParser
{
    /// <summary>
    /// Extension methods for <see cref="TemplateContext"/>
    /// </summary>
    internal static class TemplateContextExtensions
    {
        /// <summary>
        /// Stores the <see cref="WorkflowFeatures"/> in the <see cref="TemplateContext"/> state.
        /// </summary>
        public static void SetFeatures(
            this TemplateContext context,
            WorkflowFeatures features)
        {
            context.State[s_featuresKey] = features;
        }

        /// <summary>
        /// Gets the <see cref="WorkflowFeatures"/> from the <see cref="TemplateContext"/> state.
        /// </summary>
        public static WorkflowFeatures GetFeatures(this TemplateContext context)
        {
            if (context.State.TryGetValue(s_featuresKey, out var value) &&
                value is WorkflowFeatures features)
            {
                return features;
            }

            throw new ArgumentNullException(nameof(WorkflowFeatures));
        }

        /// <summary>
        /// Stores the <see cref="JobCountValidator"/> in the <see cref="TemplateContext"/> state.
        /// </summary>
        public static void SetJobCountValidator(
            this TemplateContext context,
            JobCountValidator validator)
        {
            context.State[s_jobCountValidatorKey] = validator;
        }

        /// <summary>
        /// Gets the <see cref="JobCountValidator"/> from the <see cref="TemplateContext"/> state.
        /// </summary>
        public static JobCountValidator GetJobCountValidator(this TemplateContext context)
        {
            if (context.State.TryGetValue(s_jobCountValidatorKey, out var value) &&
                value is JobCountValidator validator)
            {
                return validator;
            }

            throw new ArgumentNullException(nameof(JobCountValidator));
        }

        /// <summary>
        /// Lookup key for the <see cref="WorkflowFeatures"/> object within the state dictionary.
        /// </summary>
        private static readonly string s_featuresKey = typeof(WorkflowFeatures).FullName!;

        /// <summary>
        /// Lookup key for the <see cref="JobCountValidator"/> object within the state dictionary.
        /// </summary>
        private static readonly string s_jobCountValidatorKey = typeof(JobCountValidator).FullName!;
    }
}
