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

﻿using System;
using GitHub.Runner.Sdk;
using Xunit;

namespace GitHub.Runner.Common.Tests.Util
{
    public class UrlUtilL0
    {
        [Fact]
        [Trait("Level", "L0")]
        [Trait("Category", "Common")]
        public void GetCredentialEmbeddedUrl_NoUsernameAndPassword()
        {
            // Act.
            Uri result = UrlUtil.GetCredentialEmbeddedUrl(new Uri("https://github.com/actions/runner.git"), string.Empty, string.Empty);
            // Actual
            Assert.Equal("https://github.com/actions/runner.git", result.AbsoluteUri);
        }

        [Fact]
        [Trait("Level", "L0")]
        [Trait("Category", "Common")]
        public void GetCredentialEmbeddedUrl_NoUsername()
        {
            // Act.
            Uri result = UrlUtil.GetCredentialEmbeddedUrl(new Uri("https://github.com/actions/runner.git"), string.Empty, "password123");
            // Actual
            Assert.Equal("https://emptyusername:password123@github.com/actions/runner.git", result.AbsoluteUri);
        }

        [Fact]
        [Trait("Level", "L0")]
        [Trait("Category", "Common")]
        public void GetCredentialEmbeddedUrl_NoPassword()
        {
            // Act.
            Uri result = UrlUtil.GetCredentialEmbeddedUrl(new Uri("https://github.com/actions/runner.git"), "user123", string.Empty);
            // Actual
            Assert.Equal("https://user123@github.com/actions/runner.git", result.AbsoluteUri);
        }

        [Fact]
        [Trait("Level", "L0")]
        [Trait("Category", "Common")]
        public void GetCredentialEmbeddedUrl_HasUsernameAndPassword()
        {
            // Act.
            Uri result = UrlUtil.GetCredentialEmbeddedUrl(new Uri("https://github.com/actions/runner.git"), "user123", "password123");
            // Actual
            Assert.Equal("https://user123:password123@github.com/actions/runner.git", result.AbsoluteUri);
        }

        [Fact]
        [Trait("Level", "L0")]
        [Trait("Category", "Common")]
        public void GetCredentialEmbeddedUrl_UsernameAndPasswordEncoding()
        {
            // Act.
            Uri result = UrlUtil.GetCredentialEmbeddedUrl(new Uri("https://github.com/actions/runner.git"), "user 123", "password 123");
            // Actual
            Assert.Equal("https://user%20123:password%20123@github.com/actions/runner.git", result.AbsoluteUri);
        }
    }
}
