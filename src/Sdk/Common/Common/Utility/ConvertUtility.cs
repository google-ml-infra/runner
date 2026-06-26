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

﻿using System;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;

namespace GitHub.Services.Common
{
    /// <summary>
    /// Utility class for wrapping Convert.ChangeType to handle nullable values.
    /// </summary>
    public class ConvertUtility
    {
        public static object ChangeType(object value, Type type)
        {
            return ChangeType(value, type, CultureInfo.CurrentCulture);
        }

        public static object ChangeType(object value, Type type, IFormatProvider provider)
        {
            if (type.IsOfType(typeof(Nullable<>)))
            {
                var nullableConverter = new NullableConverter(type);
                return nullableConverter.ConvertTo(value, nullableConverter.UnderlyingType);
            }

            return Convert.ChangeType(value, type, provider);
        }
    }
}
