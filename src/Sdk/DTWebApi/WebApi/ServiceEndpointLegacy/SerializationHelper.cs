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
using System.Collections.Generic;

namespace GitHub.DistributedTask.WebApi
{
    public static class SerializationHelper
    {
        public static void Copy<T>(
            ref List<T> source,
            ref List<T> target,
            Boolean clearSource = false)
        {
            if (source != null && source.Count > 0)
            {
                target = new List<T>(source);
            }

            if (clearSource)
            {
                source = null;
            }
        }

        public static void Copy<TKey, TValue>(
            ref IDictionary<TKey, TValue> source,
            ref IDictionary<TKey, TValue> target,
            Boolean clearSource = false)
        {
            Copy(ref source, ref target, EqualityComparer<TKey>.Default, clearSource);
        }

        public static void Copy<TKey, TValue>(
            ref IDictionary<TKey, TValue> source,
            ref IDictionary<TKey, TValue> target,
            IEqualityComparer<TKey> comparer,
            Boolean clearSource = false)
        {
            if (source != null && source.Count > 0)
            {
                target = new Dictionary<TKey, TValue>(source, comparer);
            }

            if (clearSource)
            {
                source = null;
            }
        }
    }
}
