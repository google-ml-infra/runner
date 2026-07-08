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
using GitHub.DistributedTask.Expressions2.Sdk;

namespace GitHub.DistributedTask.ObjectTemplating
{
    /// <summary>
    /// This expression node retrieves a user-defined named-value. This is used during expression evaluation.
    /// </summary>
    internal sealed class ContextValueNode : NamedValue
    {
        protected override Object EvaluateCore(
            EvaluationContext context,
            out ResultMemory resultMemory)
        {
            resultMemory = null;
            return (context.State as TemplateContext).ExpressionValues[Name];
        }
    }
}
