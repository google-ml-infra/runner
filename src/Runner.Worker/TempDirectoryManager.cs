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

﻿using GitHub.Runner.Common.Util;
using System;
using System.IO;
using System.Threading;
using GitHub.Runner.Common;
using GitHub.Runner.Sdk;

namespace GitHub.Runner.Worker
{
    [ServiceLocator(Default = typeof(TempDirectoryManager))]
    public interface ITempDirectoryManager : IRunnerService
    {
        void InitializeTempDirectory(IExecutionContext jobContext);
        void CleanupTempDirectory();
    }

    public sealed class TempDirectoryManager : RunnerService, ITempDirectoryManager
    {
        private string _tempDirectory;

        public override void Initialize(IHostContext hostContext)
        {
            base.Initialize(hostContext);
            _tempDirectory = HostContext.GetDirectory(WellKnownDirectory.Temp);
        }

        public void InitializeTempDirectory(IExecutionContext jobContext)
        {
            ArgUtil.NotNull(jobContext, nameof(jobContext));
            ArgUtil.NotNullOrEmpty(_tempDirectory, nameof(_tempDirectory));
            jobContext.SetRunnerContext("temp", _tempDirectory);
            jobContext.Debug($"Cleaning runner temp folder: {_tempDirectory}");
            try
            {
                IOUtil.DeleteDirectory(_tempDirectory, contentsOnly: true, continueOnContentDeleteError: true, cancellationToken: jobContext.CancellationToken);
            }
            catch (Exception ex)
            {
                Trace.Error(ex);
            }
            finally
            {
                // make sure folder exists
                Directory.CreateDirectory(_tempDirectory);
            }
        }

        public void CleanupTempDirectory()
        {
            ArgUtil.NotNullOrEmpty(_tempDirectory, nameof(_tempDirectory));
            Trace.Info($"Cleaning runner temp folder: {_tempDirectory}");
            try
            {
                IOUtil.DeleteDirectory(_tempDirectory, contentsOnly: true, continueOnContentDeleteError: true, cancellationToken: CancellationToken.None);
            }
            catch (Exception ex)
            {
                Trace.Error(ex);
            }
        }
    }
}
