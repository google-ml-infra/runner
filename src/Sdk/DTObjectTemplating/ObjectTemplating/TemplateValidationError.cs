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
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using GitHub.Services.WebApi.Internal;

namespace GitHub.DistributedTask.ObjectTemplating
{
    /// <summary>
    /// Provides information about an error which occurred during validation.
    /// </summary>
    [DataContract]
    [ClientIgnore]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class TemplateValidationError
    {
        public TemplateValidationError()
        {
        }

        public TemplateValidationError(String message)
            : this(null, message)
        {
        }

        public TemplateValidationError(
            String code,
            String message)
        {
            Code = code;
            Message = message;
        }

        [DataMember(EmitDefaultValue = false)]
        public String Code
        {
            get;
            set;
        }

        [DataMember(EmitDefaultValue = false)]
        public String Message
        {
            get;
            set;
        }

        public static IEnumerable<TemplateValidationError> Create(Exception exception)
        {
            for (int i = 0; i < 50; i++)
            {
                yield return new TemplateValidationError(exception.Message);
                if (exception.InnerException == null)
                {
                    break;
                }

                exception = exception.InnerException;
            }
        }
    }
}
