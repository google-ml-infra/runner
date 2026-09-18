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

using System.Globalization;

namespace GitHub.Actions.Pipelines.WebApi
{
    public static class PipelinesWebApiResources
    {

        public static string FlagEnumTypeRequired()
        {
            const string Format = @"Invalid type. An enum type with the Flags attribute must be supplied.";
            return Format;
        }

        public static string InvalidFlagsEnumValue(object arg0, object arg1)
        {
            const string Format = @"'{0}' is not a valid value for {1}";
            return string.Format(CultureInfo.CurrentCulture, Format, arg0, arg1);
        }

        public static string NonEmptyEnumElementsRequired(object arg0)
        {
            const string Format = @"Each comma separated enum value must be non-empty: {0}";
            return string.Format(CultureInfo.CurrentCulture, Format, arg0);
        }
    }
}
