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

using System;

namespace GitHub.Actions.WorkflowParser.Conversion
{
    internal static class PermissionLevelExtensions
    {
        public static bool IsLessThanOrEqualTo(
            this PermissionLevel permissionLevel, 
            PermissionLevel other)
        {
            switch (permissionLevel, other)
            {
                case (PermissionLevel.NoAccess, PermissionLevel.NoAccess):
                case (PermissionLevel.NoAccess, PermissionLevel.Read):
                case (PermissionLevel.NoAccess, PermissionLevel.Write):
                case (PermissionLevel.Read, PermissionLevel.Read):
                case (PermissionLevel.Read, PermissionLevel.Write):
                case (PermissionLevel.Write, PermissionLevel.Write): 
                    return true;
                case (PermissionLevel.Read, PermissionLevel.NoAccess):
                case (PermissionLevel.Write, PermissionLevel.NoAccess):
                case (PermissionLevel.Write, PermissionLevel.Read):
                    return false;
                default:
                    throw new ArgumentException($"Invalid enum comparison: {permissionLevel} and {other}");
            }
        }

        public static string ConvertToString(this PermissionLevel permissionLevel)
        {
            switch (permissionLevel)
            {
                case PermissionLevel.NoAccess:
                    return "none";
                case PermissionLevel.Read:
                    return "read";
                case PermissionLevel.Write:
                    return "write";
                default:
                    throw new NotSupportedException($"invalid permission level found. {permissionLevel}");
            }
        }
    }
}