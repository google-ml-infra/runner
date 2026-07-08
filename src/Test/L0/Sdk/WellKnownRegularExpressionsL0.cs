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

using GitHub.DistributedTask.Pipelines.Expressions;
using Xunit;

namespace GitHub.Runner.Common.Tests.Sdk
{
    public sealed class WellKnownRegularExpressionsL0
    {
        [Fact]
        [Trait("Level", "L0")]
        [Trait("Category", "Sdk")]
        public void SHA1_Key_Returns_CommitHash_Regex()
        {
            var regex = WellKnownRegularExpressions.GetRegex(WellKnownRegularExpressions.SHA1);

            Assert.NotNull(regex);
        }

        [Fact]
        [Trait("Level", "L0")]
        [Trait("Category", "Sdk")]
        public void CommitHash_Key_Returns_CommitHash_Regex()
        {
            var regex = WellKnownRegularExpressions.GetRegex(WellKnownRegularExpressions.CommitHash);

            Assert.NotNull(regex);
        }

        [Fact]
        [Trait("Level", "L0")]
        [Trait("Category", "Sdk")]
        public void SHA1_And_CommitHash_Return_Same_Regex()
        {
            var sha1Regex = WellKnownRegularExpressions.GetRegex(WellKnownRegularExpressions.SHA1);
            var commitHashRegex = WellKnownRegularExpressions.GetRegex(WellKnownRegularExpressions.CommitHash);

            Assert.Same(sha1Regex, commitHashRegex);
        }

        [Fact]
        [Trait("Level", "L0")]
        [Trait("Category", "Sdk")]
        public void Matches_40_Char_Hex()
        {
            var regex = WellKnownRegularExpressions.GetRegex(WellKnownRegularExpressions.CommitHash);

            Assert.Matches(regex.Value, new string('a', 40));
        }

        [Fact]
        [Trait("Level", "L0")]
        [Trait("Category", "Sdk")]
        public void Matches_64_Char_Hex()
        {
            var regex = WellKnownRegularExpressions.GetRegex(WellKnownRegularExpressions.CommitHash);

            Assert.Matches(regex.Value, new string('a', 64));
        }

        [Fact]
        [Trait("Level", "L0")]
        [Trait("Category", "Sdk")]
        public void Does_Not_Match_63_Char_Hex()
        {
            var regex = WellKnownRegularExpressions.GetRegex(WellKnownRegularExpressions.CommitHash);

            Assert.DoesNotMatch(regex.Value, new string('a', 63));
        }

        [Fact]
        [Trait("Level", "L0")]
        [Trait("Category", "Sdk")]
        public void Does_Not_Match_65_Char_Hex()
        {
            var regex = WellKnownRegularExpressions.GetRegex(WellKnownRegularExpressions.CommitHash);

            Assert.DoesNotMatch(regex.Value, new string('a', 65));
        }

        [Fact]
        [Trait("Level", "L0")]
        [Trait("Category", "Sdk")]
        public void Matches_Mixed_Case_64_Char()
        {
            var regex = WellKnownRegularExpressions.GetRegex(WellKnownRegularExpressions.CommitHash);
            var value = new string('A', 32) + new string('b', 32);

            Assert.Matches(regex.Value, value);
        }

        [Fact]
        [Trait("Level", "L0")]
        [Trait("Category", "Sdk")]
        public void Unknown_Key_Returns_Null()
        {
            var regex = WellKnownRegularExpressions.GetRegex("UnknownType");

            Assert.Null(regex);
        }
    }
}
