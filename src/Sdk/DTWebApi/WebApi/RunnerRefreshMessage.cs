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
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace GitHub.DistributedTask.WebApi
{
    [DataContract]
    public sealed class RunnerRefreshMessage
    {
        public static readonly String MessageType = "RunnerRefresh";

        [JsonConstructor]
        internal RunnerRefreshMessage()
        {
        }

        [DataMember(Name = "target_version")]
        public String TargetVersion
        {
            get;
            set;
        }

        [DataMember(Name = "download_url")]
        public string DownloadUrl
        {
            get;
            set;
        }

        [DataMember(Name = "sha256_checksum")]
        public string SHA256Checksum
        {
            get;
            set;
        }

        [DataMember(Name = "os")]
        public string OS
        {
            get;
            set;
        }
    }
}
