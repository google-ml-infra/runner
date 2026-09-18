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

using System;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using GitHub.DistributedTask.ObjectTemplating.Schema;

namespace GitHub.DistributedTask.Pipelines.ObjectTemplating
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class PipelineTemplateSchemaFactory
    {
        public static TemplateSchema GetSchema()
        {
            if (s_schema == null)
            {
                var assembly = Assembly.GetExecutingAssembly();
                var json = default(String);
                using (var stream = assembly.GetManifestResourceStream("GitHub.DistributedTask.Pipelines.ObjectTemplating.workflow-v1.0.json"))
                using (var streamReader = new StreamReader(stream))
                {
                    json = streamReader.ReadToEnd();
                }

                var objectReader = new JsonObjectReader(null, json);
                var schema = TemplateSchema.Load(objectReader);
                Interlocked.CompareExchange(ref s_schema, schema, null);
            }

            return s_schema;
        }

        private static TemplateSchema s_schema;
    }
}
