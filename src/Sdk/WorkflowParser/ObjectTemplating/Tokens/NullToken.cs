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
using System.Runtime.Serialization;
using GitHub.Actions.Expressions.Sdk;

namespace GitHub.Actions.WorkflowParser.ObjectTemplating.Tokens
{
    [DataContract]
    public sealed class NullToken : LiteralToken, INull
    {
        public NullToken(
            Int32? fileId,
            Int32? line,
            Int32? column)
            : base(TokenType.Null, fileId, line, column)
        {
        }

        public override TemplateToken Clone(Boolean omitSource)
        {
           return omitSource ? new NullToken(null, null, null) : new NullToken(FileId, Line, Column);
        }

        public override String ToString()
        {
           return String.Empty;
        }
    }
}