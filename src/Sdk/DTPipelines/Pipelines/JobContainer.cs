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

namespace GitHub.DistributedTask.Pipelines
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public sealed class JobContainer
    {
        /// <summary>
        /// Generated unique alias
        /// </summary>
        public String Alias { get; } = Guid.NewGuid().ToString("N");

        /// <summary>
        /// Gets or sets the environment which is provided to the container.
        /// </summary>
        public IDictionary<String, String> Environment
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the container image name.
        /// </summary>
        public String Image
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the options used for the container instance.
        /// </summary>
        public String Options
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the container entrypoint override.
        /// </summary>
        public String Entrypoint
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the container command and args (after the image name).
        /// </summary>
        public String Command
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the volumes which are mounted into the container.
        /// </summary>
        public IList<String> Volumes
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the ports which are exposed on the container.
        /// </summary>
        public IList<String> Ports
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the credentials used for pulling the container iamge.
        /// </summary>
        public ContainerRegistryCredentials Credentials
        {
            get;
            set;
        }
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public sealed class ContainerRegistryCredentials
    {
        /// <summary>
        /// Gets or sets the user to authenticate to a registry with
        /// </summary>
        public String Username
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the password to authenticate to a registry with
        /// </summary>
        public String Password
        {
            get;
            set;
        }
    }
}
