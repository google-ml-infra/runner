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
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace GitHub.DistributedTask.WebApi
{
    [DataContract]
    public class ServiceConnectivityCheckInput
    {
        [JsonConstructor]
        public ServiceConnectivityCheckInput()
        {
            Endpoints = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        }

        [DataMember(EmitDefaultValue = false)]
        public Dictionary<string, string> Endpoints { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int IntervalInSecond { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int RequestTimeoutInSecond { get; set; }
    }

    [DataContract]
    public class ServiceConnectivityCheckResult
    {
        [JsonConstructor]
        public ServiceConnectivityCheckResult()
        {
            EndpointsResult = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);
        }

        [DataMember(Order = 1, EmitDefaultValue = true)]
        public bool HasFailure { get; set; }

        [DataMember(Order = 2, EmitDefaultValue = false)]
        public Dictionary<string, List<string>> EndpointsResult { get; set; }
    }
}
