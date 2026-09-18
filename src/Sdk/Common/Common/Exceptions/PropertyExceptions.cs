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

using GitHub.Services.Common.Internal;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using System.Security;

namespace GitHub.Services.Common
{
    /// <summary>
    /// Thrown when validating user input. Similar to ArgumentException but doesn't require the property to be an input parameter.
    /// </summary>
    [Serializable]
    [SuppressMessage("Microsoft.Design", "CA1032:ImplementStandardExceptionConstructors")]
    [ExceptionMapping("0.0", "3.0", "VssPropertyValidationException", "GitHub.Services.Common.VssPropertyValidationException, GitHub.Services.Common, Version=14.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
    public class VssPropertyValidationException : VssServiceException
    {
        public VssPropertyValidationException(String propertyName, String message)
            : base(message)
        {
            PropertyName = propertyName;
        }

        public VssPropertyValidationException(String propertyName, String message, Exception innerException)
            : base(message, innerException)
        {
            PropertyName = propertyName;
        }

        protected VssPropertyValidationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            PropertyName = info.GetString("PropertyName");
        }

        public String PropertyName { get; set; }

        [Obsolete]
        [SecurityCritical]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("PropertyName", PropertyName);
        }
    }

    /// <summary>
    /// PropertyTypeNotSupportedException - this is thrown when a type is DBNull or an Object type other than a Byte array.
    /// </summary>
    [Serializable]
    [SuppressMessage("Microsoft.Design", "CA1032:ImplementStandardExceptionConstructors")]
    [ExceptionMapping("0.0", "3.0", "PropertyTypeNotSupportedException", "GitHub.Services.Common.PropertyTypeNotSupportedException, GitHub.Services.Common, Version=14.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
    public class PropertyTypeNotSupportedException : VssPropertyValidationException
    {
        public PropertyTypeNotSupportedException(String propertyName, Type type)
            : base(propertyName, CommonResources.VssUnsupportedPropertyValueType(propertyName, type.FullName))
        {
        }

        protected PropertyTypeNotSupportedException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
