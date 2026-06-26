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

﻿using System.Runtime.Serialization;

namespace Sdk.RSWebApi.Contracts
{
    [DataContract]
    public struct Annotation
    {
        [DataMember(Name = "level", EmitDefaultValue = false)]
        public AnnotationLevel Level;

        [DataMember(Name = "message", EmitDefaultValue = false)]
        public string Message;

        [DataMember(Name = "title", EmitDefaultValue = false)]
        public string Title;

        [DataMember(Name = "rawDetails", EmitDefaultValue = false)]
        public string RawDetails;

        [DataMember(Name = "path", EmitDefaultValue = false)]
        public string Path;

        [DataMember(Name = "isInfrastructureIssue", EmitDefaultValue = false)]
        public bool IsInfrastructureIssue;

        [DataMember(Name = "startLine", EmitDefaultValue = false)]
        public long StartLine;

        [DataMember(Name = "endLine", EmitDefaultValue = false)]
        public long EndLine;

        [DataMember(Name = "startColumn", EmitDefaultValue = false)]
        public long StartColumn;

        [DataMember(Name = "endColumn", EmitDefaultValue = false)]
        public long EndColumn;

        [DataMember(Name = "stepNumber", EmitDefaultValue = false)]
        public long StepNumber;
    }
}
