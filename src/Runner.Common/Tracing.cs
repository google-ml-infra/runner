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

using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using GitHub.DistributedTask.Logging;
using GitHub.Runner.Sdk;

namespace GitHub.Runner.Common
{
    public sealed class Tracing : ITraceWriter, IDisposable
    {
        private ISecretMasker _secretMasker;
        private TraceSource _traceSource;

        /// <summary>
        /// The underlying <see cref="System.Diagnostics.TraceSource"/> for this instance.
        /// Useful when third-party libraries require a <see cref="System.Diagnostics.TraceSource"/>
        /// to route their diagnostics into the runner's log infrastructure.
        /// </summary>
        public TraceSource Source => _traceSource;

        public Tracing(string name, ISecretMasker secretMasker, SourceSwitch sourceSwitch, HostTraceListener traceListener, StdoutTraceListener stdoutTraceListener = null)
        {
            ArgUtil.NotNull(secretMasker, nameof(secretMasker));
            _secretMasker = secretMasker;
            _traceSource = new TraceSource(name);
            _traceSource.Switch = sourceSwitch;

            // Remove the default trace listener.
            if (_traceSource.Listeners.Count > 0 &&
                _traceSource.Listeners[0] is DefaultTraceListener)
            {
                _traceSource.Listeners.RemoveAt(0);
            }

            _traceSource.Listeners.Add(traceListener);
            if (stdoutTraceListener != null)
            {
                _traceSource.Listeners.Add(stdoutTraceListener);
            }
        }

        public void Info(string message)
        {
            Trace(TraceEventType.Information, message);
        }

        public void Info(string format, params object[] args)
        {
            Trace(TraceEventType.Information, StringUtil.Format(format, args));
        }

        public void Info(object item)
        {
            string json = JsonConvert.SerializeObject(item, Formatting.Indented);
            Trace(TraceEventType.Information, json);
        }

        public void Error(Exception exception)
        {
            Trace(TraceEventType.Error, exception.ToString());
            var innerEx = exception.InnerException;
            while (innerEx != null)
            {
                Trace(TraceEventType.Error, "#####################################################");
                Trace(TraceEventType.Error, innerEx.ToString());
                innerEx = innerEx.InnerException;
            }
        }

        // Do not remove the non-format overload.
        public void Error(string message)
        {
            Trace(TraceEventType.Error, message);
        }

        public void Error(string format, params object[] args)
        {
            Trace(TraceEventType.Error, StringUtil.Format(format, args));
        }

        // Do not remove the non-format overload.
        public void Warning(string message)
        {
            Trace(TraceEventType.Warning, message);
        }

        public void Warning(string format, params object[] args)
        {
            Trace(TraceEventType.Warning, StringUtil.Format(format, args));
        }

        // Do not remove the non-format overload.
        public void Verbose(string message)
        {
            Trace(TraceEventType.Verbose, message);
        }

        public void Verbose(string format, params object[] args)
        {
            Trace(TraceEventType.Verbose, StringUtil.Format(format, args));
        }

        public void Verbose(object item)
        {
            string json = JsonConvert.SerializeObject(item, Formatting.Indented);
            Trace(TraceEventType.Verbose, json);
        }

        public void Entering([CallerMemberName] string name = "")
        {
            Trace(TraceEventType.Verbose, $"Entering {name}");
        }

        public void Leaving([CallerMemberName] string name = "")
        {
            Trace(TraceEventType.Verbose, $"Leaving {name}");
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Trace(TraceEventType eventType, string message)
        {
            ArgUtil.NotNull(_traceSource, nameof(_traceSource));
            _traceSource.TraceEvent(
                eventType: eventType,
                id: 0,
                message: _secretMasker.MaskSecrets(message));
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                _traceSource.Flush();
                _traceSource.Close();
            }
        }
    }
}
