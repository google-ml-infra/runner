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

﻿using System;
using GitHub.Runner.Worker.Container;
using Xunit;

namespace GitHub.Runner.Common.Tests.Worker.Container
{
    public sealed class ContainerInfoL0
    {
        [Fact]
        [Trait("Level", "L0")]
        [Trait("Category", "Worker")]
        public void MountVolumeConstructorParsesStringInput()
        {
            // Arrange
            MountVolume target = new("/dst/dir"); // Maps anonymous Docker volume into target dir
            MountVolume source_target = new("/src/dir:/dst/dir"); // Maps source to target dir
            MountVolume target_ro = new("/dst/dir:ro");
            MountVolume source_target_ro = new("/src/dir:/dst/dir:ro");

            // Assert
            Assert.Null(target.SourceVolumePath);
            Assert.Equal("/dst/dir", target.TargetVolumePath);
            Assert.False(target.ReadOnly);

            Assert.Equal("/src/dir", source_target.SourceVolumePath);
            Assert.Equal("/dst/dir", source_target.TargetVolumePath);
            Assert.False(source_target.ReadOnly);

            Assert.Null(target_ro.SourceVolumePath);
            Assert.Equal("/dst/dir", target_ro.TargetVolumePath);
            Assert.True(target_ro.ReadOnly);

            Assert.Equal("/src/dir", source_target_ro.SourceVolumePath);
            Assert.Equal("/dst/dir", source_target_ro.TargetVolumePath);
            Assert.True(source_target_ro.ReadOnly);
        }
    }
}
