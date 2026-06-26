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

﻿using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace GitHub.Actions.Pipelines.WebApi
{
    [DataContract]
    [KnownType(typeof(CreateActionsStorageArtifactParameters))]
    [JsonConverter(typeof(CreateArtifactParametersJsonConverter))]
    public class CreateArtifactParameters
    {
        protected CreateArtifactParameters(ArtifactType type)
        {
            Type = type;
        }

        /// <summary>
        /// The type of the artifact.
        /// </summary>
        [DataMember]
        public ArtifactType Type
        {
            get;
        }

        /// <summary>
        /// The name of the artifact.
        /// </summary>
        [DataMember]
        public string Name
        {
            get;
            set;
        }
    }
}
