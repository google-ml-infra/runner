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

#nullable disable // Consider removing in the future to minimize likelihood of NullReferenceException; refer https://learn.microsoft.com/en-us/dotnet/csharp/nullable-references

using System;
using System.Globalization;

namespace GitHub.Actions.Expressions.Sdk.Operators
{
    internal sealed class Equal : Container
    {
        protected sealed override Boolean TraceFullyExpanded => false;

        public sealed override String ConvertToExpression()
        {
            return String.Format(
                CultureInfo.InvariantCulture,
                "({0} == {1})",
                Parameters[0].ConvertToExpression(),
                Parameters[1].ConvertToExpression());
        }

        internal sealed override String ConvertToExpandedExpression(EvaluationContext context)
        {
            // Check if the result was stored
            if (context.TryGetTraceResult(this, out String result))
            {
                return result;
            }

            return String.Format(
                CultureInfo.InvariantCulture,
                "({0} == {1})",
                Parameters[0].ConvertToExpandedExpression(context),
                Parameters[1].ConvertToExpandedExpression(context));
        }

        protected sealed override Object EvaluateCore(
            EvaluationContext context,
            out ResultMemory resultMemory)
        {
            resultMemory = null;
            var left = Parameters[0].Evaluate(context);
            var right = Parameters[1].Evaluate(context);
            return left.AbstractEqual(right);
        }
    }
}
