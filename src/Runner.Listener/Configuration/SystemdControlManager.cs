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

﻿#if OS_LINUX
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using GitHub.Runner.Common.Util;
using GitHub.Runner.Common;

namespace GitHub.Runner.Listener.Configuration
{
    public class SystemDControlManager : ServiceControlManager, ILinuxServiceControlManager
    {
        // This is the name you would see when you do `systemctl list-units | grep runner`
        private const string _svcNamePattern = "actions.runner.{0}.{1}.service";
        private const string _svcDisplayPattern = "GitHub Actions Runner ({0}.{1})";
        private const string _shTemplate = "systemd.svc.sh.template";
        private const string _shName = "svc.sh";

        public void GenerateScripts(RunnerSettings settings)
        {
            try
            {
                string serviceName;
                string serviceDisplayName;
                CalculateServiceName(settings, _svcNamePattern, _svcDisplayPattern, out serviceName, out serviceDisplayName);

                string svcShPath = Path.Combine(HostContext.GetDirectory(WellKnownDirectory.Root), _shName);

                string svcShContent = File.ReadAllText(Path.Combine(HostContext.GetDirectory(WellKnownDirectory.Bin), _shTemplate));
                var tokensToReplace = new Dictionary<string, string>
                                          {
                                              { "{{SvcDescription}}", serviceDisplayName },
                                              { "{{SvcNameVar}}", serviceName }
                                          };

                svcShContent = tokensToReplace.Aggregate(
                    svcShContent,
                    (current, item) => current.Replace(item.Key, item.Value));

                File.WriteAllText(svcShPath, svcShContent, new UTF8Encoding(false));

                var unixUtil = HostContext.CreateService<IUnixUtil>();
                unixUtil.ChmodAsync("755", svcShPath).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                Trace.Error(ex);
                throw;
            }
        }
    }
}
#endif
