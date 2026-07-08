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

﻿using GitHub.Services.Common;
using System;

namespace GitHub.DistributedTask.WebApi
{
    public static class VersionParser
    {
        public static void ParseVersion(
            String version,
            out Int32 major,
            out Int32 minor,
            out Int32 patch,
            out String semanticVersion)
        {
            ArgumentUtility.CheckStringForNullOrEmpty(version, "version");

            String[] segments = version.Split(new char[] { '.', '-' }, StringSplitOptions.None);
            if (segments.Length < 3 || segments.Length > 4)
            {
                throw new ArgumentException("wrong number of segments");
            }

            if (!Int32.TryParse(segments[0], out major))
            {
                throw new ArgumentException("major");
            }

            if (!Int32.TryParse(segments[1], out minor))
            {
                throw new ArgumentException("minor");
            }

            if (!Int32.TryParse(segments[2], out patch))
            {
                throw new ArgumentException("patch");
            }

            semanticVersion = null;
            if (segments.Length == 4)
            {
                semanticVersion = segments[3];
            }
        }
    }
}
