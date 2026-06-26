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
using System.Collections.Generic;

namespace GitHub.Services.OAuth
{
    /// <summary>
    /// Represents an object which participates in setting parameters for an OAuth token request.
    /// </summary>
    public interface IVssOAuthTokenParameterProvider
    {
        /// <summary>
        /// Sets applicable parameters on the provided parameters collection for a token request in which the provider
        /// is a participant.
        /// </summary>
        /// <param name="parameters">The current set of parameters</param>
        void SetParameters(IDictionary<String, String> parameters);
    }
}
