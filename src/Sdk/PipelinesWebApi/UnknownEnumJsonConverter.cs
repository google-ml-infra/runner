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
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace GitHub.Actions.Pipelines.WebApi
{
    public class UnknownEnumJsonConverter : StringEnumConverter
    {
        public UnknownEnumJsonConverter()
        {
            this.NamingStrategy = new CamelCaseNamingStrategy();
        }

        public override bool CanConvert(Type objectType)
        {
            // we require one member to be named "Unknown"
            return objectType.IsEnum && Enum.GetNames(objectType).Any(name => string.Equals(name, UnknownName, StringComparison.OrdinalIgnoreCase));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            // Newtonsoft doesn't call CanConvert if you specify the converter using a JsonConverter attribute
            // they just assume you know what you're doing :)
            if (!CanConvert(objectType))
            {
                // if there's no Unknown value, fall back to the StringEnumConverter behavior
                return base.ReadJson(reader, objectType, existingValue, serializer);
            }

            if (reader.TokenType == JsonToken.Integer)
            {
                var intValue = Convert.ToInt32(reader.Value);
                var values = (int[])Enum.GetValues(objectType);
                if (values.Contains(intValue))
                {
                    return Enum.Parse(objectType, intValue.ToString());
                }
            }

            if (reader.TokenType == JsonToken.String)
            {
                var stringValue = reader.Value.ToString();
                return UnknownEnum.Parse(objectType, stringValue);
            }

            // we know there's an Unknown value because CanConvert returned true
            return Enum.Parse(objectType, UnknownName);
        }

        private const string UnknownName = "Unknown";
    }
}
