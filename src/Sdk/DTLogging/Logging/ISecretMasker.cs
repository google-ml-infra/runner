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
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace GitHub.DistributedTask.Logging
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface ISecretMasker
    {
        void AddRegex(String pattern);
        void AddValue(String value);
        void AddValueEncoder(ValueEncoder encoder);
        ISecretMasker Clone();
        String MaskSecrets(String input);
        public event EventHandler<NewSecretEventArgs> NewSecretAdded;
    }

    public abstract class NewSecretEventArgs : EventArgs
    {
        public abstract String Type { get; }
    }

    [DataContract]
    public sealed class NewRegexSecretEventArgs : NewSecretEventArgs
    {
        [DataMember]
        public override String Type => "regex";

        public NewRegexSecretEventArgs(String pattern)
        {
            Pattern = pattern;
        }

        [DataMember]
        public String Pattern { get; private set; }
    }

    [DataContract]
    public sealed class NewVariableSecretEventArgs : NewSecretEventArgs
    {
        [DataMember]
        public override String Type => "variable";

        public NewVariableSecretEventArgs(List<string> values)
        {
            Values.AddRange(values);
        }

        [DataMember]
        public List<string> Values { get; private set; } = new List<string>();
    }
}
