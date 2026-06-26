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

﻿using System.ComponentModel;
using System.Net.Http;
using System.Threading.Tasks;

namespace GitHub.Services.WebApi
{
    //This class should be used by callers of derivatives of HttpClientBase to deal with
    //getting a proper exception from a task, when you want to get a synchronous result
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class TaskExtensions
    {
        /// <summary>
        /// Blocks until the task has completed, throwing the remote exception if one was raised.
        /// </summary>
        /// <param name="task">The task to await</param>
        [AsyncFixer.BlockCaller]
        public static void SyncResult(this Task task)
        {
            // NOTE: GetResult() on TaskAwaiter uses ExceptionDispatchInfo.Throw if there 
            // is an exception, which preserves the original call stack and does not use 
            // AggregateException (unless explicitly thrown by the caller).
            task.GetAwaiter().GetResult();
        }

        /// <summary>
        /// Blocks until the task has completed, returning the result or throwing the remote exception if one was raised.
        /// </summary>
        /// <typeparam name="T">The type for the result</typeparam>
        /// <param name="task">The task to await</param>
        /// <returns>The result of the task</returns>
        [AsyncFixer.BlockCaller]
        public static T SyncResult<T>(this Task<T> task)
        {
            // NOTE: GetResult() on TaskAwaiter uses ExceptionDispatchInfo.Throw if there 
            // is an exception, which preserves the original call stack and does not use 
            // AggregateException (unless explicitly thrown by the caller).
            return task.GetAwaiter().GetResult();
        }

        /// <summary>
        /// Blocks until the task has completed, returning the result or throwing the remote exception if one was raised.
        /// </summary>
        /// <param name="task">The task to await</param>
        /// <returns>The result of the task</returns>
        [AsyncFixer.BlockCaller]
        public static HttpResponseMessage SyncResult(this Task<HttpResponseMessage> task)
        {
            // NOTE: This is effectively the same as <see cref="TaskExtensions.SyncResult(Task{T})"/>, 
            // but currently remains to support binary compatibility.

            // NOTE: GetResult() on TaskAwaiter uses ExceptionDispatchInfo.Throw if there 
            // is an exception, which preserves the original call stack and does not use 
            // AggregateException (unless explicitly thrown by the caller).
            return task.GetAwaiter().GetResult();
        }
    }
}

namespace AsyncFixer
{
    [System.AttributeUsage(System.AttributeTargets.Method)]
    public class BlockCaller : System.Attribute
    { }
}

