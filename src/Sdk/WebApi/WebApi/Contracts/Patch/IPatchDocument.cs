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

﻿using System.Collections.Generic;

namespace GitHub.Services.WebApi.Patch
{
    /// <summary>
    /// The interface for the Patch Document
    /// </summary>
    /// <typeparam name="TModel">The type this patch document applies to.</typeparam>
    public interface IPatchDocument<TModel> : IPatchOperationApplied, IPatchOperationApplying
    {
        /// <summary>
        /// The patch operations.
        /// </summary>
        IEnumerable<IPatchOperation<TModel>> Operations { get; }

        /// <summary>
        /// Applies the operations to the target object.
        /// </summary>
        /// <param name="target">The object to apply the operations to.</param>
        void Apply(TModel target);
    }
}
