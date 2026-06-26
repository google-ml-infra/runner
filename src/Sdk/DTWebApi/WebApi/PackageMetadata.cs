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
using System.ComponentModel;
using System.Runtime.Serialization;

namespace GitHub.DistributedTask.WebApi
{
    /// <summary>
    /// Represents a downloadable package.
    /// </summary>
    [DataContract]
    public class PackageMetadata
    {
        /// <summary>
        /// The type of package (e.g. "agent")
        /// </summary>
        [DataMember]
        public String Type
        {
            get;
            set;
        }

        /// <summary>
        /// The platform (win7, linux, etc.)
        /// </summary>
        [DataMember]
        public String Platform
        {
            get;
            set;
        }

        /// <summary>
        /// The date the package was created
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public DateTime CreatedOn
        {
            get;
            set;
        }

        /// <summary>
        /// The package version.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public PackageVersion Version
        {
            get;
            set;
        }

        /// <summary>
        /// A direct link to download the package.
        /// </summary>
        [DataMember]
        public String DownloadUrl
        {
            get;
            set;
        }

        /// <summary>
        /// File ID in file service
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Int32? FileId
        {
            get;
            set;
        }

        /// <summary>
        /// Auth token to download the package
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public String Token
        {
            get;
            set;
        }

        /// <summary>
        /// SHA256 hash
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public String HashValue
        {
            get;
            set;
        }

        /// <summary>
        /// A link to documentation
        /// </summary>
        [DataMember]
        public String InfoUrl
        {
            get;
            set;
        }

        /// <summary>
        /// The UI uses this to display instructions, e.g. "unzip MyAgent.zip"
        /// </summary>
        [DataMember]
        public String Filename
        {
            get;
            set;
        }

        /// <summary>
        /// A set of trimmed down packages:
        /// - the package without 'externals'
        /// - the package without 'dotnet runtime'
        /// - the package without 'dotnet runtime' and 'externals'
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public List<TrimmedPackageMetadata> TrimmedPackages
        {
            get;
            set;
        }
    }

    [DataContract]
    public class TrimmedPackageMetadata
    {
        [DataMember(EmitDefaultValue = false)]
        public string HashValue { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string DownloadUrl { get; set; }

        public Dictionary<string, string> TrimmedContents
        {
            get
            {
                if (m_trimmedContents == null)
                {
                    m_trimmedContents = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                }
                return m_trimmedContents;
            }
        }

        [DataMember(Name = "TrimmedContents", EmitDefaultValue = false)]
        private Dictionary<string, string> m_trimmedContents;
    }
}
