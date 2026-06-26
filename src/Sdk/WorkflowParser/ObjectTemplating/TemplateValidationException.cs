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

#nullable disable // Consider removing in the future to minimize likelihood of NullReferenceException; refer https://learn.microsoft.com/en-us/dotnet/csharp/nullable-references

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace GitHub.Actions.WorkflowParser.ObjectTemplating
{
    public class TemplateValidationException : Exception
    {
        public TemplateValidationException()
            : this(TemplateStrings.TemplateNotValid())
        {
        }

        public TemplateValidationException(IEnumerable<TemplateValidationError> errors)
            : this(TemplateStrings.TemplateNotValidWithErrors(string.Join(",", (errors ?? Enumerable.Empty<TemplateValidationError>()).Select(e => e.Message))))
        {
            m_errors = new List<TemplateValidationError>(errors ?? Enumerable.Empty<TemplateValidationError>());
        }

        public TemplateValidationException(
            String message,
            IEnumerable<TemplateValidationError> errors)
            : this(message)
        {
            m_errors = new List<TemplateValidationError>(errors ?? Enumerable.Empty<TemplateValidationError>());
        }

        public TemplateValidationException(String message)
            : base(message)
        {
        }

        public TemplateValidationException(
            String message,
            Exception innerException)
            : base(message, innerException)
        {
        }

        public IList<TemplateValidationError> Errors
        {
            get
            {
                if (m_errors == null)
                {
                    m_errors = new List<TemplateValidationError>();
                }
                return m_errors;
            }
        }

        private List<TemplateValidationError> m_errors;
    }
}
