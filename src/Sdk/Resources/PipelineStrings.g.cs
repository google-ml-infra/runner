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

﻿using System.Globalization;

namespace GitHub.DistributedTask.Pipelines
{
    public static class PipelineStrings
    {
        public static string ExpressionInvalid(object arg0)
        {
            const string Format = @"'{0}' is not a valid expression. Expressions must be enclosed with '$[' and ']'.";
            return string.Format(CultureInfo.CurrentCulture, Format, arg0);
        }

        public static string InvalidRegexOptions(object arg0, object arg1)
        {
            const string Format = @"Provider regex options '{0}' are invalid. Supported combination of flags: `{1}`. Eg: 'IgnoreCase, Multiline', 'Multiline'";
            return string.Format(CultureInfo.CurrentCulture, Format, arg0, arg1);
        }

        public static string RegexFailed(object arg0, object arg1)
        {
            const string Format = @"Regular expression failed evaluating '{0}' : {1}";
            return string.Format(CultureInfo.CurrentCulture, Format, arg0, arg1);
        }
    }
}
