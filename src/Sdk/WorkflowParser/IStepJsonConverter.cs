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

#nullable disable // Consider removing in the future to minimize likelihood of NullReferenceException; refer https://learn.microsoft.com/en-us/dotnet/csharp/nullable-references

using System;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GitHub.Actions.WorkflowParser
{
    internal sealed class IStepJsonConverter : JsonConverter
    {
        public override Boolean CanWrite
        {
            get
            {
                return false;
            }
        }

        public override Boolean CanConvert(Type objectType)
        {
            return typeof(IStep).GetTypeInfo().IsAssignableFrom(objectType.GetTypeInfo());
        }

        public override Object ReadJson(
            JsonReader reader,
            Type objectType,
            Object existingValue,
            JsonSerializer serializer)
        {
            if (reader.TokenType != JsonToken.StartObject)
            {
                return null;
            }

            JObject value = JObject.Load(reader);
            Object newValue = null;

            if (value.TryGetValue("Uses", StringComparison.OrdinalIgnoreCase, out _))
            {
                newValue = new ActionStep();
            }
            else if (value.TryGetValue("Run", StringComparison.OrdinalIgnoreCase, out _))
            {
                newValue = new RunStep();
            }
            else
            {
                return existingValue;
            }


            if (value != null)
            {
                using JsonReader objectReader = value.CreateReader();
                serializer.Populate(objectReader, newValue);
            }

            return newValue;
        }

        public override void WriteJson(
            JsonWriter writer,
            Object value,
            JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
