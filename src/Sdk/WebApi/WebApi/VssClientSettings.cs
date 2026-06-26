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

﻿using System.IO;
using GitHub.Services.Common;
using GitHub.Services.Common.ClientStorage;

namespace GitHub.Services.WebApi
{
    /// <summary>
    /// Helper for retrieving client settings which are environment-specific or retrieved from the Windows Registry
    /// </summary>
    internal static class VssClientSettings
    {
        /// <summary>
        /// Directory containing the client cache files which resides below the settings directory.
        /// 
        /// This will look something like this:
        /// C:\Documents and Settings\username\Local Settings\Application Data\GitHub\ActionsService\[GeneratedVersionInfo.ActionsProductVersion]\Cache
        /// </summary>
        internal static string ClientCacheDirectory
        {
            get
            {
                return Path.Combine(ClientSettingsDirectory, "Cache");
            }
        }

        /// <summary>
        /// Directory containing the client settings files.
        /// 
        /// This will look something like this:
        /// C:\Documents and Settings\username\Local Settings\Application Data\GitHub\ActionsService\[GeneratedVersionInfo.ActionsProductVersion]
        /// </summary>
        internal static string ClientSettingsDirectory
        {
            get
            {
                // We purposely do not cache this value. This value needs to change if 
                // Windows Impersonation is being used.
                return Path.Combine(VssFileStorage.ClientSettingsDirectory, GeneratedVersionInfo.ActionsProductVersion);
            }
        }
    }
}
