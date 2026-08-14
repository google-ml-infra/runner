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
using System.Runtime.Serialization;
using GitHub.Services.Common;

namespace GitHub.Services.WebApi.Patch
{
    [Serializable]
    [ExceptionMapping("0.0", "3.0", "PatchOperationFailedException", "GitHub.Services.WebApi.Patch.PatchOperationFailedException, GitHub.Services.WebApi, Version=14.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
    public class PatchOperationFailedException : VssServiceException
    {
        public PatchOperationFailedException()
        {
        }

        public PatchOperationFailedException(string message)
            : base(message)
        {
        }

        public PatchOperationFailedException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected PatchOperationFailedException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }

    [Serializable]
    [ExceptionMapping("0.0", "3.0", "InvalidPatchFieldNameException", "GitHub.Services.WebApi.Patch.InvalidPatchFieldNameException, GitHub.Services.WebApi, Version=14.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
    public class InvalidPatchFieldNameException : PatchOperationFailedException
    {
        public InvalidPatchFieldNameException(string message)
            : base(message)
        {
        }
    }

    [Serializable]
    [ExceptionMapping("0.0", "3.0", "TestPatchOperationFailedException", "GitHub.Services.WebApi.Patch.TestPatchOperationFailedException, GitHub.Services.WebApi, Version=14.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
    public class TestPatchOperationFailedException : PatchOperationFailedException
    {
        public TestPatchOperationFailedException()
        {
        }

        public TestPatchOperationFailedException(string message)
            : base(message)
        {
        }

        public TestPatchOperationFailedException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected TestPatchOperationFailedException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
