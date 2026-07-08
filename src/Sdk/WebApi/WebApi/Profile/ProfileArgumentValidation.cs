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

namespace GitHub.Services.Profile
{
    public static class ProfileArgumentValidation
    {
        public static void ValidateAttributeName(string attributeName)
        {
            ArgumentUtility.CheckStringForNullOrEmpty(attributeName, "attributeName", true);
            ArgumentUtility.CheckStringForInvalidCharacters(attributeName, "attributeName");
            if (attributeName.Contains(Semicolon))
            {
                throw new ArgumentException("Attribute name cannot contain the character ';'", attributeName);
            }
        }

        public static void ValidateContainerName(string containerName)
        {
            ArgumentUtility.CheckStringForNullOrEmpty(containerName, "containerName", true);
            ArgumentUtility.CheckStringForInvalidCharacters(containerName, "containerName");
            if (containerName.Contains(Semicolon))
            {
                throw new ArgumentException("Container name cannot contain the character ';'", containerName);
            }
        }

        public static void ValidateApplicationContainerName(string containerName)
        {
            ValidateContainerName(containerName);
            if (VssStringComparer.AttributesDescriptor.Compare(containerName, Profile.CoreContainerName) == 0)
            {
                throw new ArgumentException(
                    string.Format("The container name '{0}' is reserved. Please specify a valid application container name", Profile.CoreContainerName), "containerName");
            }
        }

        private const string Semicolon = ";";
    }
}
