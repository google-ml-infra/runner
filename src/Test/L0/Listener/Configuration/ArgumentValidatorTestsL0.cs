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

﻿using GitHub.Runner.Listener.Configuration;
using Xunit;

namespace GitHub.Runner.Common.Tests.Listener.Configuration
{
    public sealed class ArgumentValidatorTestsL0
    {
        [Fact]
        [Trait("Level", "L0")]
        [Trait("Category", "ArgumentValidator")]
        public void ServerUrlValidator()
        {
            using (TestHostContext hc = new(this))
            {
                Assert.True(Validators.ServerUrlValidator("http://servername"));
                Assert.False(Validators.ServerUrlValidator("Fail"));
                Assert.False(Validators.ServerUrlValidator("ftp://servername"));
            }
        }

        [Fact]
        [Trait("Level", "L0")]
        [Trait("Category", "ArgumentValidator")]
        public void AuthSchemeValidator()
        {
            using (TestHostContext hc = new(this))
            {
                Assert.True(Validators.AuthSchemeValidator("OAuth"));
                Assert.False(Validators.AuthSchemeValidator("Fail"));
            }
        }

        [Fact]
        [Trait("Level", "L0")]
        [Trait("Category", "ArgumentValidator")]
        public void NonEmptyValidator()
        {
            using (TestHostContext hc = new(this))
            {
                Assert.True(Validators.NonEmptyValidator("test"));
                Assert.False(Validators.NonEmptyValidator(string.Empty));
            }
        }


#if OS_WINDOWS
        [Fact]
        [Trait("Level", "L0")]
        [Trait("Category", "ArgumentValidator")]
        public void WindowsLogonAccountValidator()
        {
            using (TestHostContext hc = new TestHostContext(this))
            {
                Assert.False(Validators.NTAccountValidator(string.Empty));
                Assert.True(Validators.NTAccountValidator("NT AUTHORITY\\LOCAL SERVICE"));
            }
        }
#endif
    }
}
