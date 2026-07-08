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
using System.Linq;
using System.Runtime.Serialization;

namespace GitHub.Actions.Pipelines.WebApi
{
    public static class UnknownEnum
    {
        public static T Parse<T>(string stringValue)
        {
            return (T)Parse(typeof(T), stringValue);
        }

        public static object Parse(Type enumType, string stringValue)
        {
            var underlyingType = Nullable.GetUnderlyingType(enumType);
            enumType = underlyingType != null ? underlyingType : enumType;

            var names = Enum.GetNames(enumType);
            if (!string.IsNullOrEmpty(stringValue))
            {
                var match = names.FirstOrDefault(name => string.Equals(name, stringValue, StringComparison.OrdinalIgnoreCase));
                if (match != null)
                {
                    return Enum.Parse(enumType, match);
                }

                // maybe we have an enum member with an EnumMember attribute specifying a custom name
                foreach (var field in enumType.GetFields())
                {
                    var enumMemberAttribute = field.GetCustomAttributes(typeof(EnumMemberAttribute), false).FirstOrDefault() as EnumMemberAttribute;
                    if (enumMemberAttribute != null && string.Equals(enumMemberAttribute.Value, stringValue, StringComparison.OrdinalIgnoreCase))
                    {
                        // we already have the field, no need to do enum.parse on it
                        return field.GetValue(null);
                    }
                }
            }

            return Enum.Parse(enumType, UnknownName);
        }

        private const string UnknownName = "Unknown";
    }
}
