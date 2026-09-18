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

using GitHub.DistributedTask.WebApi;
using GitHub.Runner.Worker;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Xunit;

namespace GitHub.Runner.Common.Tests.Worker
{
    public sealed class VariablesL0
    {
        [Fact]
        [Trait("Level", "L0")]
        [Trait("Category", "Worker")]
        public void Constructor_AppliesMaskHints()
        {
            using (TestHostContext hc = new(this))
            {
                // Arrange.
                var copy = new Dictionary<string, VariableValue>
                {
                    { "MySecretName", new VariableValue("My secret value", true) },
                    { "MyPublicVariable", "My public value" },
                };
                var variables = new Variables(hc, copy);

                // Assert.
                Assert.Equal(2, variables.AllVariables.Count());
                Assert.Equal("My public value", variables.Get("MyPublicVariable"));
                Assert.Equal("My secret value", variables.Get("MySecretName"));
            }
        }

        [Fact]
        [Trait("Level", "L0")]
        [Trait("Category", "Worker")]
        public void Constructor_HandlesNullValue()
        {
            using (TestHostContext hc = new(this))
            {
                // Arrange.
                var copy = new Dictionary<string, VariableValue>
                {
                    { "variable1",  new VariableValue(null, false) },
                    { "variable2", "some variable 2 value" },
                };

                // Act.
                var variables = new Variables(hc, copy);

                // Assert.
                Assert.Equal(string.Empty, variables.Get("variable1"));
                Assert.Equal("some variable 2 value", variables.Get("variable2"));
            }
        }

        [Fact]
        [Trait("Level", "L0")]
        [Trait("Category", "Worker")]
        public void Constructor_SetsNullAsEmpty()
        {
            using (TestHostContext hc = new(this))
            {
                // Arrange.
                var copy = new Dictionary<string, VariableValue>
                {
                    { "variable1", new VariableValue(null, false) },
                };

                // Act.
                var variables = new Variables(hc, copy);

                // Assert.
                Assert.Equal(string.Empty, variables.Get("variable1"));
            }
        }

        [Fact]
        [Trait("Level", "L0")]
        [Trait("Category", "Worker")]
        public void Constructor_SetsOrdinalIgnoreCaseComparer()
        {
            using (TestHostContext hc = new(this))
            {
                // Arrange.
                CultureInfo currentCulture = CultureInfo.CurrentCulture;
                CultureInfo currentUICulture = CultureInfo.CurrentUICulture;
                try
                {
                    CultureInfo.CurrentCulture = new CultureInfo("tr-TR");
                    CultureInfo.CurrentUICulture = new CultureInfo("tr-TR");
                    var copy = new Dictionary<string, VariableValue>
                    {
                        { "i", "foo" },
                        { "I", "foo" },
                    };

                    // Act.
                    var variables = new Variables(hc, copy);

                    // Assert.
                    Assert.Equal(1, variables.AllVariables.Count());
                }
                finally
                {
                    // Cleanup.
                    CultureInfo.CurrentCulture = currentCulture;
                    CultureInfo.CurrentUICulture = currentUICulture;
                }
            }
        }

        [Fact]
        [Trait("Level", "L0")]
        [Trait("Category", "Worker")]
        public void Constructor_SkipVariableWithEmptyName()
        {
            using (TestHostContext hc = new(this))
            {
                // Arrange.
                var copy = new Dictionary<string, VariableValue>
                {
                    { "", "" },
                    { "   ", "" },
                    { "MyPublicVariable", "My public value" },
                };

                var variables = new Variables(hc, copy);

                // Assert.
                Assert.Equal(1, variables.AllVariables.Count());
                Assert.Equal("MyPublicVariable", variables.AllVariables.Single().Name);
                Assert.Equal("My public value", variables.AllVariables.Single().Value);
            }
        }

        [Fact]
        [Trait("Level", "L0")]
        [Trait("Category", "Worker")]
        public void Get_ReturnsNullIfNotFound()
        {
            using (TestHostContext hc = new(this))
            {
                // Arrange.
                var variables = new Variables(hc, new Dictionary<string, VariableValue>());

                // Act.
                string actual = variables.Get("no such");

                // Assert.
                Assert.Null(actual);
            }
        }

        [Fact]
        [Trait("Level", "L0")]
        [Trait("Category", "Worker")]
        public void GetBoolean_DoesNotThrowWhenNull()
        {
            using (TestHostContext hc = new(this))
            {
                // Arrange.
                var variables = new Variables(hc, new Dictionary<string, VariableValue>());

                // Act.
                bool? actual = variables.GetBoolean("no such");

                // Assert.
                Assert.Null(actual);
            }
        }

        [Fact]
        [Trait("Level", "L0")]
        [Trait("Category", "Worker")]
        public void GetEnum_DoesNotThrowWhenNull()
        {
            using (TestHostContext hc = new(this))
            {
                // Arrange.
                var variables = new Variables(hc, new Dictionary<string, VariableValue>());

                // Act.
                System.IO.FileShare? actual = variables.GetEnum<System.IO.FileShare>("no such");

                // Assert.
                Assert.Null(actual);
            }
        }
    }
}
