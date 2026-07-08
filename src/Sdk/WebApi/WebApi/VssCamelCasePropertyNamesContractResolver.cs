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
using Newtonsoft.Json.Serialization;

namespace GitHub.Services.WebApi
{
    internal class VssCamelCasePropertyNamesContractResolver : CamelCasePropertyNamesContractResolver
    {
        protected override JsonDictionaryContract CreateDictionaryContract(Type type)
        {
            // We need to preserve case for keys in the PropertiesCollection
            JsonDictionaryContract contract = base.CreateDictionaryContract(type);
            contract.DictionaryKeyResolver = (name) => name;
            return contract;
        }
    }

    internal class VssCamelCasePropertyNamesPreserveEnumsContractResolver : CamelCasePropertyNamesContractResolver
    {
        protected override JsonDictionaryContract CreateDictionaryContract(Type type)
        {
            // We need to preserve case for keys in the PropertiesCollection and optionally use integer values for enum keys
            JsonDictionaryContract contract = base.CreateDictionaryContract(type);

            Type keyType = contract.DictionaryKeyType;
            Boolean isEnumKey = keyType != null ? keyType.IsEnum : false;

            if (isEnumKey)
            {
                contract.DictionaryKeyResolver = (name) => ((int)Enum.Parse(keyType, name)).ToString();
            }
            else
            {
                contract.DictionaryKeyResolver = (name) => name;
            }

            return contract;
        }
    }
}
