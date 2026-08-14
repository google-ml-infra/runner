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

using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using GitHub.DistributedTask.WebApi;

namespace GitHub.DistributedTask.Pipelines
{
    [DataContract]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public sealed class JobResources
    {
        /// <summary>
        /// Gets the collection of containers associated with the current job
        /// </summary>
        public List<ContainerResource> Containers
        {
            get
            {
                if (m_containers == null)
                {
                    m_containers = new List<ContainerResource>();
                }
                return m_containers;
            }
        }

        /// <summary>
        /// Gets the collection of endpoints associated with the current job
        /// </summary>
        public List<ServiceEndpoint> Endpoints
        {
            get
            {
                if (m_endpoints == null)
                {
                    m_endpoints = new List<ServiceEndpoint>();
                }
                return m_endpoints;
            }
        }

        /// <summary>
        /// Gets the collection of repositories associated with the current job
        /// </summary>
        public List<RepositoryResource> Repositories
        {
            get
            {
                if (m_repositories == null)
                {
                    m_repositories = new List<RepositoryResource>();
                }
                return m_repositories;
            }
        }

        [OnSerializing]
        private void OnSerializing(StreamingContext context)
        {
            if (m_containers?.Count == 0)
            {
                m_containers = null;
            }

            if (m_endpoints?.Count == 0)
            {
                m_endpoints = null;
            }

            if (m_repositories?.Count == 0)
            {
                m_repositories = null;
            }
        }

        [DataMember(Name = "Containers", EmitDefaultValue = false)]
        private List<ContainerResource> m_containers;

        [DataMember(Name = "Endpoints", EmitDefaultValue = false)]
        private List<ServiceEndpoint> m_endpoints;

        [DataMember(Name = "Repositories", EmitDefaultValue = false)]
        private List<RepositoryResource> m_repositories;
    }
}
