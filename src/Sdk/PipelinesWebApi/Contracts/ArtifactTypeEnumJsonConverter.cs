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
using Newtonsoft.Json.Serialization;
using System;
using System.Net.Http.Formatting;

namespace GitHub.Actions.Pipelines.WebApi
{
    public class ArtifactTypeEnumJsonConverter : UnknownEnumJsonConverter
    {
        //json.net v12 exposes a "NamingStrategy" member that can do all this. We are at json.net v10 which only supports camel case.
        //This is a poor man's way to fake it 
        public override void WriteJson(JsonWriter writer, object enumValue, JsonSerializer serializer)
        {
            var value = (ArtifactType)enumValue;
            if (value == ArtifactType.Actions_Storage)
            {
                writer.WriteValue("actions_storage");
            }
            else
            {
                base.WriteJson(writer, enumValue, serializer);
            }
        }
    }
}
