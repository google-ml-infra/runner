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
using System.Collections.Generic;
using System.ComponentModel;

namespace GitHub.DistributedTask.Expressions2.Sdk
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public abstract class Container : ExpressionNode
    {
        public IReadOnlyList<ExpressionNode> Parameters => m_parameters.AsReadOnly();

        public void AddParameter(ExpressionNode node)
        {
            m_parameters.Add(node);
            node.Container = this;
        }

        private readonly List<ExpressionNode> m_parameters = new List<ExpressionNode>();
    }
}
