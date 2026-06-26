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

﻿using System.Globalization;

namespace GitHub.DistributedTask.Expressions
{
    public static class ExpressionResources
    {
        public static string ExceededAllowedMemory(object arg0)
        {
            const string Format = @"The maximum allowed memory size was exceeded while evaluating the following expression: {0}";
            return string.Format(CultureInfo.CurrentCulture, Format, arg0);
        }

        public static string InvalidFormatArgIndex(object arg0)
        {
            const string Format = @"The following format string references more arguments than were supplied: {0}";
            return string.Format(CultureInfo.CurrentCulture, Format, arg0);
        }

        public static string InvalidFormatSpecifiers(object arg0, object arg1)
        {
            const string Format = @"The format specifiers '{0}' are not valid for objects of type '{1}'";
            return string.Format(CultureInfo.CurrentCulture, Format, arg0, arg1);
        }

        public static string InvalidFormatString(object arg0)
        {
            const string Format = @"The following format string is invalid: {0}";
            return string.Format(CultureInfo.CurrentCulture, Format, arg0);
        }
    }
}
