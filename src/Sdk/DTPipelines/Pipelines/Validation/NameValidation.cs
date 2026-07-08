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
using System.ComponentModel;
using System.Text;

namespace GitHub.DistributedTask.Pipelines.Validation
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class NameValidation
    {
        public static Boolean IsValid(
            String name,
            Boolean allowHyphens = false)
        {
            var result = true;
            for (Int32 i = 0; i < name.Length; i++)
            {
                if ((name[i] >= 'a' && name[i] <= 'z') ||
                    (name[i] >= 'A' && name[i] <= 'Z') ||
                    (name[i] >= '0' && name[i] <= '9' && i > 0) ||
                    (name[i] == '_') ||
                    (allowHyphens && name[i] == '-' && i > 0))
                {
                    continue;
                }
                else
                {
                    result = false;
                    break;
                }
            }

            return result;
        }

        public static String Sanitize(
            String name,
            Boolean allowHyphens = false)
        {
            if (name == null)
            {
                return String.Empty;
            }

            var sb = new StringBuilder();
            for (Int32 i = 0; i < name.Length; i++)
            {
                if ((name[i] >= 'a' && name[i] <= 'z') ||
                    (name[i] >= 'A' && name[i] <= 'Z') ||
                    (name[i] >= '0' && name[i] <= '9' && sb.Length > 0) ||
                    (name[i] == '_') ||
                    (allowHyphens && name[i] == '-' && sb.Length > 0))
                {
                    sb.Append(name[i]);
                }
            }
            return sb.ToString();
        }
    }
}
