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
using System.ServiceProcess;
using System.Diagnostics;
using System.ComponentModel;

namespace RunnerService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static int Main(String[] args)
        {
            if (args != null && args.Length == 1 && args[0].Equals("init", StringComparison.InvariantCultureIgnoreCase))
            {
                // TODO: LOC all strings.
                if (!EventLog.Exists("Application"))
                {
                    Console.WriteLine("[ERROR] Application event log doesn't exist on current machine.");
                    return 1;
                }

                EventLog applicationLog = new EventLog("Application");
                if (applicationLog.OverflowAction == OverflowAction.DoNotOverwrite)
                {
                    Console.WriteLine("[WARNING] The retention policy for Application event log is set to \"Do not overwrite events\".");
                    Console.WriteLine("[WARNING] Make sure manually clear logs as needed, otherwise RunnerService will stop writing output to event log.");
                }

                try
                {
                    EventLog.WriteEntry(RunnerService.EventSourceName, "create event log trace source for actions-runner service", EventLogEntryType.Information, 100);
                    return 0;
                }
                catch (Win32Exception ex)
                {
                    Console.WriteLine("[ERROR] Unable to create '{0}' event source under 'Application' event log.", RunnerService.EventSourceName);
                    Console.WriteLine("[ERROR] {0}", ex.Message);
                    Console.WriteLine("[ERROR] Error Code: {0}", ex.ErrorCode);
                    return 1;
                }
            }

            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new RunnerService(args.Length > 0 ? args[0] : "ActionsRunnerService")
            };
            ServiceBase.Run(ServicesToRun);

            return 0;
        }
    }
}
