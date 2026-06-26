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
using System.Runtime.Serialization;

namespace GitHub.Services.Profile
{
    [Flags]
    public enum CoreProfileAttributes
    {
        Minimal = 0x0000, // Does not contain email, avatar, display name, or marketing preferences
        Email = 0x0001,
        Avatar = 0x0002,
        DisplayName = 0x0004,
        ContactWithOffers = 0x0008,
        All = 0xFFFF,
    }
}
