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

﻿using GitHub.Services.Common;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace GitHub.DistributedTask.Logging
{
    internal sealed class RegexSecret : ISecret
    {
        public RegexSecret(String pattern)
        {
            ArgumentUtility.CheckStringForNullOrEmpty(pattern, nameof(pattern));
            m_pattern = pattern;
            m_regex = new Regex(pattern);
        }

        public override Boolean Equals(Object obj)
        {
            var item = obj as RegexSecret;
            if (item == null)
            {
                return false;
            }
            return String.Equals(m_pattern, item.m_pattern, StringComparison.Ordinal);
        }

        public override int GetHashCode() => m_pattern.GetHashCode();

        public IEnumerable<ReplacementPosition> GetPositions(String input)
        {
            Int32 startIndex = 0;
            while (startIndex < input.Length)
            {
                var match = m_regex.Match(input, startIndex);
                if (match.Success)
                {
                    startIndex = match.Index + 1;
                    yield return new ReplacementPosition(match.Index, match.Length);
                }
                else
                {
                    yield break;
                }
            }
        }

        private readonly String m_pattern;
        private readonly Regex m_regex;
    }
}
