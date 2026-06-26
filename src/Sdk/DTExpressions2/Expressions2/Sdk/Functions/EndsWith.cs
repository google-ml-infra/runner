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

namespace GitHub.DistributedTask.Expressions2.Sdk.Functions
{
    internal sealed class EndsWith : Function
    {
        protected sealed override Boolean TraceFullyRealized => false;

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
                    return leftString.EndsWith(rightString, StringComparison.OrdinalIgnoreCase);
                }
            }

            return false;
        }
    }
}
