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

﻿using GitHub.DistributedTask.Expressions2;
using GitHub.DistributedTask.Expressions2.Sdk;
using GitHub.DistributedTask.ObjectTemplating;
using System;
using System.Collections.Generic;
using Xunit;

namespace GitHub.Runner.Common.Tests.Sdk
{
    /// <summary>
    /// Regression tests for ExpressionParser.CreateTree to verify that
    /// the case function does not accidentally set allowUnknownKeywords.
    /// </summary>
    public sealed class ExpressionParserL0
    {
        [Fact]
        [Trait("Level", "L0")]
        [Trait("Category", "Sdk")]
        public void CreateTree_RejectsUnrecognizedNamedValue()
        {
            // Regression: the case function parameter was passed positionally into
            // the allowUnknownKeywords parameter, causing all named values
            // to be silently accepted.
            var parser = new ExpressionParser();
            var namedValues = new List<INamedValueInfo>
            {
                new NamedValueInfo<ContextValueNode>("inputs"),
            };

            var ex = Assert.Throws<ParseException>(() =>
                parser.CreateTree("github.event.repository.private", null, namedValues, null));

            Assert.Contains("Unrecognized named-value", ex.Message);
        }

        [Fact]
        [Trait("Level", "L0")]
        [Trait("Category", "Sdk")]
        public void CreateTree_AcceptsRecognizedNamedValue()
        {
            var parser = new ExpressionParser();
            var namedValues = new List<INamedValueInfo>
            {
                new NamedValueInfo<ContextValueNode>("inputs"),
            };

            var node = parser.CreateTree("inputs.foo", null, namedValues, null);

            Assert.NotNull(node);
        }

        [Fact]
        [Trait("Level", "L0")]
        [Trait("Category", "Sdk")]
        public void CreateTree_CaseFunctionWorks()
        {
            var parser = new ExpressionParser();
            var namedValues = new List<INamedValueInfo>
            {
                new NamedValueInfo<ContextValueNode>("github"),
            };

            var node = parser.CreateTree("case(github.event_name, 'push', 'Push Event')", null, namedValues, null);

            Assert.NotNull(node);
        }

        [Fact]
        [Trait("Level", "L0")]
        [Trait("Category", "Sdk")]
        public void CreateTree_CaseFunctionDoesNotAffectUnknownKeywords()
        {
            // The key regression test: unrecognized named values must still be rejected.
            var parser = new ExpressionParser();
            var namedValues = new List<INamedValueInfo>
            {
                new NamedValueInfo<ContextValueNode>("inputs"),
            };

            var ex = Assert.Throws<ParseException>(() =>
                parser.CreateTree("github.ref", null, namedValues, null));

            Assert.Contains("Unrecognized named-value", ex.Message);
        }
    }
}
