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
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace GitHub.Services.Common
{
    /// <summary>
    /// Thrown when a config file fails to load
    /// </summary
    [Serializable]
    [SuppressMessage("Microsoft.Design", "CA1032:ImplementStandardExceptionConstructors")]
    [ExceptionMapping("0.0", "3.0", "ConfigFileException", "GitHub.Services.Common.ConfigFileException, GitHub.Services.Common, Version=14.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
    public class ConfigFileException : VssException
    {
        public ConfigFileException(String message)
            : base(message)
        {
        }

        public ConfigFileException(String message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected ConfigFileException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }

    [Serializable]
    [SuppressMessage("Microsoft.Design", "CA1032:ImplementStandardExceptionConstructors")]
    [ExceptionMapping("0.0", "3.0", "VssServiceException", "GitHub.Services.Common.VssServiceException, GitHub.Services.Common, Version=14.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
    public class VssServiceException : VssException
    {
        public VssServiceException()
            : base()
        {
        }

        public VssServiceException(String message, Exception innerException)
            : base(message, innerException)
        {
        }

        public VssServiceException(String message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes an exception from serialized data
        /// </summary>
        /// <param name="info">object holding the serialized data</param>
        /// <param name="context">context info about the source or destination</param>
        protected VssServiceException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        /// Gets the type name and key for serialization of this exception.
        /// If not provided, the serializer will provide default values.
        /// </summary>
        public virtual void GetTypeNameAndKey(Version restApiVersion, out String typeName, out String typeKey)
        {
            GetTypeNameAndKeyForExceptionType(GetType(), restApiVersion, out typeName, out typeKey);
        }
    }
}
