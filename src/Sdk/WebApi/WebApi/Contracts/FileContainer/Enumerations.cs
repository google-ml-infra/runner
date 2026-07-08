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
using System.Runtime.Serialization;

namespace GitHub.Services.FileContainer
{
    /// <summary>
    /// Options a container can have.
    /// </summary>
    [Flags]
    [DataContract]
    public enum ContainerOptions
    {
        /// <summary>
        /// No option.
        /// </summary>
        [EnumMember]
        None = 0,

        ///// <summary>
        ///// Encrypts content of the container.
        ///// </summary>
        //EncryptContent = 1
    }

    /// <summary>
    /// Type of a container item.
    /// </summary>
    [DataContract]
    public enum ContainerItemType
    {
        /// <summary>
        /// Any item type.
        /// </summary>
        [EnumMember]
        Any = 0,

        /// <summary>
        /// Item is a folder which can have child items.
        /// </summary>
        [EnumMember]
        Folder = 1,

        /// <summary>
        /// Item is a file which is stored in the file service.
        /// </summary>
        [EnumMember]
        File = 2,
    }

    /// <summary>
    /// Status of a container item.
    /// </summary>
    [DataContract]
    public enum ContainerItemStatus
    {
        /// <summary>
        /// Item is created.
        /// </summary>
        [EnumMember]
        Created = 1,

        /// <summary>
        /// Item is a file pending for upload.
        /// </summary>
        [EnumMember]
        PendingUpload = 2
    }
}
