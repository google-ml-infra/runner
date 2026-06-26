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

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using GitHub.DistributedTask.WebApi;
using GitHub.Runner.Sdk;
using GitHub.Runner.Worker;
using GitHub.Runner.Worker.Handlers;
using Moq;
using Xunit;

namespace GitHub.Runner.Common.Tests.Worker.Handlers
{
    public sealed class NodeHandlerL0
    {
        [Fact]
        [Trait("Level", "L0")]
        [Trait("Category", "Worker")]
        public void NodeJSActionExecutionDataSupportsNode24()
        {
            // Create NodeJSActionExecutionData with node24
            var nodeJSData = new NodeJSActionExecutionData
            {
                NodeVersion = "node24",
                Script = "test.js"
            };
            
            // Act & Assert
            Assert.Equal("node24", nodeJSData.NodeVersion);
            Assert.Equal(ActionExecutionType.NodeJS, nodeJSData.ExecutionType);
        }
    }
}
