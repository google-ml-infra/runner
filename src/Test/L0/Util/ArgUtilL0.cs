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

using GitHub.Runner.Sdk;
using System;
using Xunit;

namespace GitHub.Runner.Common.Tests.Util
{
    public sealed class ArgUtilL0
    {
        [Fact]
        [Trait("Level", "L0")]
        [Trait("Category", "Common")]
        public void Equal_MatchesObjectEquality()
        {
            using (TestHostContext hc = new(this))
            {
                Tracing trace = hc.GetTrace();

                // Arrange.
                string expected = "Some string".ToLower();  // ToLower is required to avoid reference equality
                string actual = "Some string".ToLower();    // due to compile-time string interning.

                // Act/Assert.
                ArgUtil.Equal(expected: expected, actual: actual, name: "Some parameter");
            }
        }

        [Fact]
        [Trait("Level", "L0")]
        [Trait("Category", "Common")]
        public void Equal_MatchesReferenceEquality()
        {
            using (TestHostContext hc = new(this))
            {
                Tracing trace = hc.GetTrace();

                // Arrange.
                object expected = new();
                object actual = expected;

                // Act/Assert.
                ArgUtil.Equal(expected: expected, actual: actual, name: "Some parameter");
            }
        }

        [Fact]
        [Trait("Level", "L0")]
        [Trait("Category", "Common")]
        public void Equal_MatchesStructEquality()
        {
            using (TestHostContext hc = new(this))
            {
                Tracing trace = hc.GetTrace();

                // Arrange.
                int expected = 123;
                int actual = expected;

                // Act/Assert.
                ArgUtil.Equal(expected: expected, actual: actual, name: "Some parameter");
            }
        }

        [Fact]
        [Trait("Level", "L0")]
        [Trait("Category", "Common")]
        public void Equal_ThrowsWhenActualObjectIsNull()
        {
            using (TestHostContext hc = new(this))
            {
                Tracing trace = hc.GetTrace();

                // Arrange.
                object expected = new();
                object actual = null;

                // Act/Assert.
                Assert.Throws<ArgumentOutOfRangeException>(() =>
                {
                    ArgUtil.Equal(expected: expected, actual: actual, name: "Some parameter");
                });
            }
        }

        [Fact]
        [Trait("Level", "L0")]
        [Trait("Category", "Common")]
        public void Equal_ThrowsWhenExpectedObjectIsNull()
        {
            using (TestHostContext hc = new(this))
            {
                Tracing trace = hc.GetTrace();

                // Arrange.
                object expected = null;
                object actual = new();

                // Act/Assert.
                Assert.Throws<ArgumentOutOfRangeException>(() =>
                {
                    ArgUtil.Equal(expected: expected, actual: actual, name: "Some parameter");
                });
            }
        }

        [Fact]
        [Trait("Level", "L0")]
        [Trait("Category", "Common")]
        public void Equal_ThrowsWhenObjectsAreNotEqual()
        {
            using (TestHostContext hc = new(this))
            {
                Tracing trace = hc.GetTrace();

                // Arrange.
                object expected = new();
                object actual = new();

                // Act/Assert.
                Assert.Throws<ArgumentOutOfRangeException>(() =>
                {
                    ArgUtil.Equal(expected: expected, actual: actual, name: "Some parameter");
                });
            }
        }

        [Fact]
        [Trait("Level", "L0")]
        [Trait("Category", "Common")]
        public void Equal_ThrowsWhenStructsAreNotEqual()
        {
            using (TestHostContext hc = new(this))
            {
                Tracing trace = hc.GetTrace();

                // Arrange.
                int expected = 123;
                int actual = 456;

                // Act/Assert.
                Assert.Throws<ArgumentOutOfRangeException>(() =>
                {
                    ArgUtil.Equal(expected: expected, actual: actual, name: "Some parameter");
                });
            }
        }
    }
}
