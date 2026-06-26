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
using System.Reflection;
using GitHub.Services.WebApi;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GitHub.DistributedTask.Pipelines
{
    internal sealed class ActionStepDefinitionReferenceConverter : VssSecureJsonConverter
    {
        public override bool CanWrite
        {
            get
            {
                return false;
            }
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(Step).GetTypeInfo().IsAssignableFrom(objectType.GetTypeInfo());
        }

        public override object ReadJson(
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
            if (value.TryGetValue("Type", StringComparison.OrdinalIgnoreCase, out JToken actionTypeValue))
            {
                ActionSourceType actionType;
                if (actionTypeValue.Type == JTokenType.Integer)
                {
                    actionType = (ActionSourceType)(Int32)actionTypeValue;
                }
                else if (actionTypeValue.Type != JTokenType.String || !Enum.TryParse((String)actionTypeValue, true, out actionType))
                {
                    return null;
                }

                ActionStepDefinitionReference reference = null;
                switch (actionType)
                {
                    case ActionSourceType.Repository:
                        reference = new RepositoryPathReference();
                        break;

                    case ActionSourceType.ContainerRegistry:
                        reference = new ContainerRegistryReference();
                        break;

                    case ActionSourceType.Script:
                        reference = new ScriptReference();
                        break;
                }

                using (var objectReader = value.CreateReader())
                {
                    serializer.Populate(objectReader, reference);
                }

                return reference;
            }
            else
            {
                return null;
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
