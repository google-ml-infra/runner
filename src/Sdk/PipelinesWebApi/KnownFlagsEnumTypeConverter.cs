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
using System.Globalization;

namespace GitHub.Actions.Pipelines.WebApi
{
    /// <summary>
    /// Parses known enum flags in a comma-separated string. Unknown flags are ignored. Allows for degraded compatibility without serializing enums to integer values.
    /// </summary>
    /// <remarks>
    /// Case insensitive. Both standard and EnumMemberAttribute names are parsed.
    /// json deserialization doesn't happen for query parameters :)
    /// </remarks>
    public class KnownFlagsEnumTypeConverter : EnumConverter
    {
        public KnownFlagsEnumTypeConverter(Type type)
            : base(type)
        {
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }

        /// <exception cref="FormatException">Thrown if a flag name is empty.</exception>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string stringValue)
            {
                try
                {
                    return FlagsEnum.ParseKnownFlags(EnumType, stringValue);
                }
                catch (Exception ex)
                {
                    // Matches the exception type thrown by EnumTypeConverter.
                    throw new FormatException(PipelinesWebApiResources.InvalidFlagsEnumValue(stringValue, EnumType), ex);
                }
            }
            return base.ConvertFrom(context, culture, value);
        }
    }
}
