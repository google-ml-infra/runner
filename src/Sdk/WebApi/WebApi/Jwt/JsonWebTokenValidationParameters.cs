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

﻿using System.Collections.Generic;
using System.Security.Claims;

namespace GitHub.Services.WebApi.Jwt
{
    public sealed class JsonWebTokenValidationParameters
    {
        public JsonWebTokenValidationParameters()
        {
            ValidateActor = false;
            ValidateAudience = true;
            ValidateIssuer = true;
            ValidateExpiration = true;
            ValidateNotBefore = false;
            ValidateSignature = true;
            ClockSkewInSeconds = 300;
            IdentityNameClaimType = ClaimTypes.NameIdentifier;
        }

        public bool ValidateActor
        {
            get;
            set;
        }

        public bool ValidateAudience
        {
            get;
            set;
        }

        public bool ValidateIssuer
        {
            get;
            set;
        }

        public bool ValidateExpiration
        {
            get;
            set;
        }

        public bool ValidateNotBefore
        {
            get;
            set;
        }

        public bool ValidateSignature
        {
            get;
            set;
        }

        public JsonWebTokenValidationParameters ActorValidationParameters
        {
            get;
            set;
        }

        public IEnumerable<string> AllowedAudiences
        {
            get;
            set;
        }

        public int ClockSkewInSeconds
        {
            get;
            set;
        }

        public VssSigningCredentials SigningCredentials
        {
            get;
            set;
        }

        public IEnumerable<string> ValidIssuers
        {
            get;
            set;
        }

        public string IdentityNameClaimType
        {
            get;
            set;
        }
    }
}
