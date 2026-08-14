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
using System.ComponentModel;

namespace GitHub.Services.Common
{
    /// <summary>
    /// Matches Exception Types to back compatible TypeName and TypeKey for the specified range
    /// of REST Api versions.  This allows the current server to send back compatible typename
    /// and type key json when talking to older clients.  It also allows current clients to translate
    /// exceptions returned from older servers to a current client's exception type.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class ExceptionMappingAttribute : Attribute
    {
        /// <summary>
        /// Matches Exception Types to back compatible TypeName and TypeKey for the specified range
        /// of REST Api versions.  This allows the current server to send back compatible typename
        /// and type key json when talking to older clients.  It also allows current clients to translate
        /// exceptions returned from older servers to a current client's exception type.
        /// </summary>
        /// <param name="minApiVersion">The inclusive minimum REST Api version for this mapping.</param>
        /// <param name="exclusiveMaxApiVersion">The exclusive maximum REST Api version for this mapping.</param>
        /// <param name="typeKey">The original typekey to be returned by the server when processing requests within the REST Api range specified.</param>
        /// <param name="typeName">The original typeName to be returned by the server when processing requests within the REST Api range specified.</param>
        public ExceptionMappingAttribute(string minApiVersion, string exclusiveMaxApiVersion, string typeKey, string typeName)
        {
            MinApiVersion = new Version(minApiVersion);
            ExclusiveMaxApiVersion = new Version(exclusiveMaxApiVersion);
            TypeKey = typeKey;
            TypeName = typeName;
        }

        /// <summary>
        /// The inclusive minimum REST Api version for this mapping.
        /// </summary>
        public Version MinApiVersion { get; private set; }

        /// <summary>
        /// The exclusive maximum REST Api version for this mapping.
        /// </summary>
        public Version ExclusiveMaxApiVersion { get; private set; }

        /// <summary>
        /// The original typekey to be returned by the server when processing requests within the REST Api range specified.
        /// </summary>
        public string TypeKey { get; private set; }

        /// <summary>
        /// The original typeName to be returned by the server when processing requests within the REST Api range specified.
        /// </summary>
        public string TypeName { get; private set; }
    }
}
