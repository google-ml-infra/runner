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
using System.Runtime.Serialization;

namespace GitHub.DistributedTask.WebApi
{
    [DataContract]
    public class VariableValue
    {
        public VariableValue()
        {
        }

        public VariableValue(VariableValue value)
            : this(value.Value, value.IsSecret)
        {
        }

        public VariableValue(String value, Boolean isSecret)
        {
            Value = value;
            IsSecret = isSecret;
        }

        [DataMember(EmitDefaultValue = true)]
        public String Value
        {
            get;
            set;
        }

        [DataMember(EmitDefaultValue = false)]
        public Boolean IsSecret
        {
            get;
            set;
        }

        public static implicit operator VariableValue(String value)
        {
            return new VariableValue(value, false);
        }
    }
}
