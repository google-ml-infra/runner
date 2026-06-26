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

using System.Collections.Generic;

namespace GitHub.Actions.WorkflowParser
{
    internal static class CollectionsExtensions
    {
        /// <summary>
        /// Adds all of the given values to this collection.
        /// Can be used with dictionaries, which implement <see cref="ICollection{T}"/> and <see cref="IEnumerable{T}"/> where T is <see cref="KeyValuePair{TKey, TValue}"/>.
        /// </summary>
        public static TCollection AddRange<T, TCollection>(this TCollection collection, IEnumerable<T> values)
            where TCollection : ICollection<T>
        {
            foreach (var value in values)
            {
                collection.Add(value);
            }

            return collection;
        }
    }
}
