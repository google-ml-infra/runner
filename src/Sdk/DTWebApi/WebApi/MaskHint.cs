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
using System.Runtime.Serialization;

namespace GitHub.DistributedTask.WebApi
{
    [DataContract]
    public class MaskHint
    {
        public MaskHint()
        {
        }

        private MaskHint(MaskHint maskHintToBeCloned)
        {
            this.Type = maskHintToBeCloned.Type;
            this.Value = maskHintToBeCloned.Value;
        }

        public MaskHint Clone()
        {
            return new MaskHint(this);
        }

        [DataMember]
        public MaskType Type
        {
            get;
            set;
        }

        [DataMember]
        public String Value
        {
            get;
            set;
        }

        public override Boolean Equals(Object obj)
        {
            var otherHint = obj as MaskHint;
            if (otherHint != null)
            {
                return this.Type == otherHint.Type && String.Equals(this.Value ?? String.Empty, otherHint.Value ?? String.Empty, StringComparison.Ordinal);
            }

            return false;
        }

        public override Int32 GetHashCode()
        {
            return this.Type.GetHashCode() ^ (this.Value ?? String.Empty).GetHashCode();
        }
    }
}
