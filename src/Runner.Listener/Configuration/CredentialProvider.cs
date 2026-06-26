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
using GitHub.Runner.Common;
using GitHub.Runner.Sdk;
using GitHub.Services.Common;
using GitHub.Services.OAuth;

namespace GitHub.Runner.Listener.Configuration
{
    public interface ICredentialProvider
    {
        Boolean RequireInteractive { get; }
        CredentialData CredentialData { get; set; }
        VssCredentials GetVssCredentials(IHostContext context, bool allowAuthUrlV2);
        void EnsureCredential(IHostContext context, CommandSettings command, string serverUrl);
    }

    public abstract class CredentialProvider : ICredentialProvider
    {
        public CredentialProvider(string scheme)
        {
            CredentialData = new CredentialData();
            CredentialData.Scheme = scheme;
        }

        public virtual Boolean RequireInteractive => false;
        public CredentialData CredentialData { get; set; }

        public abstract VssCredentials GetVssCredentials(IHostContext context, bool allowAuthUrlV2);
        public abstract void EnsureCredential(IHostContext context, CommandSettings command, string serverUrl);
    }

    public sealed class OAuthAccessTokenCredential : CredentialProvider
    {
        public OAuthAccessTokenCredential() : base(Constants.Configuration.OAuthAccessToken) { }

        public override VssCredentials GetVssCredentials(IHostContext context, bool allowAuthUrlV2)
        {
            ArgUtil.NotNull(context, nameof(context));
            Tracing trace = context.GetTrace(nameof(OAuthAccessTokenCredential));
            trace.Info(nameof(GetVssCredentials));
            ArgUtil.NotNull(CredentialData, nameof(CredentialData));
            string token;
            if (!CredentialData.Data.TryGetValue(Constants.Runner.CommandLine.Args.Token, out token))
            {
                token = null;
            }

            ArgUtil.NotNullOrEmpty(token, nameof(token));

            trace.Info("token retrieved: {0} chars", token.Length);
            VssCredentials creds = new(new VssOAuthAccessTokenCredential(token), CredentialPromptType.DoNotPrompt);
            trace.Info("cred created");

            return creds;
        }

        public override void EnsureCredential(IHostContext context, CommandSettings command, string serverUrl)
        {
            ArgUtil.NotNull(context, nameof(context));
            Tracing trace = context.GetTrace(nameof(OAuthAccessTokenCredential));
            trace.Info(nameof(EnsureCredential));
            ArgUtil.NotNull(command, nameof(command));
            CredentialData.Data[Constants.Runner.CommandLine.Args.Token] = command.GetToken();
        }
    }
}
