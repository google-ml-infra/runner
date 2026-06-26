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

#nullable disable // Consider removing in the future to minimize likelihood of NullReferenceException; refer https://learn.microsoft.com/en-us/dotnet/csharp/nullable-references

using System;
using System.Collections.Generic;

namespace GitHub.Actions.Expressions.Data
{
    public static class ExpressionDataExtensions
    {
        public static ArrayExpressionData AssertArray(
            this ExpressionData value,
            String objectDescription)
        {
            if (value is ArrayExpressionData array)
            {
                return array;
            }

            throw new ArgumentException($"Unexpected type '{value?.GetType().Name}' encountered while reading '{objectDescription}'. The type '{nameof(ArrayExpressionData)}' was expected.");
        }

        public static DictionaryExpressionData AssertDictionary(
            this ExpressionData value,
            String objectDescription)
        {
            if (value is DictionaryExpressionData dictionary)
            {
                return dictionary;
            }

            throw new ArgumentException($"Unexpected type '{value?.GetType().Name}' encountered while reading '{objectDescription}'. The type '{nameof(DictionaryExpressionData)}' was expected.");
        }

        public static StringExpressionData AssertString(
            this ExpressionData value,
            String objectDescription)
        {
            if (value is StringExpressionData str)
            {
                return str;
            }

            throw new ArgumentException($"Unexpected type '{value?.GetType().Name}' encountered while reading '{objectDescription}'. The type '{nameof(StringExpressionData)}' was expected.");
        }

        /// <summary>
        /// Returns all context data objects (depth first)
        /// </summary>
        public static IEnumerable<ExpressionData> Traverse(this ExpressionData value)
        {
            return Traverse(value, omitKeys: false);
        }

        /// <summary>
        /// Returns all context data objects (depth first)
        /// </summary>
        /// <param name="omitKeys">If true, dictionary keys are omitted</param>
        public static IEnumerable<ExpressionData> Traverse(
            this ExpressionData value,
            Boolean omitKeys)
        {
            yield return value;

            if (value is ArrayExpressionData || value is DictionaryExpressionData)
            {
                var state = new TraversalState(null, value);
                while (state != null)
                {
                    if (state.MoveNext(omitKeys))
                    {
                        value = state.Current;
                        yield return value;

                        if (value is ArrayExpressionData || value is DictionaryExpressionData)
                        {
                            state = new TraversalState(state, value);
                        }
                    }
                    else
                    {
                        state = state.Parent;
                    }
                }
            }
        }

        private sealed class TraversalState
        {
            public TraversalState(
                TraversalState parent,
                ExpressionData data)
            {
                Parent = parent;
                m_data = data;
            }

            public Boolean MoveNext(Boolean omitKeys)
            {
                switch (m_data.Type)
                {
                    case ExpressionDataType.Array:
                        var array = m_data.AssertArray("array");
                        if (++m_index < array.Count)
                        {
                            Current = array[m_index];
                            return true;
                        }
                        else
                        {
                            Current = null;
                            return false;
                        }

                    case ExpressionDataType.Dictionary:
                        var dictionary = m_data.AssertDictionary("dictionary");

                        // Return the value
                        if (m_isKey)
                        {
                            m_isKey = false;
                            Current = dictionary[m_index].Value;
                            return true;
                        }

                        if (++m_index < dictionary.Count)
                        {
                            // Skip the key, return the value
                            if (omitKeys)
                            {
                                m_isKey = false;
                                Current = dictionary[m_index].Value;
                                return true;
                            }

                            // Return the key
                            m_isKey = true;
                            Current = new StringExpressionData(dictionary[m_index].Key);
                            return true;
                        }

                        Current = null;
                        return false;

                    default:
                        throw new NotSupportedException($"Unexpected {nameof(ExpressionData)} type '{m_data.Type}'");
                }
            }

            private ExpressionData m_data;
            private Int32 m_index = -1;
            private Boolean m_isKey;
            public ExpressionData Current;
            public TraversalState Parent;
        }
    }
}
