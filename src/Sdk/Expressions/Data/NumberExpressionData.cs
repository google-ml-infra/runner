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
using System.Globalization;
using System.Runtime.Serialization;
using GitHub.Actions.Expressions.Sdk;
using Newtonsoft.Json.Linq;

namespace GitHub.Actions.Expressions.Data
{
    [DataContract]
    public sealed class NumberExpressionData : ExpressionData, INumber
    {
        public NumberExpressionData(Double value)
            : base(ExpressionDataType.Number)
        {
            m_value = value;
        }

        public Double Value
        {
            get
            {
                return m_value;
            }
        }

        public override ExpressionData Clone()
        {
            return new NumberExpressionData(m_value);
        }

        public override JToken ToJToken()
        {
            if (Double.IsNaN(m_value) || m_value == Double.PositiveInfinity || m_value == Double.NegativeInfinity)
            {
                return (JToken)m_value;
            }

            var floored = Math.Floor(m_value);
            if (m_value == floored && m_value <= (Double)Int32.MaxValue && m_value >= (Double)Int32.MinValue)
            {
                var flooredInt = (Int32)floored;
                return (JToken)flooredInt;
            }
            else if (m_value == floored && m_value <= (Double)Int64.MaxValue && m_value >= (Double)Int64.MinValue)
            {
                var flooredInt = (Int64)floored;
                return (JToken)flooredInt;
            }
            else
            {
                return (JToken)m_value;
            }
        }

        public override String ToString()
        {
            return m_value.ToString("G15", CultureInfo.InvariantCulture);
        }

        Double INumber.GetNumber()
        {
            return Value;
        }

        public static implicit operator Double(NumberExpressionData data)
        {
            return data.Value;
        }

        public static implicit operator NumberExpressionData(Double data)
        {
            return new NumberExpressionData(data);
        }

        [DataMember(Name = "n", EmitDefaultValue = false)]
        private Double m_value;
    }
}
