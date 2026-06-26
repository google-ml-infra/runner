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

namespace GitHub.Services.WebApi
{
    /// <summary>Define event log id ranges</summary>
    /// This corresponds with values in Framework\Server\Common\EventLog.cs
    public static class VssEventId
    {
        public static readonly int DefaultEventId = 0;

        // Errors
        public static readonly int ExceptionBaseEventId = 3000;

        private static readonly int EtmBaseEventId = ExceptionBaseEventId + 1200; // 4200
        public static readonly int VssIdentityServiceException = EtmBaseEventId + 7;
        public static readonly int AccountException = EtmBaseEventId + 36;

        //File Container Service range
        public static readonly int FileContainerBaseEventId = ExceptionBaseEventId + 1700; // 4700 
    }
}
