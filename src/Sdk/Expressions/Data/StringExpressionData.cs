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

#nullable disable // Consider removing in the future to minimize likelihood of NullReferenceException; refer https://learn.microsoft.com/en-us/dotnet/csharp/nullable-references

using System;
using System.Runtime.Serialization;
using GitHub.Actions.Expressions.Sdk;
using Newtonsoft.Json.Linq;

namespace GitHub.Actions.Expressions.Data
{
    [DataContract]
    public sealed class StringExpressionData : ExpressionData, IString
    {
        public StringExpressionData(String value)
            : base(ExpressionDataType.String)
        {
            m_value = value;
        }

        public String Value
        {
            get
            {
                if (m_value == null)
                {
                    m_value = String.Empty;
                }

                return m_value;
            }
        }

        public override ExpressionData Clone()
        {
            return new StringExpressionData(m_value);
        }

        public override JToken ToJToken()
        {
            return (JToken)m_value;
        }

        String IString.GetString()
        {
            return Value;
        }

        public override String ToString()
        {
            return Value;
        }

        public static implicit operator String(StringExpressionData data)
        {
            return data.Value;
        }

        public static implicit operator StringExpressionData(String data)
        {
            return new StringExpressionData(data);
        }

        [OnSerializing]
        private void OnSerializing(StreamingContext context)
        {
            if (m_value?.Length == 0)
            {
                m_value = null;
            }
        }

        [DataMember(Name = "s", EmitDefaultValue = false)]
        private String m_value;
    }
}