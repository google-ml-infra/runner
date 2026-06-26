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

﻿// Microsoft Confidential
// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.ComponentModel;

namespace GitHub.Services.Common
{
    /// <summary>
    /// Base class for constant generation.  Allows types/fields to be generated
    /// with an alternate name.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public abstract class GenerateConstantAttributeBase : Attribute
    {
        protected GenerateConstantAttributeBase(string alternateName = null)
        {
            AlternateName = alternateName;
        }

        public string AlternateName { get; private set; }
    }

    /// <summary>
    /// Can be applied to a const/readonly-static field of a class/enum/struct, but is 
    /// only used when the containing type has the 'GenerateSpecificConstants' attribute applied.  
    /// This allows the developer to specify exactly what constants to include out of the containing type.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class GenerateConstantAttribute : GenerateConstantAttributeBase
    {
        public GenerateConstantAttribute(string alternateName = null)
            : base(alternateName)
        {
        }
    }

    /// <summary>
    /// Applied to any enum/class/struct.  Causes the constants generator to create javascript constants 
    /// for all const/readonly-static fields contained by the type.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Enum | AttributeTargets.Struct)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class GenerateAllConstantsAttribute : GenerateConstantAttribute
    {
        public GenerateAllConstantsAttribute(string alternateName = null)
            : base(alternateName)
        {
        }
    }

    /// <summary>
    /// Applied to any enum/class/struct.  Causes the constants generator to create javascript constants at runtime
    /// for the type for any member constants/enumerated values that are tagged with the 'GenerateConstant' attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Enum | AttributeTargets.Struct)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class GenerateSpecificConstantsAttribute : GenerateConstantAttribute
    {
        public GenerateSpecificConstantsAttribute(string alternateName = null)
            : base(alternateName)
        {
        }
    }

    /// <summary>
    /// Applied to a class that represents a data model which is serialized to javascript. 
    /// This attribute controls how TypeScript interfaces are generated for the class that
    /// this is applied to.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Interface)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class GenerateInterfaceAttribute : GenerateConstantAttributeBase
    {
        public GenerateInterfaceAttribute()
            : this(true)
        {
        }

        public GenerateInterfaceAttribute(string alternateName)
            : base(alternateName)
        {
            GenerateInterface = true;
        }

        public GenerateInterfaceAttribute(bool generateInterface)
            : base()
        {
            GenerateInterface = generateInterface;
        }

        /// <summary>
        /// Whether or not to generate a typescript interface for this type
        /// </summary>
        public bool GenerateInterface { get; set; }
    }
}
