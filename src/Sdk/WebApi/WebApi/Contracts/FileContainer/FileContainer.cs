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
using System.ComponentModel;
using System.Runtime.Serialization;

namespace GitHub.Services.FileContainer
{
    /// <summary>
    /// Represents a container that encapsulates a hierarchical file system.
    /// </summary>
    [DataContract]
    public class FileContainer
    {
        /// <summary>
        /// Id.
        /// </summary>
        [DataMember(IsRequired = true)]
        public Int64 Id { get; [EditorBrowsable(EditorBrowsableState.Never)] set; }

        /// <summary>
        /// Project Id.
        /// </summary>
        [DataMember(IsRequired = false)]
        public Guid ScopeIdentifier { get; [EditorBrowsable(EditorBrowsableState.Never)] set; }

        /// <summary>
        /// Uri of the artifact associated with the container.
        /// </summary>
        [DataMember(IsRequired = true)]
        public Uri ArtifactUri { get; [EditorBrowsable(EditorBrowsableState.Never)] set; }

        /// <summary>
        /// Security token of the artifact associated with the container.
        /// </summary>
        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        public String SecurityToken { get; [EditorBrowsable(EditorBrowsableState.Never)] set; }

        /// <summary>
        /// Name.
        /// </summary>
        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        public String Name { get; [EditorBrowsable(EditorBrowsableState.Never)] set; }

        /// <summary>
        /// Description.
        /// </summary>
        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        public String Description { get; [EditorBrowsable(EditorBrowsableState.Never)] set; }

        /// <summary>
        /// Total size of the files in bytes.
        /// </summary>
        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        public Int64 Size { get; [EditorBrowsable(EditorBrowsableState.Never)] set; }

        /// <summary>
        /// Options the container can have.
        /// </summary>
        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        public ContainerOptions Options { get; [EditorBrowsable(EditorBrowsableState.Never)] set; }

        /// <summary>
        /// Identifier of the optional encryption key.
        /// </summary>
        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        public Guid SigningKeyId { get; [EditorBrowsable(EditorBrowsableState.Never)] set; }

        /// <summary>
        /// Owner.
        /// </summary>
        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        public Guid CreatedBy { get; [EditorBrowsable(EditorBrowsableState.Never)] set; }

        /// <summary>
        /// Creation date.
        /// </summary>
        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        public DateTime DateCreated { get; [EditorBrowsable(EditorBrowsableState.Never)] set; }

        /// <summary>
        /// Location of the item resource.
        /// </summary>
        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        public String ItemLocation { get; [EditorBrowsable(EditorBrowsableState.Never)] set; }

        /// <summary>
        /// Download Url for the content of this item.
        /// </summary>
        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        public String ContentLocation { get; [EditorBrowsable(EditorBrowsableState.Never)] set; }

        /// <summary>
        /// ItemStore Locator for this container.
        /// </summary>
        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        public String LocatorPath { get; [EditorBrowsable(EditorBrowsableState.Never)] set; }

        public override bool Equals(object obj)
        {
            FileContainer other = obj as FileContainer;

            if (other == null)
            {
                return false;
            }

            return this.ArtifactUri == other.ArtifactUri &&
                    this.Description == other.Description &&
                    this.Id == other.Id &&
                    this.Name == other.Name &&
                    this.ScopeIdentifier == other.ScopeIdentifier;
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
    }
}
