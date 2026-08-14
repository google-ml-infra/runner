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

namespace GitHub.Services.WebApi.Patch.Json
{
    /// <summary>
    /// The JSON model for a JSON Patch operation
    /// </summary>
    [DataContract]
    public class JsonPatchOperation
    {
        /// <summary>
        /// The patch operation
        /// </summary>
        [DataMember(Name = "op", IsRequired = true)]
        public Operation Operation { get; set; }

        /// <summary>
        /// The path for the operation.
        /// In the case of an array, a zero based index can be used to specify the position in the array (e.g. /biscuits/0/name). The "-" character can be used instead of an index to insert at the end of the array (e.g. /biscuits/-).
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Path { get; set; }

        /// <summary>
        /// The path to copy from for the Move/Copy operation.
        /// </summary>
        /// <returns></returns>
        [DataMember]
        public string From { get; set; }

        /// <summary>
        /// The value for the operation.  
        /// This is either a primitive or a JToken.
        /// </summary>
        [DataMember]
        public object Value { get; set; }
    }
}
