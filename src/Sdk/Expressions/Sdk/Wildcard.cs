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

#nullable disable // Consider removing in the future to minimize likelihood of NullReferenceException; refer https://learn.microsoft.com/en-us/dotnet/csharp/nullable-references

using System;

namespace GitHub.Actions.Expressions.Sdk
{
    public sealed class Wildcard : ExpressionNode
    {
        // Prevent the value from being stored on the evaluation context.
        // This avoids unneccessarily duplicating the value in memory.
        protected sealed override Boolean TraceFullyExpanded => false;

        public sealed override String ConvertToExpression()
        {
            return ExpressionConstants.Wildcard.ToString();
        }

        internal sealed override String ConvertToExpandedExpression(EvaluationContext context)
        {
            return ExpressionConstants.Wildcard.ToString();
        }

        protected sealed override Object EvaluateCore(
            EvaluationContext context,
            out ResultMemory resultMemory)
        {
            resultMemory = null;
            return ExpressionConstants.Wildcard.ToString();
        }
    }

}
