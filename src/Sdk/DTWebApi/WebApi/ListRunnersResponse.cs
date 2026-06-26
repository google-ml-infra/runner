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

﻿using GitHub.Services.WebApi;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using System.Linq;

namespace GitHub.DistributedTask.WebApi
{

    public class ListRunnersResponse
    {
        public ListRunnersResponse()
        {
        }

        public ListRunnersResponse(ListRunnersResponse responseToBeCloned)
        {
            this.TotalCount = responseToBeCloned.TotalCount;
            this.Runners = responseToBeCloned.Runners;
        }

        [JsonProperty("total_count")]
        public int TotalCount
        {
            get;
            set;
        }

        [JsonProperty("runners")]
        public List<Runner> Runners
        {
            get;
            set;
        }

        public ListRunnersResponse Clone()
        {
            return new ListRunnersResponse(this);
        }

        public List<TaskAgent> ToTaskAgents()
        {
            return Runners.Select(runner => new TaskAgent() { Id = runner.Id, Name = runner.Name }).ToList();
        }
    }

}
