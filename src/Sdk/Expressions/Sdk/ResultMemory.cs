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

namespace GitHub.Actions.Expressions.Sdk
{
    public class ResultMemory
    {
        /// <summary>
        /// Only set a non-null value when both of the following conditions are met:
        /// 1) The result is a complex object. In other words, the result is
        /// not a simple type: string, boolean, number, or null.
        /// 2) The result is a newly created object.
        ///
        /// <para>
        /// For example, consider a function jsonParse() which takes a string parameter,
        /// and returns a JToken object. The JToken object is newly created and a rough
        /// measurement should be returned for the number of bytes it consumes in memory.
        /// </para>
        ///
        /// <para>
        /// For another example, consider a function which returns a sub-object from a
        /// complex parameter value. From the perspective of an individual function,
        /// the size of the complex parameter value is unknown. In this situation, set the
        /// value to IntPtr.Size.
        /// </para>
        ///
        /// <para>
        /// When you are unsure, set the value to null. Null indicates the overhead of a
        /// new pointer should be accounted for.
        /// </para>
        /// </summary>
        public Int32? Bytes { get; set; }

        /// <summary>
        /// Indicates whether <c ref="Bytes" /> represents the total size of the result.
        /// True indicates the accounting-overhead of downstream parameters can be discarded.
        ///
        /// For <c ref="EvaluationOptions.Converters" />, this value is currently ignored.
        ///
        /// <para>
        /// For example, consider a funciton jsonParse() which takes a string paramter,
        /// and returns a JToken object. The JToken object is newly created and a rough
        /// measurement should be returned for the amount of bytes it consumes in memory.
        /// Set the <c ref="IsTotal" /> to true, since new object contains no references
        /// to previously allocated memory.
        /// </para>
        ///
        /// <para>
        /// For another example, consider a function which wraps a complex parameter result.
        /// <c ref="Bytes" /> should be set to the amount of newly allocated memory.
        /// However since the object references previously allocated memory, set <c ref="IsTotal" />
        /// to false.
        /// </para>
        /// </summary>
        public Boolean IsTotal { get; set; }
    }
}
