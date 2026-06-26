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

namespace GitHub.Actions.Expressions.Sdk
{
    /// <summary>
    /// Helper class for ExpressionNode authors. This class helps calculate memory overhead for a result object.
    /// </summary>
    public sealed class MemoryCounter
    {
        internal MemoryCounter(
            ExpressionNode node,
            Int32? maxBytes)
        {
            m_node = node;
            m_maxBytes = (maxBytes ?? 0) > 0 ? maxBytes.Value : Int32.MaxValue;
        }

        public Int32 CurrentBytes => m_currentBytes;

        public void Add(Int32 amount)
        {
            if (!TryAdd(amount))
            {
                throw new InvalidOperationException(ExpressionResources.ExceededAllowedMemory(m_node?.ConvertToExpression()));
            }
        }

        public void Add(String value)
        {
            Add(CalculateSize(value));
        }

        public void AddMinObjectSize()
        {
            Add(MinObjectSize);
        }

        public void Remove(String value)
        {
            m_currentBytes -= CalculateSize(value);
        }

        public static Int32 CalculateSize(String value)
        {
            // This measurement doesn't have to be perfect.
            // https://codeblog.jonskeet.uk/2011/04/05/of-memory-and-strings/

            Int32 bytes;
            checked
            {
                bytes = StringBaseOverhead + ((value?.Length ?? 0) * 2);
            }
            return bytes;
        }

        internal Boolean TryAdd(Int32 amount)
        {
            try
            {
                checked
                {
                    amount += m_currentBytes;
                }

                if (amount > m_maxBytes)
                {
                    return false;
                }

                m_currentBytes = amount;
                return true;
            }
            catch (OverflowException)
            {
                return false;
            }
        }

        internal Boolean TryAdd(String value)
        {
            return TryAdd(CalculateSize(value));
        }

        internal const Int32 MinObjectSize = 24;
        internal const Int32 StringBaseOverhead = 26;
        private readonly Int32 m_maxBytes;
        private readonly ExpressionNode m_node;
        private Int32 m_currentBytes;
    }
}
