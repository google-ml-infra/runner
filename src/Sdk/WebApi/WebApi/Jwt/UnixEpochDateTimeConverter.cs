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
using GitHub.Services.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GitHub.Services.WebApi.Jwt
{
    class UnixEpochDateTimeConverter : DateTimeConverterBase
    {
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            long unixVal = reader.Value is string ? long.Parse((string)reader.Value) : (long)reader.Value;

            return unixVal.FromUnixEpochTime();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            long unixVal = ((DateTime)value).ToUnixEpochTime();

            writer.WriteValue(unixVal);
        }
    }
}
