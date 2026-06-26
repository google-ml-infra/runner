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
using System.ComponentModel;

namespace GitHub.Services.WebApi
{
    /// <summary>
    /// Any responses from public APIs must implement this interface. It is used to enforce that 
    /// the data being returned has been security checked.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface ISecuredObject
    {
        /// <summary>
        /// The id of the namespace which secures this resource.
        /// </summary>
        Guid NamespaceId
        {
            get;
        }

        /// <summary>
        /// The security bit to demand.
        /// </summary>
        Int32 RequiredPermissions
        {
            get;
        }

        /// <summary>
        /// The token to secure this resource.
        /// </summary>
        String GetToken();
    }

    /// <summary>
    /// Containers of ISecuredObjects should implement this interface. If you implement this interface, all
    /// serializable properties must be of type ISecuredObject or IEnumerable of ISecuredObject. This will
    /// be enforced using a roslyn analyzer.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface ISecuredObjectContainer { }
}
