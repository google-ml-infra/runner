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

﻿#nullable enable
using System;
using System.IO;
using System.Runtime.CompilerServices;
using GitHub.DistributedTask.Pipelines;
using GitHub.Runner.Sdk;
using GitHub.Runner.Worker;
using Moq;
using Xunit;

namespace GitHub.Runner.Common.Tests.Worker;

public class SnapshotOperationProviderL0
{
    private Mock<IExecutionContext>? _ec;
    private SnapshotOperationProvider? _snapshotOperationProvider;
    private string? _snapshotRequestFilePath;
    private string? _snapshotRequestDirectoryPath;

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    [Trait("Level", "L0")]
    [Trait("Category", "Worker")]
    public async void CreateSnapshotRequestAsync(bool shouldSnapshotDirectoryAlreadyExist)
    {
        using (TestHostContext testHostContext = CreateTestHostContext())
        {
            //Arrange
            Setup(testHostContext, shouldSnapshotDirectoryAlreadyExist);
            var expectedSnapshot = new Snapshot(Guid.NewGuid().ToString());

            //Act
            await _snapshotOperationProvider!.CreateSnapshotRequestAsync(_ec!.Object, expectedSnapshot);

            //Assert
            var actualSnapshot = IOUtil.LoadObject<Snapshot>(_snapshotRequestFilePath);
            Assert.NotNull(actualSnapshot);
            Assert.Equal(expectedSnapshot.ImageName, actualSnapshot!.ImageName);
            _ec.Verify(ec => ec.Write(null, $"Request written to: {_snapshotRequestFilePath}"), Times.Once);
            _ec.Verify(ec => ec.Write(null, $"Image Name: {expectedSnapshot.ImageName} Version: {expectedSnapshot.Version}"), Times.Once);
            _ec.Verify(ec => ec.Write(null, "This request will be processed after the job completes. You will not receive any feedback on the snapshot process within the workflow logs of this job."), Times.Once);
            _ec.Verify(ec => ec.Write(null, "If the snapshot process is successful, you should see a new image with the requested name in the list of available custom images when creating a new GitHub-hosted Runner."), Times.Once);
            _ec.VerifyNoOtherCalls();
        }
    }

    private void Setup(IHostContext hostContext, bool shouldSnapshotDirectoryAlreadyExist)
    {
        _ec = new Mock<IExecutionContext>();
        _snapshotOperationProvider = new SnapshotOperationProvider();
        _snapshotOperationProvider.Initialize(hostContext);
        _snapshotRequestFilePath = Path.Combine(hostContext.GetDirectory(WellKnownDirectory.Root), ".snapshot", "request.json");
        _snapshotRequestDirectoryPath = Path.GetDirectoryName(_snapshotRequestFilePath);

        if (_snapshotRequestDirectoryPath != null)
        {
            // Clean up any existing the snapshot directory and its contents before starting the test. 
            if (Directory.Exists(_snapshotRequestDirectoryPath))
            {
                Directory.Delete(_snapshotRequestDirectoryPath, true);
            }

            if (shouldSnapshotDirectoryAlreadyExist)
            {
                // Create a fresh snapshot directory if it's required for the test. 
                Directory.CreateDirectory(_snapshotRequestDirectoryPath);
            }
        }
    }

    private TestHostContext CreateTestHostContext([CallerMemberName] string testName = "")
    {
        var testHostContext = new TestHostContext(this, testName);
        _ec = new Mock<IExecutionContext>();
        _ec.Object.Initialize(testHostContext);
        return testHostContext;
    }
}
