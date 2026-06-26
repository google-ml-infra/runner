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
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace GitHub.Actions.WorkflowParser
{
    public class WorkflowValidationException : Exception
    {
        public WorkflowValidationException()
            : this(WorkflowStrings.WorkflowNotValid())
        {
        }

        public WorkflowValidationException(IEnumerable<WorkflowValidationError> errors)
            : this(WorkflowStrings.WorkflowNotValidWithErrors(string.Join(" ", (errors ?? Enumerable.Empty<WorkflowValidationError>()).Take(ErrorCount).Select(e => e.Message))))
        {
            m_errors = new List<WorkflowValidationError>(errors ?? Enumerable.Empty<WorkflowValidationError>());
        }

        public WorkflowValidationException(String message)
            : base(message)
        {
        }

        public WorkflowValidationException(
            String message,
            Exception innerException)
            : base(message, innerException)
        {
        }

        internal IReadOnlyList<WorkflowValidationError> Errors => (m_errors ?? new List<WorkflowValidationError>()).AsReadOnly();

        private List<WorkflowValidationError>? m_errors;

        /// <summary>
        /// Previously set to 2 when there were UI limitations.
        /// Setting this to 10 to increase the number of errors returned from parser.
        /// </summary>
        private const int ErrorCount = 10;
    }
}
