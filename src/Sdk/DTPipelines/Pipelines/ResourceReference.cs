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

﻿using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace GitHub.DistributedTask.Pipelines
{
    /// <summary>
    /// Provides a base set of properties common to all pipeline resource types.
    /// </summary>
    [DataContract]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public abstract class ResourceReference
    {
        protected ResourceReference()
        {
        }

        protected ResourceReference(ResourceReference referenceToCopy)
        {
            this.Name = referenceToCopy.Name;
        }

        /// <summary>
        /// Gets or sets the name of the referenced resource.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        [JsonConverter(typeof(ExpressionValueJsonConverter<String>))]
        public ExpressionValue<String> Name
        {
            get;
            set;
        }

        public override String ToString()
        {
            var name = this.Name;
            if (name != null)
            {
                var s = name.Literal;
                if (!String.IsNullOrEmpty(s))
                {
                    return s;
                }

                s = name.Expression;
                if (!String.IsNullOrEmpty(s))
                {
                    return s;
                }
            }

            return null;
        }
    }
}
