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

﻿#nullable disable // Consider removing in the future to minimize likelihood of NullReferenceException; refer https://learn.microsoft.com/en-us/dotnet/csharp/nullable-references

using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace GitHub.Actions.WorkflowParser
{
    // Actions service should not use this class at all.
    [DataContract]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public sealed class FileInfo
    {
        [JsonConstructor]
        public FileInfo()
        {
        }

        private FileInfo(FileInfo infoToClone)
        {
            this.Path = infoToClone.Path;
            this.NWO = infoToClone.NWO;
            this.ResolvedRef = infoToClone.ResolvedRef;
            this.ResolvedSha = infoToClone.ResolvedSha;
            this.IsTrusted = infoToClone.IsTrusted;
            this.IsRequired = infoToClone.IsRequired;
        }

        [DataMember(Name = "path", EmitDefaultValue = false)]
        public string Path { get; set; }

        [DataMember(Name = "nwo", EmitDefaultValue = false)]
        public string NWO { get; set; }

        [DataMember(Name = "resolved_ref", EmitDefaultValue = false)]
        public string ResolvedRef { get; set; }

        [DataMember(Name = "resolved_sha", EmitDefaultValue = false)]
        public string ResolvedSha { get; set; }

        [DataMember(Name = "is_trusted", EmitDefaultValue = false)]
        public bool IsTrusted { get; set; }

        [DataMember(Name = "is_required", EmitDefaultValue = false)]
        public bool IsRequired { get; set; }

        public FileInfo Clone()
        {
            return new FileInfo(this);
        }
    }
}
