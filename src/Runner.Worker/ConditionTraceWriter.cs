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
using System.Text;
using GitHub.Runner.Common;
using GitHub.Runner.Sdk;
using ObjectTemplating = GitHub.DistributedTask.ObjectTemplating;

namespace GitHub.Runner.Worker
{
    public sealed class ConditionTraceWriter : ObjectTemplating::ITraceWriter
    {
        private readonly IExecutionContext _executionContext;
        private readonly Tracing _trace;
        private readonly StringBuilder _traceBuilder = new();

        public string Trace => _traceBuilder.ToString();

        public ConditionTraceWriter(Tracing trace, IExecutionContext executionContext)
        {
            ArgUtil.NotNull(trace, nameof(trace));
            _trace = trace;
            _executionContext = executionContext;
        }

        public void Error(string format, params Object[] args)
        {
            var message = StringUtil.Format(format, args);
            _trace.Error(message);
            _executionContext?.Debug(message);
        }

        public void Info(string format, params Object[] args)
        {
            var message = StringUtil.Format(format, args);
            _trace.Info(message);
            _executionContext?.Debug(message);
            _traceBuilder.AppendLine(message);
        }

        public void Verbose(string format, params Object[] args)
        {
            var message = StringUtil.Format(format, args);
            _trace.Verbose(message);
            _executionContext?.Debug(message);
        }
    }
}
