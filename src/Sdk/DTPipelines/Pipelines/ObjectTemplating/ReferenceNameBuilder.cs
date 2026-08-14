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
using System.Globalization;
using System.Text;
using GitHub.DistributedTask.Pipelines.Validation;

namespace GitHub.DistributedTask.Pipelines.ObjectTemplating
{
    internal sealed class ReferenceNameBuilder
    {
        internal void AppendSegment(String value)
        {
            if (String.IsNullOrEmpty(value))
            {
                return;
            }

            if (m_name.Length == 0)
            {
                var first = value[0];
                if ((first >= 'a' && first <= 'z') ||
                    (first >= 'A' && first <= 'Z') ||
                    first == '_')
                {
                    // Legal first char
                }
                else if ((first >= '0' && first <= '9') || first == '-')
                {
                    // Illegal first char, but legal char.
                    // Prepend "_".
                    m_name.Append("_");
                }
                else
                {
                    // Illegal char
                }
            }
            else
            {
                // Separator
                m_name.Append(c_separator);
            }

            foreach (var c in value)
            {
                if ((c >= 'a' && c <= 'z') ||
                    (c >= 'A' && c <= 'Z') ||
                    (c >= '0' && c <= '9') ||
                    c == '_' ||
                    c == '-')
                {
                    // Legal
                    m_name.Append(c);
                }
                else
                {
                    // Illegal
                    m_name.Append("_");
                }
            }
        }

        internal String Build()
        {
            var original = m_name.Length > 0 ? m_name.ToString() : "job";

            var attempt = 1;
            var suffix = default(String);
            while (true)
            {
                if (attempt == 1)
                {
                    suffix = String.Empty;
                }
                else if (attempt < 1000)
                {
                    suffix = String.Format(CultureInfo.InvariantCulture, "_{0}", attempt);
                }
                else
                {
                    throw new InvalidOperationException("Unable to create a unique name");
                }

                var candidate = original.Substring(0, Math.Min(original.Length, PipelineConstants.MaxNodeNameLength - suffix.Length)) + suffix;

                if (m_distinctNames.Add(candidate))
                {
                    m_name.Clear();
                    return candidate;
                }

                attempt++;
            }
        }

        internal Boolean TryAddKnownName(
            String value,
            out String error)
        {
            if (!NameValidation.IsValid(value, allowHyphens: true) && value.Length < PipelineConstants.MaxNodeNameLength)
            {
                error = $"The identifier '{value}' is invalid. IDs may only contain alphanumeric characters, '_', and '-'. IDs must start with a letter or '_' and must be less than {PipelineConstants.MaxNodeNameLength} characters.";
                return false;
            }
            else if (!m_distinctNames.Add(value))
            {
                error = $"The identifier '{value}' may not be used more than once within the same scope.";
                return false;
            }
            else
            {
                error = null;
                return true;
            }
        }

        private const String c_separator = "_";
        private readonly HashSet<String> m_distinctNames = new HashSet<String>(StringComparer.OrdinalIgnoreCase);
        private readonly StringBuilder m_name = new StringBuilder();
    }
}
