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

#nullable enable

using System;
using GitHub.Actions.WorkflowParser.ObjectTemplating;
using GitHub.Actions.WorkflowParser.ObjectTemplating.Tokens;

namespace GitHub.Actions.WorkflowParser.Conversion
{
    internal sealed class JobCountValidator
    {
        public JobCountValidator(
            TemplateContext context,
            Int32 maxCount)
        {
            m_context = context ?? throw new ArgumentNullException(nameof(context));
            m_maxCount = maxCount;
        }

        /// <summary>
        /// Increments the job counter.
        ///
        /// Appends an error to the template context only when the max job count is initially exceeded.
        /// Additional calls will not append more errors.
        /// </summary>
        /// <param name="token">The token to use for error reporting.</param>
        public void Increment(TemplateToken? token)
        {
            // Initial breach?
            if (m_maxCount > 0 &&
                m_count + 1 > m_maxCount &&
                m_count <= m_maxCount)
            {
                m_context.Error(token, $"Workflows may not contain more than {m_maxCount} jobs across all referenced files");
            }

            // Increment
            m_count++;
        }

        private readonly TemplateContext m_context;
        private readonly Int32 m_maxCount;
        private Int32 m_count;
    }
}
