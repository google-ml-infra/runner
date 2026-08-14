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
using System.Runtime.Serialization;

namespace GitHub.DistributedTask.WebApi
{
    /// <summary>
    /// Message that tells the runner to redirect itself to BrokerListener for messages.
    /// (Note that we use a special Message instead of a simple 302. This is because 
    /// the runner will need to apply the runner's token to the request, and it is
    /// a security best practice to *not* blindly add sensitive data to redirects
    /// 302s.)
    /// </summary>
    [DataContract]
    public class BrokerMigrationMessage
    {
        public static readonly string MessageType = "BrokerMigration";

        public BrokerMigrationMessage()
        {
        }

        public BrokerMigrationMessage(
            Uri brokerUrl)
        {
            this.BrokerBaseUrl = brokerUrl;
        }

        /// <summary>
        /// The base url for the broker listener
        /// </summary>
        [DataMember]
        public Uri BrokerBaseUrl
        {
            get;
            internal set;
        }
    }
}
