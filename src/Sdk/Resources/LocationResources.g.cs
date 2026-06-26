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

namespace GitHub.Services.WebApi
{
    public static class LocationResources
    {
        public static string ParentDefinitionNotFound(object arg0, object arg1, object arg2, object arg3)
        {
            const string Format = @"Cannot save service definition with type {0} identifier {1} because parent definition with type {2} identifier {3} could not be found.";
            return string.Format(CultureInfo.CurrentCulture, Format, arg0, arg1, arg2, arg3);
        }
    }
}
