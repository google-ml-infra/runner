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

namespace GitHub.Services.Common
{
    public static class PerformanceTimerConstants
    {
        public const string Header = "X-VSS-PerfData";
        public const string PerfTimingKey = "PerformanceTimings";

        [Obsolete]
        public const string Aad = "AAD"; // Previous timer, broken into Token and Graph below

        public const string AadToken = "AadToken";
        public const string AadGraph = "AadGraph";
        public const string BlobStorage = "BlobStorage";
        public const string FinalSqlCommand = "FinalSQLCommand";
        public const string Redis = "Redis";
        public const string ServiceBus = "ServiceBus";
        public const string Sql = "SQL";
        public const string SqlReadOnly = "SQLReadOnly";
        public const string SqlRetries = "SQLRetries";
        public const string TableStorage = "TableStorage";
        public const string VssClient = "VssClient";
        public const string DocumentDB = "DocumentDB";
    }
}
