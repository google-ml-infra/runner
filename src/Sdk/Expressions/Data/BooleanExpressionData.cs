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

﻿using System;
using System.Runtime.Serialization;
using GitHub.Actions.Expressions.Sdk;
using Newtonsoft.Json.Linq;

namespace GitHub.Actions.Expressions.Data
{
    [DataContract]
    public sealed class BooleanExpressionData : ExpressionData, IBoolean
    {
        public BooleanExpressionData(Boolean value)
            : base(ExpressionDataType.Boolean)
        {
            m_value = value;
        }

        public Boolean Value
        {
            get
            {
                return m_value;
            }
        }

        public override ExpressionData Clone()
        {
            return new BooleanExpressionData(m_value);
        }

        public override JToken ToJToken()
        {
            return (JToken)m_value;
        }

        public override String ToString()
        {
            return m_value ? "true" : "false";
        }

        Boolean IBoolean.GetBoolean()
        {
            return Value;
        }

        public static implicit operator Boolean(BooleanExpressionData data)
        {
            return data.Value;
        }

        public static implicit operator BooleanExpressionData(Boolean data)
        {
            return new BooleanExpressionData(data);
        }

        [DataMember(Name = "b", EmitDefaultValue = false)]
        private Boolean m_value;
    }
}
