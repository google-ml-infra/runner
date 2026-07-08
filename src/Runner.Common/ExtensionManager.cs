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

﻿using GitHub.Runner.Sdk;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace GitHub.Runner.Common
{
    [ServiceLocator(Default = typeof(ExtensionManager))]
    public interface IExtensionManager : IRunnerService
    {
        List<T> GetExtensions<T>() where T : class, IExtension;
    }

    public sealed class ExtensionManager : RunnerService, IExtensionManager
    {
        private readonly ConcurrentDictionary<Type, List<IExtension>> _cache = new();

        public List<T> GetExtensions<T>() where T : class, IExtension
        {
            Trace.Info("Getting extensions for interface: '{0}'", typeof(T).FullName);
            List<IExtension> extensions = _cache.GetOrAdd(
                key: typeof(T),
                valueFactory: (Type key) =>
                {
                    return LoadExtensions<T>();
                });
            return extensions.Select(x => x as T).ToList();
        }

        //
        // We will load extensions from assembly
        // once AssemblyLoadContext.Resolving event is able to
        // resolve dependency recursively
        //
        private List<IExtension> LoadExtensions<T>() where T : class, IExtension
        {
            var extensions = new List<IExtension>();
            switch (typeof(T).FullName)
            {
                // Action command extensions.
                case "GitHub.Runner.Worker.IActionCommandExtension":
                    Add<T>(extensions, "GitHub.Runner.Worker.InternalPluginSetRepoPathCommandExtension, Runner.Worker");
                    Add<T>(extensions, "GitHub.Runner.Worker.SetEnvCommandExtension, Runner.Worker");
                    Add<T>(extensions, "GitHub.Runner.Worker.SetOutputCommandExtension, Runner.Worker");
                    Add<T>(extensions, "GitHub.Runner.Worker.SaveStateCommandExtension, Runner.Worker");
                    Add<T>(extensions, "GitHub.Runner.Worker.AddPathCommandExtension, Runner.Worker");
                    Add<T>(extensions, "GitHub.Runner.Worker.AddMaskCommandExtension, Runner.Worker");
                    Add<T>(extensions, "GitHub.Runner.Worker.AddMatcherCommandExtension, Runner.Worker");
                    Add<T>(extensions, "GitHub.Runner.Worker.RemoveMatcherCommandExtension, Runner.Worker");
                    Add<T>(extensions, "GitHub.Runner.Worker.WarningCommandExtension, Runner.Worker");
                    Add<T>(extensions, "GitHub.Runner.Worker.ErrorCommandExtension, Runner.Worker");
                    Add<T>(extensions, "GitHub.Runner.Worker.NoticeCommandExtension, Runner.Worker");
                    Add<T>(extensions, "GitHub.Runner.Worker.DebugCommandExtension, Runner.Worker");
                    Add<T>(extensions, "GitHub.Runner.Worker.GroupCommandExtension, Runner.Worker");
                    Add<T>(extensions, "GitHub.Runner.Worker.EndGroupCommandExtension, Runner.Worker");
                    Add<T>(extensions, "GitHub.Runner.Worker.EchoCommandExtension, Runner.Worker");
                    break;
                case "GitHub.Runner.Worker.IFileCommandExtension":
                    Add<T>(extensions, "GitHub.Runner.Worker.AddPathFileCommand, Runner.Worker");
                    Add<T>(extensions, "GitHub.Runner.Worker.SetEnvFileCommand, Runner.Worker");
                    Add<T>(extensions, "GitHub.Runner.Worker.CreateStepSummaryCommand, Runner.Worker");
                    Add<T>(extensions, "GitHub.Runner.Worker.SaveStateFileCommand, Runner.Worker");
                    Add<T>(extensions, "GitHub.Runner.Worker.SetOutputFileCommand, Runner.Worker");
                    break;
                case "GitHub.Runner.Listener.Check.ICheckExtension":
                    Add<T>(extensions, "GitHub.Runner.Listener.Check.InternetCheck, Runner.Listener");
                    Add<T>(extensions, "GitHub.Runner.Listener.Check.ActionsCheck, Runner.Listener");
                    Add<T>(extensions, "GitHub.Runner.Listener.Check.GitCheck, Runner.Listener");
                    Add<T>(extensions, "GitHub.Runner.Listener.Check.NodeJsCheck, Runner.Listener");
                    break;
                default:
                    // This should never happen.
                    throw new NotSupportedException($"Unexpected extension type: '{typeof(T).FullName}'");
            }

            return extensions;
        }

        private void Add<T>(List<IExtension> extensions, string assemblyQualifiedName) where T : class, IExtension
        {
            Trace.Info($"Creating instance: {assemblyQualifiedName}");
            Type type = Type.GetType(assemblyQualifiedName, throwOnError: true);
            var extension = Activator.CreateInstance(type) as T;
            ArgUtil.NotNull(extension, nameof(extension));
            extension.Initialize(HostContext);
            extensions.Add(extension);
        }
    }
}
