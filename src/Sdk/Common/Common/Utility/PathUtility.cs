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

﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitHub.Services.Common
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal static class PathUtility
    {
        /// <summary>
        /// Replacement for Path.Combine.
        /// For URL please use UrlUtility.CombineUrl
        /// </summary>
        /// <param name="path1">The first half of the path.</param>
        /// <param name="path2">The second half of the path.</param>
        /// <returns>The concatenated string with and leading slashes or 
        /// tildes removed from the second string.</returns>
        public static String Combine(String path1, String path2)
        {
            if (String.IsNullOrEmpty(path1))
            {
                return path2;
            }

            if (String.IsNullOrEmpty(path2))
            {
                return path1;
            }

            Char separator = path1.Contains("/") ? '/' : '\\';

            Char[] trimChars = new Char[] { '\\', '/' };

            return path1.TrimEnd(trimChars) + separator.ToString() + path2.TrimStart(trimChars);
        }
    }
}
