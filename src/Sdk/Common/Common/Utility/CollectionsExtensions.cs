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

﻿using System.Collections.Generic;

namespace GitHub.Services.Common
{
    public static class CollectionsExtensions
    {
        /// <summary>
        /// Adds all of the given values to this collection.
        /// Can be used with dictionaries, which implement <see cref="ICollection{T}"/> and <see cref="IEnumerable{T}"/> where T is <see cref="KeyValuePair{TKey, TValue}"/>.
        /// For dictionaries, also see <see cref="DictionaryExtensions.SetRange{K, V, TDictionary}(TDictionary, IEnumerable{KeyValuePair{K, V}})"/>
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

        /// <summary>
        /// Adds all of the given values to this collection if and only if the values object is not null.
        /// See <see cref="AddRange{T, TCollection}(TCollection, IEnumerable{T})"/> for more details.
        /// </summary>
        public static TCollection AddRangeIfRangeNotNull<T, TCollection>(this TCollection collection, IEnumerable<T> values)
            where TCollection : ICollection<T>
        {
            if (values != null)
            {
                collection.AddRange(values);
            }

            return collection;
        }
    }
}
