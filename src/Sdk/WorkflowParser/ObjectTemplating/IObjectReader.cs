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

using System;
using GitHub.Actions.WorkflowParser.ObjectTemplating.Tokens;

namespace GitHub.Actions.WorkflowParser.ObjectTemplating
{
    /// <summary>
    /// Interface for reading a source object (or file).
    /// This interface is used by TemplateReader to build a TemplateToken DOM.
    /// </summary>
    internal interface IObjectReader
    {
        Boolean AllowLiteral(out LiteralToken token);

        Boolean AllowSequenceStart(out SequenceToken token);

        Boolean AllowSequenceEnd();

        Boolean AllowMappingStart(out MappingToken token);

        Boolean AllowMappingEnd();

        void ValidateStart();

        void ValidateEnd();
    }
}
