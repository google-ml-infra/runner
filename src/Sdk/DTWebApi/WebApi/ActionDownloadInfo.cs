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
using System.Runtime.Serialization;

namespace GitHub.DistributedTask.WebApi
{
    [DataContract]
    public class ActionDownloadInfo
    {
        [DataMember(EmitDefaultValue = false)]
        public ActionDownloadAuthentication Authentication { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public ActionDownloadPackageDetails PackageDetails { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string NameWithOwner { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string ResolvedNameWithOwner { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string ResolvedSha { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string TarballUrl { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Ref { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string ZipballUrl { get; set; }
    }

    [DataContract]
    public class ActionDownloadAuthentication
    {
        [DataMember(EmitDefaultValue = false)]
        public DateTime ExpiresAt { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Token { get; set; }
    }

    [DataContract]
    public class ActionDownloadPackageDetails 
    {
        [DataMember(EmitDefaultValue = false)]
        public string Version { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string ManifestDigest { get; set; }
    }
}
