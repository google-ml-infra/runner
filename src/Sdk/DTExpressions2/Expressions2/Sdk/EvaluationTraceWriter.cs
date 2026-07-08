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
using GitHub.DistributedTask.Logging;
using GitHub.Services.Common;

namespace GitHub.DistributedTask.Expressions2.Sdk
{
    internal sealed class EvaluationTraceWriter : ITraceWriter
    {
        public EvaluationTraceWriter(ITraceWriter trace, ISecretMasker secretMasker)
        {
            ArgumentUtility.CheckForNull(secretMasker, nameof(secretMasker));
            m_trace = trace;
            m_secretMasker = secretMasker;
        }

        public void Info(String message)
        {
            if (m_trace != null)
            {
                message = m_secretMasker.MaskSecrets(message);
                m_trace.Info(message);
            }
        }

        public void Verbose(String message)
        {
            if (m_trace != null)
            {
                message = m_secretMasker.MaskSecrets(message);
                m_trace.Verbose(message);
            }
        }

        private readonly ISecretMasker m_secretMasker;
        private readonly ITraceWriter m_trace;
    }
}
