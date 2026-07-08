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

﻿using System;
using System.ComponentModel;

namespace GitHub.Services.Common
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class BackoffTimerHelper
    {
        public static TimeSpan GetRandomBackoff(
            TimeSpan minBackoff,
            TimeSpan maxBackoff,
            TimeSpan? previousBackoff = null)
        {
            Random random = null;
            if (previousBackoff.HasValue)
            {
                random = new Random((Int32)previousBackoff.Value.TotalMilliseconds);
            }
            else
            {
                random = new Random();
            }

            return TimeSpan.FromMilliseconds(random.Next((Int32)minBackoff.TotalMilliseconds, (Int32)maxBackoff.TotalMilliseconds));
        }

        public static TimeSpan GetExponentialBackoff(
            Int32 attempt,
            TimeSpan minBackoff,
            TimeSpan maxBackoff,
            TimeSpan deltaBackoff)
        {
            Double randomBackoff = (Double)new Random().Next((Int32)(deltaBackoff.TotalMilliseconds * 0.8), (Int32)(deltaBackoff.TotalMilliseconds * 1.2));
            Double additionalBackoff = attempt < 0 ? (Math.Pow(2.0, (Double)attempt)) * randomBackoff : (Math.Pow(2.0, (Double)attempt) - 1.0) * randomBackoff;
            return TimeSpan.FromMilliseconds(Math.Min(minBackoff.TotalMilliseconds + additionalBackoff, maxBackoff.TotalMilliseconds));
        }
    }
}
