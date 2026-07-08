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

namespace GitHub.Actions.Expressions.Sdk.Functions
{
    internal sealed class Contains : Function
    {
        protected sealed override Boolean TraceFullyExpanded => false;

        protected sealed override Object EvaluateCore(
            EvaluationContext context,
            out ResultMemory resultMemory)
        {
            resultMemory = null;
            var left = Parameters[0].Evaluate(context);
            if (left.IsPrimitive)
            {
                var leftString = left.ConvertToString();

                var right = Parameters[1].Evaluate(context);
                if (right.IsPrimitive)
                {
                    var rightString = right.ConvertToString();
                    return leftString.IndexOf(rightString, StringComparison.OrdinalIgnoreCase) >= 0;
                }
            }
            else if (left.TryGetCollectionInterface(out var collection) &&
                collection is IReadOnlyArray array &&
                array.Count > 0)
            {
                var right = Parameters[1].Evaluate(context);
                foreach (var item in array)
                {
                    var itemResult = EvaluationResult.CreateIntermediateResult(context, item);
                    if (right.AbstractEqual(itemResult))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
