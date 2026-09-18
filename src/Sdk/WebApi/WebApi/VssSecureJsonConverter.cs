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
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GitHub.Services.WebApi
{
    public abstract class VssSecureJsonConverter : JsonConverter
    {
        public override abstract bool CanConvert(Type objectType);

        public override abstract object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer);

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Validate(value, serializer);
        }

        private void Validate(object value, JsonSerializer serializer)
        {
            VssSecureJsonConverterHelper.Validate?.Invoke(value, serializer);
        }
    }

    public abstract class VssSecureCustomCreationConverter<T> : CustomCreationConverter<T>
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Validate(value, serializer);
        }

        private void Validate(object value, JsonSerializer serializer)
        {
            VssSecureJsonConverterHelper.Validate?.Invoke(value, serializer);
        }
    }

    public abstract class VssSecureDateTimeConverterBase : DateTimeConverterBase
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Validate(value, serializer);
        }

        private void Validate(object value, JsonSerializer serializer)
        {
            VssSecureJsonConverterHelper.Validate?.Invoke(value, serializer);
        }
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class VssSecureJsonConverterHelper
    {
        /// <summary>
        /// The action to validate the object being converted.
        /// </summary>
        public static Action<object, JsonSerializer> Validate { get; set; }
    }
}
