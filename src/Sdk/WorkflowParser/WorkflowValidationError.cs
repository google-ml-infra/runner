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
using System.Runtime.Serialization;

namespace GitHub.Actions.WorkflowParser
{
    /// <summary>
    /// Provides information about an error which occurred during workflow validation.
    /// </summary>
    [DataContract]
    public class WorkflowValidationError
    {
        public WorkflowValidationError()
        {
        }

        public WorkflowValidationError(String? message)
            : this(null, message)
        {
        }

        public WorkflowValidationError(
            String? code,
            String? message)
        {
            Code = code;
            Message = message;
        }

        [DataMember(EmitDefaultValue = false)]
        public String? Code
        {
            get;
            set;
        }

        [DataMember(EmitDefaultValue = false)]
        public String? Message
        {
            get;
            set;
        }

        internal WorkflowValidationError Clone()
        {
            return new WorkflowValidationError(Code, Message);
        }

        public static IEnumerable<WorkflowValidationError> Create(Exception exception)
        {
            for (int i = 0; i < 50; i++)
            {
                yield return new WorkflowValidationError(exception.Message);
                if (exception.InnerException == null)
                {
                    break;
                }

                exception = exception.InnerException;
            }
        }
    }
}
