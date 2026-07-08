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

﻿using GitHub.DistributedTask.Pipelines;

namespace GitHub.Runner.Worker.Dap
{
    /// <summary>
    /// Consolidated runtime configuration for the job debugger.
    /// Populated once from the acquire response and owned by <see cref="GlobalContext"/>.
    /// </summary>
    public sealed class DebuggerConfig
    {
        public DebuggerConfig(bool enabled, DebuggerTunnelInfo tunnel, bool overrideWelcomeMessage = false, string welcomeMessage = null)
        {
            Enabled = enabled;
            Tunnel = tunnel;
            OverrideWelcomeMessage = overrideWelcomeMessage;
            WelcomeMessage = welcomeMessage;
        }

        /// <summary>Whether the debugger is enabled for this job.</summary>
        public bool Enabled { get; }

        /// <summary>
        /// Dev Tunnel details for remote debugging.
        /// Required when <see cref="Enabled"/> is true.
        /// </summary>
        public DebuggerTunnelInfo Tunnel { get; }

        /// <summary>
        /// When true, the runner overrides the default welcome message with
        /// <see cref="WelcomeMessage"/>. A null or empty <see cref="WelcomeMessage"/>
        /// suppresses the message entirely. When false, the default help text is shown.
        /// </summary>
        public bool OverrideWelcomeMessage { get; }

        /// <summary>
        /// Optional welcome message content for the debugger console. Only used when
        /// <see cref="OverrideWelcomeMessage"/> is true.
        /// </summary>
        public string WelcomeMessage { get; }

        /// <summary>Whether the tunnel configuration is complete and valid.</summary>
        public bool HasValidTunnel => Tunnel != null
            && !string.IsNullOrEmpty(Tunnel.TunnelId)
            && !string.IsNullOrEmpty(Tunnel.ClusterId)
            && !string.IsNullOrEmpty(Tunnel.HostToken)
            && Tunnel.Port >= 1024 && Tunnel.Port <= 65535;
    }
}
