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

namespace GitHub.Services.WebApi.Patch
{
    /// <summary>
    /// Event for when a patch operation is about to be applied
    /// </summary>
    public interface IPatchOperationApplying
    {
        event PatchOperationApplyingEventHandler PatchOperationApplying;
    }

    /// <summary>
    /// Event handler for patch operation applying.
    /// </summary>
    public delegate void PatchOperationApplyingEventHandler(object sender, PatchOperationApplyingEventArgs e);
}
