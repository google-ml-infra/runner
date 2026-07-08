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

﻿using GitHub.Actions.RunService.WebApi;
using Xunit;

namespace GitHub.Actions.RunService.WebApi.Tests;

public sealed class RunServiceHttpClientL0
{
    [Fact]
    public void Truncate()
    {
        TestTruncate(string.Empty.PadLeft(199, 'a'), string.Empty.PadLeft(199, 'a'));
        TestTruncate(string.Empty.PadLeft(200, 'a'), string.Empty.PadLeft(200, 'a'));
        TestTruncate(string.Empty.PadLeft(201, 'a'), string.Empty.PadLeft(200, 'a') + "[truncated]");
    }

    private void TestTruncate(string errorBody, string expected)
    {
        Assert.Equal(expected, RunServiceHttpClient.Truncate(errorBody));
    }
}
