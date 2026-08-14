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
using System.ComponentModel;
using System.Runtime.Serialization;
using GitHub.DistributedTask.Expressions2.Sdk;
using GitHub.Services.WebApi.Internal;
using Newtonsoft.Json.Linq;

namespace GitHub.DistributedTask.Pipelines.ContextData
{
    [DataContract]
    [ClientIgnore]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public sealed class BooleanContextData : PipelineContextData, IBoolean
    {
        public BooleanContextData(Boolean value)
            : base(PipelineContextDataType.Boolean)
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

        public override PipelineContextData Clone()
        {
            return new BooleanContextData(m_value);
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

        public static implicit operator Boolean(BooleanContextData data)
        {
            return data.Value;
        }

        public static implicit operator BooleanContextData(Boolean data)
        {
            return new BooleanContextData(data);
        }

        [DataMember(Name = "b", EmitDefaultValue = false)]
        private Boolean m_value;
    }
}
