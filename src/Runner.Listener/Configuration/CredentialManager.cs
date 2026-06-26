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
using System.Collections.Generic;
using System.Runtime.Serialization;
using GitHub.Runner.Common;
using GitHub.Runner.Sdk;
using GitHub.Services.Common;
using GitHub.Services.OAuth;

namespace GitHub.Runner.Listener.Configuration
{
    // TODO: Refactor extension manager to enable using it from the agent process.
    [ServiceLocator(Default = typeof(CredentialManager))]
    public interface ICredentialManager : IRunnerService
    {
        ICredentialProvider GetCredentialProvider(string credType);
        VssCredentials LoadCredentials(bool allowAuthUrlV2);
    }

    public class CredentialManager : RunnerService, ICredentialManager
    {
        public static readonly Dictionary<string, Type> CredentialTypes = new(StringComparer.OrdinalIgnoreCase)
        {
            { Constants.Configuration.OAuth, typeof(OAuthCredential) },
            { Constants.Configuration.OAuthAccessToken, typeof(OAuthAccessTokenCredential) },
        };

        public ICredentialProvider GetCredentialProvider(string credType)
        {
            Trace.Info(nameof(GetCredentialProvider));
            Trace.Info("Creating type {0}", credType);

            if (!CredentialTypes.ContainsKey(credType))
            {
                throw new ArgumentException("Invalid Credential Type");
            }

            Trace.Info("Creating credential type: {0}", credType);
            var creds = Activator.CreateInstance(CredentialTypes[credType]) as ICredentialProvider;
            Trace.Verbose("Created credential type");
            return creds;
        }

        public VssCredentials LoadCredentials(bool allowAuthUrlV2)
        {
            IConfigurationStore store = HostContext.GetService<IConfigurationStore>();

            if (!store.HasCredentials())
            {
                throw new InvalidOperationException("Credentials not stored. Must reconfigure.");
            }

            CredentialData credData = store.GetCredentials();
            var migratedCred = store.GetMigratedCredentials();
            if (migratedCred != null &&
                migratedCred.Scheme == Constants.Configuration.OAuth)
            {
                credData = migratedCred;
            }

            ICredentialProvider credProv = GetCredentialProvider(credData.Scheme);
            credProv.CredentialData = credData;

            VssCredentials creds = credProv.GetVssCredentials(HostContext, allowAuthUrlV2);

            return creds;
        }
    }

    [DataContract]
    public sealed class GitHubRunnerRegisterToken
    {
        [DataMember(Name = "token")]
        public string Token { get; set; }

        [DataMember(Name = "expires_at")]
        public string ExpiresAt { get; set; }
    }

    [DataContract]
    public sealed class GitHubAuthResult
    {
        [DataMember(Name = "url")]
        public string TenantUrl { get; set; }

        [DataMember(Name = "token_schema")]
        public string TokenSchema { get; set; }

        [DataMember(Name = "token")]
        public string Token { get; set; }

        [DataMember(Name = "use_v2_flow")]
        public bool UseRunnerAdminFlow { get; set; }

        public VssCredentials ToVssCredentials()
        {
            ArgUtil.NotNullOrEmpty(TokenSchema, nameof(TokenSchema));
            ArgUtil.NotNullOrEmpty(Token, nameof(Token));

            if (string.Equals(TokenSchema, "OAuthAccessToken", StringComparison.OrdinalIgnoreCase))
            {
                return new VssCredentials(new VssOAuthAccessTokenCredential(Token), CredentialPromptType.DoNotPrompt);
            }
            else
            {
                throw new NotSupportedException($"Not supported token schema: {TokenSchema}");
            }
        }
    }
}
