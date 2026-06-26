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

namespace GitHub.Actions.Pipelines.WebApi.Contracts
{
    public class ArtifactJsonConverter : ArtifactBaseJsonConverter<Artifact>
    {
        protected override Artifact Create(Type objectType)
        {
            if (objectType == typeof(ActionsStorageArtifact))
            {
                return new ActionsStorageArtifact();
            }
            else
            {
                return null;
            }
        }

        protected override Artifact Create(ArtifactType type)
        {
            if (type == ArtifactType.Actions_Storage)
            {
                return new ActionsStorageArtifact();
            }

            return null;
        }
    }
}
