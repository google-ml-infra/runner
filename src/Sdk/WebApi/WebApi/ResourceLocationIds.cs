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
using GitHub.Services.Common;

namespace GitHub.Services.FileContainer
{
    public static class FileContainerResourceIds
    {
        public const string FileContainerServiceArea = "Container";
        public const string FileContainerIdString = "E4F5C81E-E250-447B-9FEF-BD48471BEA5E";
        public const string BrowseFileContainerIdString = "E71A64AC-B2B5-4230-A4C0-DAD657CF97E2";

        public static readonly Guid FileContainer = new Guid(FileContainerIdString);
        public static readonly Guid BrowseFileContainer = new Guid(BrowseFileContainerIdString);

        public const string FileContainerResource = "Containers";
    }
}

namespace GitHub.Services.Location
{
    [GenerateAllConstants]
    public static class LocationResourceIds
    {
        public const string LocationServiceArea = "Location";

        public const string ConnectionDataResource = "ConnectionData";
        public static readonly Guid ConnectionData = new Guid("{00D9565F-ED9C-4A06-9A50-00E7896CCAB4}");

        public const string ServiceDefinitionsResource = "ServiceDefinitions";
        public static readonly Guid ServiceDefinitions = new Guid("{D810A47D-F4F4-4A62-A03F-FA1860585C4C}");

        public const string AccessMappingsResource = "AccessMappings";
        public static readonly Guid AccessMappings = new Guid("{A52F2F69-B171-4E88-9DFE-34B44CF7E386}");

        public const string ResourceAreasResource = "ResourceAreas";
        public static readonly Guid ResourceAreas = new Guid("E81700F7-3BE2-46DE-8624-2EB35882FCAA");

        // Used for updating the SPS locations in account migrations.
        public const string SpsServiceDefintionResource = "SpsServiceDefinition";

        public static readonly Guid SpsServiceDefinition = new Guid("{DF5F298A-4E06-4815-A13E-6CE90A37EFA4}");
    }
}
