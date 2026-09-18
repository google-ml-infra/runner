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
using System;
using System.Collections.Generic;

namespace GitHub.Runner.Worker
{
    public sealed class GitHubContext : DictionaryContextData, IEnvironmentContextData
    {
        private readonly HashSet<string> _contextEnvAllowlist = new(StringComparer.OrdinalIgnoreCase)
        {
            "action_path",
            "action_ref",
            "action_repository",
            "action",
            "actor",
            "actor_id",
            "api_url",
            "artifacts",
            "artifacts_list",
            "base_ref",
            "env",
            "event_name",
            "event_path",
            "graphql_url",
            "head_ref",
            "job",
            "output",
            "path",
            "ref_name",
            "ref_protected",
            "ref_type",
            "ref",
            "repository",
            "repository_id",
            "repository_owner",
            "repository_owner_id",
            "retention_days",
            "run_attempt",
            "run_id",
            "run_number",
            "server_url",
            "sha",
            "state",
            "step_summary",
            "triggering_actor",
            "workflow",
            "workflow_ref",
            "workflow_sha",
            "workspace"
        };

        public IEnumerable<KeyValuePair<string, string>> GetRuntimeEnvironmentVariables()
        {
            foreach (var data in this)
            {
                if (_contextEnvAllowlist.Contains(data.Key))
                {
                    if (data.Value is StringContextData value)
                    {
                        yield return new KeyValuePair<string, string>($"GITHUB_{data.Key.ToUpperInvariant()}", value);
                    }
                    else if (data.Value is BooleanContextData booleanValue)
                    {
                        yield return new KeyValuePair<string, string>($"GITHUB_{data.Key.ToUpperInvariant()}", booleanValue.ToString());
                    }
                }
            }
        }

        public GitHubContext ShallowCopy()
        {
            var copy = new GitHubContext();

            foreach (var pair in this)
            {
                copy[pair.Key] = pair.Value;
            }

            return copy;
        }
    }
}
