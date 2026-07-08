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

using GitHub.DistributedTask.Pipelines.ContextData;
using GitHub.Runner.Common.Util;
using GitHub.Runner.Common;

namespace GitHub.Runner.Worker
{
    public sealed class JobContext : DictionaryContextData
    {
        public ActionResult? Status
        {
            get
            {
                if (this.TryGetValue("status", out var status) && status is StringContextData statusString)
                {
                    return EnumUtil.TryParse<ActionResult>(statusString);
                }
                else
                {
                    return null;
                }
            }
            set
            {
                this["status"] = new StringContextData(value.ToString().ToLowerInvariant());
            }
        }

        public DictionaryContextData Services
        {
            get
            {
                if (this.TryGetValue("services", out var services) && services is DictionaryContextData servicesDictionary)
                {
                    return servicesDictionary;
                }
                else
                {
                    this["services"] = new DictionaryContextData();
                    return this["services"] as DictionaryContextData;
                }
            }
        }

        public DictionaryContextData Container
        {
            get
            {
                if (this.TryGetValue("container", out var container) && container is DictionaryContextData containerDictionary)
                {
                    return containerDictionary;
                }
                else
                {
                    this["container"] = new DictionaryContextData();
                    return this["container"] as DictionaryContextData;
                }
            }
        }

        public double? CheckRunId
        {
            get
            {
                if (this.TryGetValue("check_run_id", out var value) && value is NumberContextData number)
                {
                    return number.Value;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                if (value.HasValue)
                {
                    this["check_run_id"] = new NumberContextData(value.Value);
                }
                else
                {
                    this["check_run_id"] = null;
                }
            }
        }

        public string WorkflowRef
        {
            get
            {
                if (this.TryGetValue("workflow_ref", out var value) && value is StringContextData str)
                {
                    return str.Value;
                }
                return null;
            }
            set
            {
                this["workflow_ref"] = value != null ? new StringContextData(value) : null;
            }
        }

        public string WorkflowSha
        {
            get
            {
                if (this.TryGetValue("workflow_sha", out var value) && value is StringContextData str)
                {
                    return str.Value;
                }
                return null;
            }
            set
            {
                this["workflow_sha"] = value != null ? new StringContextData(value) : null;
            }
        }

        public string WorkflowRepository
        {
            get
            {
                if (this.TryGetValue("workflow_repository", out var value) && value is StringContextData str)
                {
                    return str.Value;
                }
                return null;
            }
            set
            {
                this["workflow_repository"] = value != null ? new StringContextData(value) : null;
            }
        }

        public string WorkflowFilePath
        {
            get
            {
                if (this.TryGetValue("workflow_file_path", out var value) && value is StringContextData str)
                {
                    return str.Value;
                }
                return null;
            }
            set
            {
                this["workflow_file_path"] = value != null ? new StringContextData(value) : null;
            }
        }
    }
}
