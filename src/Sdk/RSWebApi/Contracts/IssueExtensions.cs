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

using GitHub.DistributedTask.WebApi;

namespace Sdk.RSWebApi.Contracts
{
    public static class IssueExtensions
    {
        public static Annotation? ToAnnotation(this Issue issue)
        {
            var issueMessage = issue.Message;
            if (string.IsNullOrWhiteSpace(issueMessage))
            {
                if (!issue.Data.TryGetValue(RunIssueKeys.Message, out issueMessage) || string.IsNullOrWhiteSpace(issueMessage))
                {
                    return null;
                }
            }

            var annotationLevel = GetAnnotationLevel(issue.Type);
            var path = GetFilePath(issue);
            var lineNumber = GetAnnotationNumber(issue, RunIssueKeys.Line) ?? 0;
            var endLineNumber = GetAnnotationNumber(issue, RunIssueKeys.EndLine) ?? lineNumber;
            var columnNumber = GetAnnotationNumber(issue, RunIssueKeys.Col) ?? 0;
            var endColumnNumber = GetAnnotationNumber(issue, RunIssueKeys.EndColumn) ?? columnNumber;
            var logLineNumber = GetAnnotationNumber(issue, RunIssueKeys.LogLineNumber) ?? 0;
            var stepNumber = GetAnnotationNumber(issue, RunIssueKeys.StepNumber) ?? 0;
            var title = GetAnnotationField(issue, RunIssueKeys.Title);

            if (path == null && lineNumber == 0 && logLineNumber != 0)
            {
                lineNumber = logLineNumber;
                endLineNumber = logLineNumber;
            }

            return new Annotation
            {
                Level = annotationLevel,
                Message = issueMessage,
                Title = title,
                Path = path,
                StartLine = lineNumber,
                EndLine = endLineNumber,
                StartColumn = columnNumber,
                EndColumn = endColumnNumber,
                StepNumber = stepNumber,
                IsInfrastructureIssue = issue.IsInfrastructureIssue ?? false
            };
        }

        private static AnnotationLevel GetAnnotationLevel(IssueType issueType)
        {
            switch (issueType)
            {
                case IssueType.Error:
                    return AnnotationLevel.FAILURE;
                case IssueType.Warning:
                    return AnnotationLevel.WARNING;
                case IssueType.Notice:
                    return AnnotationLevel.NOTICE;
                default:
                    return AnnotationLevel.UNKNOWN;
            }
        }

        private static int? GetAnnotationNumber(Issue issue, string key)
        {
            if (issue.Data.TryGetValue(key, out var numberString) &&
                int.TryParse(numberString, out var number))
            {
                return number;
            }

            return null;
        }

        private static string GetAnnotationField(Issue issue, string key)
        {
            if (issue.Data.TryGetValue(key, out var value))
            {
                return value;
            }

            return null;
        }

        private static string GetFilePath(Issue issue)
        {
            if (issue.Data.TryGetValue(RunIssueKeys.File, out var path) &&
                !string.IsNullOrWhiteSpace(path))
            {
                return path;
            }

            return null;
        }
    }
}
