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

using System.Runtime.Serialization;

namespace GitHub.DistributedTask.WebApi
{
    // do NOT add new enum since it will break backward compatibility with GHES
    public enum JobTelemetryType
    {
        [EnumMember]
        General = 0,

        [EnumMember]
        ActionCommand = 1,

        [EnumMember]
        ConnectivityCheck = 2,
    }
}
