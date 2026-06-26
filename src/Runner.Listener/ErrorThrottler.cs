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
using System.Threading;
using System.Threading.Tasks;
using GitHub.Runner.Common;
using GitHub.Services.Common;

namespace GitHub.Runner.Listener
{
    [ServiceLocator(Default = typeof(ErrorThrottler))]
    public interface IErrorThrottler : IRunnerService
    {
        void Reset();
        Task IncrementAndWaitAsync(CancellationToken token);
    }

    public sealed class ErrorThrottler : RunnerService, IErrorThrottler
    {
        internal static readonly TimeSpan MinBackoff = TimeSpan.FromSeconds(1);
        internal static readonly TimeSpan MaxBackoff = TimeSpan.FromMinutes(1);
        internal static readonly TimeSpan BackoffCoefficient = TimeSpan.FromSeconds(1);
        private int _count = 0;

        public void Reset()
        {
            _count = 0;
        }

        public async Task IncrementAndWaitAsync(CancellationToken token)
        {
            if (++_count <= 1)
            {
                return;
            }

            TimeSpan backoff = BackoffTimerHelper.GetExponentialBackoff(
                attempt: _count - 2, // 0-based attempt
                minBackoff: MinBackoff,
                maxBackoff: MaxBackoff,
                deltaBackoff: BackoffCoefficient);
            Trace.Warning($"Back off {backoff.TotalSeconds} seconds before next attempt. Current consecutive error count: {_count}");
            await HostContext.Delay(backoff, token);
        }
    }
}
