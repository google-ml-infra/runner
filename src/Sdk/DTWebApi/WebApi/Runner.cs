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
using Newtonsoft.Json;

namespace GitHub.DistributedTask.WebApi
{
    public class Runner
    {

        public class Authorization
        {
            /// <summary>
            /// The url to refresh tokens
            /// </summary> 
            [JsonProperty("authorization_url")]
            public Uri AuthorizationUrl
            {
                get;
                internal set;
            }

            /// <summary>
            /// The url to refresh tokens with legacy service
            /// </summary> 
            [JsonProperty("legacy_authorization_url")]
            public Uri LegacyAuthorizationUrl
            {
                get;
                internal set;
            }

            /// <summary>
            /// The url to connect to poll for messages
            /// </summary> 
            [JsonProperty("server_url")]
            public string ServerUrl
            {
                get;
                internal set;
            }

            /// <summary>
            /// The client id to use when connecting to the authorization_url
            /// </summary>
            [JsonProperty("client_id")]
            public string ClientId
            {
                get;
                internal set;
            }
        }

        [JsonProperty("name")]
        public string Name
        {
            get;
            internal set;
        }

        [JsonProperty("id")]
        public ulong Id
        {
            get;
            internal set;
        }

        [JsonProperty("authorization")]
        public Authorization RunnerAuthorization
        {
            get;
            internal set;
        }
    }
}
