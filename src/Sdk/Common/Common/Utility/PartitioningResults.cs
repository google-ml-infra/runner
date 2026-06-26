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

﻿using System.Collections.Generic;

namespace GitHub.Services.Common
{
    /// <summary>
    /// Contains results from two-way variant of EnuemrableExtensions.Partition()
    /// </summary>
    /// <typeparam name="T">The type of the elements in the contained lists.</typeparam>
    public sealed class PartitionResults<T>
    {
        public List<T> MatchingPartition { get; } = new List<T>();

        public List<T> NonMatchingPartition { get; } = new List<T>();
    }

    /// <summary>
    /// Contains results from multi-partitioning variant of EnuemrableExtensions.Partition()
    /// </summary>
    /// <typeparam name="T">The type of the elements in the contained lists.</typeparam>
    public sealed class MultiPartitionResults<T>
    {
        public List<List<T>> MatchingPartitions { get; } = new List<List<T>>();

        public List<T> NonMatchingPartition { get; } = new List<T>();
    }
}
