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

namespace GitHub.Services.WebApi
{
    public static class SecurityResources
    {

        public static string InvalidAclStoreException(object arg0, object arg1)
        {
            const string Format = @"The ACL store with identifier '{1}' was not found in the security namespace '{0}'.";
            return string.Format(CultureInfo.CurrentCulture, Format, arg0, arg1);
        }

        public static string InvalidPermissionsException(object arg0, object arg1)
        {
            const string Format = @"Invalid operation. Unable to set bits '{1}' in security namespace '{0}' as it is reserved by the system.";
            return string.Format(CultureInfo.CurrentCulture, Format, arg0, arg1);
        }
    }
}
