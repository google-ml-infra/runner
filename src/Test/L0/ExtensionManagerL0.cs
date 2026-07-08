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

﻿using GitHub.Runner.Worker;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace GitHub.Runner.Common.Tests
{
    public sealed class ExtensionManagerL0
    {
        [Fact]
        [Trait("Level", "L0")]
        [Trait("Category", "Common")]
        public void LoadsTypeFromString()
        {
            using (TestHostContext tc = new(this))
            {
                // Arrange.
                var manager = new ExtensionManager();
                manager.Initialize(tc);

                // Act.
                List<IActionCommandExtension> extensions = manager.GetExtensions<IActionCommandExtension>();

                // Assert.
                Assert.True(
                    extensions.Any(x => x is SetEnvCommandExtension),
                    $"Expected {nameof(SetEnvCommandExtension)} extension to be returned as a job extension.");
            }
        }

        [Fact]
        [Trait("Level", "L0")]
        [Trait("Category", "Common")]
        public void LoadsTypes()
        {
            using (TestHostContext tc = new(this))
            {
                // Arrange.
                var manager = new ExtensionManager();
                manager.Initialize(tc);

                // Act/Assert.
                AssertContains<GitHub.Runner.Worker.IActionCommandExtension>(
                    manager,
                    concreteType: typeof(GitHub.Runner.Worker.SetEnvCommandExtension));
            }
        }

        private static void AssertContains<T>(ExtensionManager manager, Type concreteType) where T : class, IExtension
        {
            // Act.
            List<T> extensions = manager.GetExtensions<T>();

            // Assert.
            Assert.True(
                extensions.Any(x => x.GetType() == concreteType),
                $"Expected '{typeof(T).FullName}' extensions to contain concrete type '{concreteType.FullName}'.");
        }
    }
}
