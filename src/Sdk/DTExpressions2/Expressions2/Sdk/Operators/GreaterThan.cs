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
using System.Globalization;

namespace GitHub.DistributedTask.Expressions2.Sdk.Operators
{
    internal sealed class GreaterThan : Container
    {
        protected sealed override Boolean TraceFullyRealized => false;

        internal sealed override String ConvertToExpression()
        {
            return String.Format(
                CultureInfo.InvariantCulture,
                "({0} > {1})",
                Parameters[0].ConvertToExpression(),
                Parameters[1].ConvertToExpression());
        }

        internal sealed override String ConvertToRealizedExpression(EvaluationContext context)
        {
            // Check if the result was stored
            if (context.TryGetTraceResult(this, out String result))
            {
                return result;
            }

            return String.Format(
                CultureInfo.InvariantCulture,
                "({0} > {1})",
                Parameters[0].ConvertToRealizedExpression(context),
                Parameters[1].ConvertToRealizedExpression(context));
        }

        protected sealed override Object EvaluateCore(
            EvaluationContext context,
            out ResultMemory resultMemory)
        {
            resultMemory = null;
            var left = Parameters[0].Evaluate(context);
            var right = Parameters[1].Evaluate(context);
            return left.AbstractGreaterThan(right);
        }
    }
}
