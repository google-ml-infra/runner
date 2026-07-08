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
using System.Collections.Generic;
using System.ComponentModel;
using GitHub.DistributedTask.Logging;
using GitHub.Services.Common;

namespace GitHub.DistributedTask.Expressions2.Sdk
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public sealed class EvaluationContext
    {
        internal EvaluationContext(
            ITraceWriter trace,
            ISecretMasker secretMasker,
            Object state,
            EvaluationOptions options,
            ExpressionNode node)
        {
            ArgumentUtility.CheckForNull(trace, nameof(trace));
            ArgumentUtility.CheckForNull(secretMasker, nameof(secretMasker));
            Trace = trace;
            SecretMasker = secretMasker;
            State = state;

            // Copy the options
            options = new EvaluationOptions(copy: options);
            if (options.MaxMemory == 0)
            {
                // Set a reasonable default max memory
                options.MaxMemory = 1048576; // 1 mb
            }
            Options = options;
            Memory = new EvaluationMemory(options.MaxMemory, node);

            m_traceResults = new Dictionary<ExpressionNode, String>();
            m_traceMemory = new MemoryCounter(null, options.MaxMemory);
        }

        public ITraceWriter Trace { get; }

        public ISecretMasker SecretMasker { get; }

        public Object State { get; }

        internal EvaluationMemory Memory { get; }

        internal EvaluationOptions Options { get; }

        internal void SetTraceResult(
            ExpressionNode node,
            EvaluationResult result)
        {
            // Remove if previously added. This typically should not happen. This could happen
            // due to a badly authored function. So we'll handle it and track memory correctly.
            if (m_traceResults.TryGetValue(node, out String oldValue))
            {
                m_traceMemory.Remove(oldValue);
                m_traceResults.Remove(node);
            }

            // Check max memory
            String value = ExpressionUtility.FormatValue(SecretMasker, result);
            if (m_traceMemory.TryAdd(value))
            {
                // Store the result
                m_traceResults[node] = value;
            }
        }

        internal Boolean TryGetTraceResult(ExpressionNode node, out String value)
        {
            return m_traceResults.TryGetValue(node, out value);
        }

        private readonly Dictionary<ExpressionNode, String> m_traceResults = new Dictionary<ExpressionNode, String>();
        private readonly MemoryCounter m_traceMemory;
    }
}
