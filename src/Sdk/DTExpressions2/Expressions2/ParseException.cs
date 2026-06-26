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

﻿using System;
using System.ComponentModel;
using GitHub.DistributedTask.Expressions2.Tokens;

namespace GitHub.DistributedTask.Expressions2
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public sealed class ParseException : ExpressionException
    {
        internal ParseException(ParseExceptionKind kind, Token token, String expression)
            : base(secretMasker: null, message: String.Empty)
        {
            Expression = expression;
            Kind = kind;
            RawToken = token?.RawValue;
            TokenIndex = token?.Index ?? 0;
            String description;
            switch (kind)
            {
                case ParseExceptionKind.ExceededMaxDepth:
                    description = $"Exceeded max expression depth {ExpressionConstants.MaxDepth}";
                    break;
                case ParseExceptionKind.ExceededMaxLength:
                    description = $"Exceeded max expression length {ExpressionConstants.MaxLength}";
                    break;
                case ParseExceptionKind.TooFewParameters:
                    description = "Too few parameters supplied";
                    break;
                case ParseExceptionKind.TooManyParameters:
                    description = "Too many parameters supplied";
                    break;
                case ParseExceptionKind.EvenParameters:
                    description = "Even number of parameters supplied, requires an odd number of parameters";
                    break;
                case ParseExceptionKind.UnexpectedEndOfExpression:
                    description = "Unexpected end of expression";
                    break;
                case ParseExceptionKind.UnexpectedSymbol:
                    description = "Unexpected symbol";
                    break;
                case ParseExceptionKind.UnrecognizedFunction:
                    description = "Unrecognized function";
                    break;
                case ParseExceptionKind.UnrecognizedNamedValue:
                    description = "Unrecognized named-value";
                    break;
                default: // Should never reach here.
                    throw new Exception($"Unexpected parse exception kind '{kind}'.");
            }

            if (token == null)
            {
                Message = description;
            }
            else
            {
                Message = $"{description}: '{RawToken}'. Located at position {TokenIndex + 1} within expression: {Expression}";
            }
        }

        internal String Expression { get; }

        internal ParseExceptionKind Kind { get; }

        internal String RawToken { get; }

        internal Int32 TokenIndex { get; }

        public sealed override String Message { get; }
    }
}
