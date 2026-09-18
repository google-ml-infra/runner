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
using GitHub.Services.Common;

namespace GitHub.DistributedTask.Logging
{
    internal sealed class ValueSecret : ISecret
    {
        public ValueSecret(String value)
        {
            ArgumentUtility.CheckStringForNullOrEmpty(value, nameof(value));
            m_value = value;
        }

        public override Boolean Equals(Object obj)
        {
            var item = obj as ValueSecret;
            if (item == null)
            {
                return false;
            }
            return String.Equals(m_value, item.m_value, StringComparison.Ordinal);
        }

        public override Int32 GetHashCode() => m_value.GetHashCode();

        public IEnumerable<ReplacementPosition> GetPositions(String input)
        {
            if (!String.IsNullOrEmpty(input) && !String.IsNullOrEmpty(m_value))
            {
                Int32 startIndex = 0;
                while (startIndex > -1 &&
                    startIndex < input.Length &&
                    input.Length - startIndex >= m_value.Length) // remaining substring longer than secret value
                {
                    startIndex = input.IndexOf(m_value, startIndex, StringComparison.Ordinal);
                    if (startIndex > -1)
                    {
                        yield return new ReplacementPosition(startIndex, m_value.Length);
                        ++startIndex;
                    }
                }
            }
        }

        internal readonly String m_value;
    }
}
