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

#nullable enable

using System;
using System.Linq;
using GitHub.Actions.WorkflowParser.ObjectTemplating;

namespace GitHub.Actions.WorkflowParser.Conversion
{
    internal static class PermissionsHelper
    {
        /// <summary>
        /// Validates permissions requested in a reusable workflow do not exceed allowed permissions
        /// </summary>
        /// <param name="context">The template context</param>
        /// <param name="workflowJob">The reusable workflow job</param>
        /// <param name="embeddedJob">(Optional) Used when formatting errors related to an embedded job within the reusable workflow</param>
        /// <param name="requested">The permissions within the reusable workflow file. These may be defined either at the root of the file, or may be defined on a job within the file.</param>
        /// <param name="explicitMax">(Optional) The max permissions explicitly allowed by the caller</param>
        /// <param name="permissionsPolicy">The default permissions policy</param>
        /// <param name="isTrusted">Indicates whether the reusable workflow exists within the same trust boundary (e.g. enterprise/organization) as a the root workflow</param>
        internal static void ValidateEmbeddedPermissions(
            TemplateContext context,
            ReusableWorkflowJob workflowJob,
            IJob? embeddedJob,
            Permissions requested,
            Permissions? explicitMax,
            string permissionsPolicy,
            bool isTrusted)
        {
            if (requested == null)
            {
                return;
            }

            var effectiveMax = explicitMax ?? CreatePermissionsFromPolicy(context, permissionsPolicy, includeIdToken: isTrusted, includeModels: context.GetFeatures().AllowModelsPermission, includeVulnerabilityAlerts: context.GetFeatures().AllowVulnerabilityAlertsPermission);
            
            if (requested.ViolatesMaxPermissions(effectiveMax, out var permissionLevelViolations))
            {
                var requestedStr = string.Join(", ", permissionLevelViolations.Select(x => x.RequestedPermissionLevelString()));
                var allowedStr = string.Join(", ", permissionLevelViolations.Select(x => x.AllowedPermissionLevelString()));
                if (embeddedJob != null)
                {
                    context.Error(workflowJob.Id, $"Error calling workflow '{workflowJob.Ref}'. The nested job '{embeddedJob.Id!.Value}' is requesting '{requestedStr}', but is only allowed '{allowedStr}'.");
                }
                else
                {
                    context.Error(workflowJob.Id, $"Error calling workflow '{workflowJob.Ref}'. The workflow is requesting '{requestedStr}', but is only allowed '{allowedStr}'.");
                }
            }
        }

        /// <summary>
        /// Creates permissions based on policy
        /// </summary>
        /// <param name="context">The template context</param>
        /// <param name="permissionsPolicy">The permissions policy</param>
        /// <param name="includeIdToken">Indicates whether the permissions should include an ID token</param>
        private static Permissions CreatePermissionsFromPolicy(
            TemplateContext context,
            string permissionsPolicy,
            bool includeIdToken,
            bool includeModels,
            bool includeVulnerabilityAlerts)
        {
            switch (permissionsPolicy)
            {
                case WorkflowConstants.PermissionsPolicy.LimitedRead:
                    return new Permissions(PermissionLevel.NoAccess, includeIdToken: false, includeAttestations: false, includeModels: false, includeVulnerabilityAlerts: false)
                    {
                        Contents = PermissionLevel.Read,
                        Packages = PermissionLevel.Read,
                    };
                case WorkflowConstants.PermissionsPolicy.Write:
                    return new Permissions(PermissionLevel.Write, includeIdToken: includeIdToken, includeAttestations: true, includeModels: includeModels, includeVulnerabilityAlerts: includeVulnerabilityAlerts);
                default:
                    throw new ArgumentException($"Unexpected permission policy: '{permissionsPolicy}'");
            }
        }
    }
}