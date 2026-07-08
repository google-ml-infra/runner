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

﻿using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace GitHub.DistributedTask.Pipelines
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class RepositoryPropertyNames
    {
        public static readonly String Id = "id";
        public static readonly String Type = "type";
        public static readonly String Url = "url";
        public static readonly String Version = "version";
    }

    /// <summary>
    /// Provides a data contract for a repository resource referenced by a pipeline.
    /// </summary>
    [DataContract]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public sealed class RepositoryResource : Resource
    {
        /// <summary>
        /// Initializes a new <c>RepositoryReference</c> instance with default values.
        /// </summary>
        public RepositoryResource()
        {
        }

        private RepositoryResource(RepositoryResource referenceToCopy)
            : base(referenceToCopy)
        {
        }

        /// <summary>
        /// Gets or sets a unique identifier for this repository.
        /// </summary>
        public String Id
        {
            get
            {
                return this.Properties.Get<String>(RepositoryPropertyNames.Id);
            }
            set
            {
                this.Properties.Set(RepositoryPropertyNames.Id, value);
            }
        }

        /// <summary>
        /// Gets or sets the type of repository.
        /// </summary>
        public String Type
        {
            get
            {
                return this.Properties.Get<String>(RepositoryPropertyNames.Type);
            }
            set
            {
                this.Properties.Set(RepositoryPropertyNames.Type, value);
            }
        }

        /// <summary>
        /// Gets or sets the url of the repository.
        /// </summary>
        public Uri Url
        {
            get
            {
                return this.Properties.Get<Uri>(RepositoryPropertyNames.Url);
            }
            set
            {
                this.Properties.Set(RepositoryPropertyNames.Url, value);
            }
        }

        /// <summary>
        /// Gets or sets the version of the repository.
        /// </summary>
        public String Version
        {
            get
            {
                return this.Properties.Get<String>(RepositoryPropertyNames.Version);
            }
            set
            {
                this.Properties.Set(RepositoryPropertyNames.Version, value);
            }
        }

        /// <summary>
        /// Creates a clone of the current repository instance.
        /// </summary>
        /// <returns>A new <c>RepositoryReference</c> instance which is a copy of the current instance</returns>
        public RepositoryResource Clone()
        {
            return new RepositoryResource(this);
        }
    }
}
