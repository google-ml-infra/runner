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

﻿using System;
using System.Runtime.Serialization;
using GitHub.Services.WebApi;
using Newtonsoft.Json;

namespace GitHub.DistributedTask.WebApi
{
    [DataContract]
    public sealed class RunnerRefreshConfigMessage
    {
        public static readonly String MessageType = "RunnerRefreshConfig";

        [JsonConstructor]
        internal RunnerRefreshConfigMessage()
        {
        }

        public RunnerRefreshConfigMessage(
            string runnerQualifiedId,
            string configType,
            string serviceType,
            string configRefreshUrl)
        {
            this.RunnerQualifiedId = runnerQualifiedId;
            this.ConfigType = configType;
            this.ServiceType = serviceType;
            this.ConfigRefreshUrl = configRefreshUrl;
        }

        [DataMember(Name = "runnerQualifiedId")]
        public String RunnerQualifiedId
        {
            get;
            private set;
        }

        [DataMember(Name = "configType")]
        public String ConfigType
        {
            get;
            private set;
        }

        [DataMember(Name = "serviceType")]
        public String ServiceType
        {
            get;
            private set;
        }

        [DataMember(Name = "configRefreshURL")]
        public String ConfigRefreshUrl
        {
            get;
            private set;
        }
    }
}
