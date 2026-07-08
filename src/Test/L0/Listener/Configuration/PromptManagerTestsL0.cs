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

﻿using GitHub.Runner.Listener.Configuration;
using GitHub.Runner.Common.Util;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;
using GitHub.Runner.Sdk;

namespace GitHub.Runner.Common.Tests.Listener.Configuration
{
    public class PromptManagerTestsL0
    {
        private readonly string _argName = "SomeArgName";
        private readonly string _description = "Some description";
        private readonly PromptManager _promptManager = new();
        private readonly Mock<ITerminal> _terminal = new();
        private readonly string _unattendedExceptionMessage = "Invalid configuration provided for SomeArgName. Terminating unattended configuration.";

        [Fact]
        [Trait("Level", "L0")]
        [Trait("Category", "PromptManager")]
        public void FallsBackToDefault()
        {
            using (TestHostContext hc = new(this))
            {
                // Arrange.
                _terminal
                    .Setup(x => x.ReadLine())
                    .Returns(string.Empty);
                _terminal
                    .Setup(x => x.ReadSecret())
                    .Throws<InvalidOperationException>();
                hc.SetSingleton(_terminal.Object);
                _promptManager.Initialize(hc);

                // Act.
                string actual = ReadValue(defaultValue: "Some default value");

                // Assert.
                Assert.Equal("Some default value", actual);
            }
        }

        [Fact]
        [Trait("Level", "L0")]
        [Trait("Category", "PromptManager")]
        public void FallsBackToDefaultWhenTrimmed()
        {
            using (TestHostContext hc = new(this))
            {
                // Arrange.
                _terminal
                    .Setup(x => x.ReadLine())
                    .Returns(" ");
                _terminal
                    .Setup(x => x.ReadSecret())
                    .Throws<InvalidOperationException>();
                hc.SetSingleton(_terminal.Object);
                _promptManager.Initialize(hc);

                // Act.
                string actual = ReadValue(defaultValue: "Some default value");

                // Assert.
                Assert.Equal("Some default value", actual);
            }
        }

        [Fact]
        [Trait("Level", "L0")]
        [Trait("Category", "PromptManager")]
        public void FallsBackToDefaultWhenUnattended()
        {
            using (TestHostContext hc = new(this))
            {
                // Arrange.
                _terminal
                    .Setup(x => x.ReadLine())
                    .Throws<InvalidOperationException>();
                _terminal
                    .Setup(x => x.ReadSecret())
                    .Throws<InvalidOperationException>();
                hc.SetSingleton(_terminal.Object);
                _promptManager.Initialize(hc);

                // Act.
                string actual = ReadValue(
                    defaultValue: "Some default value",
                    unattended: true);

                // Assert.
                Assert.Equal("Some default value", actual);
            }
        }

        [Fact]
        [Trait("Level", "L0")]
        [Trait("Category", "PromptManager")]
        public void Prompts()
        {
            using (TestHostContext hc = new(this))
            {
                // Arrange.
                _terminal
                    .Setup(x => x.ReadLine())
                    .Returns("Some prompt value");
                _terminal
                    .Setup(x => x.ReadSecret())
                    .Throws<InvalidOperationException>();
                hc.SetSingleton(_terminal.Object);
                _promptManager.Initialize(hc);

                // Act.
                string actual = ReadValue();

                // Assert.
                Assert.Equal("Some prompt value", actual);
            }
        }

        [Fact]
        [Trait("Level", "L0")]
        [Trait("Category", "PromptManager")]
        public void PromptsAgainWhenEmpty()
        {
            using (TestHostContext hc = new(this))
            {
                // Arrange.
                var readLineValues = new Queue<string>(new[] { string.Empty, "Some prompt value" });
                _terminal
                    .Setup(x => x.ReadLine())
                    .Returns(() => readLineValues.Dequeue());
                _terminal
                    .Setup(x => x.ReadSecret())
                    .Throws<InvalidOperationException>();
                hc.SetSingleton(_terminal.Object);
                _promptManager.Initialize(hc);

                // Act.
                string actual = ReadValue();

                // Assert.
                Assert.Equal("Some prompt value", actual);
                _terminal.Verify(x => x.ReadLine(), Times.Exactly(2));
            }
        }

        [Fact]
        [Trait("Level", "L0")]
        [Trait("Category", "PromptManager")]
        public void PromptsAgainWhenFailsValidation()
        {
            using (TestHostContext hc = new(this))
            {
                // Arrange.
                var readLineValues = new Queue<string>(new[] { "Some invalid prompt value", "Some valid prompt value" });
                _terminal
                    .Setup(x => x.ReadLine())
                    .Returns(() => readLineValues.Dequeue());
                _terminal
                    .Setup(x => x.ReadSecret())
                    .Throws<InvalidOperationException>();
                hc.SetSingleton(_terminal.Object);
                _promptManager.Initialize(hc);

                // Act.
                string actual = ReadValue(validator: x => x == "Some valid prompt value");

                // Assert.
                Assert.Equal("Some valid prompt value", actual);
                _terminal.Verify(x => x.ReadLine(), Times.Exactly(2));
            }
        }

        [Fact]
        [Trait("Level", "L0")]
        [Trait("Category", "PromptManager")]
        public void ThrowsWhenUnattended()
        {
            using (TestHostContext hc = new(this))
            {
                // Arrange.
                _terminal
                    .Setup(x => x.ReadLine())
                    .Throws<InvalidOperationException>();
                _terminal
                    .Setup(x => x.ReadSecret())
                    .Throws<InvalidOperationException>();
                hc.SetSingleton(_terminal.Object);
                _promptManager.Initialize(hc);

                try
                {
                    // Act.
                    string actual = ReadValue(unattended: true);

                    // Assert.
                    throw new InvalidOperationException();
                }
                catch (Exception ex)
                {
                    // Assert.
                    Assert.Equal(_unattendedExceptionMessage, ex.Message);
                }
            }
        }

        private string ReadValue(
            bool secret = false,
            string defaultValue = null,
            Func<string, bool> validator = null,
            bool unattended = false)
        {
            return _promptManager.ReadValue(
                argName: _argName,
                description: _description,
                secret: secret,
                defaultValue: defaultValue,
                validator: validator ?? DefaultValidator,
                unattended: unattended);
        }

        private static bool DefaultValidator(string val)
        {
            return true;
        }
    }
}
