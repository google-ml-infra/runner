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

﻿using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GitHub.Services.Common
{
    public static class HashAlgorithmExtensions
    {
        public static async Task<byte[]> ComputeHashAsync(this HashAlgorithm hashAlg, Stream inputStream)
        {
            byte[] buffer = new byte[4096];

            while (true)
            {
                int read = await inputStream.ReadAsync(buffer, 0, buffer.Length);
                if (read == 0)
                    break;

                hashAlg.TransformBlock(buffer, 0, read, null, 0);
            }

            hashAlg.TransformFinalBlock(buffer, 0, 0);
            return hashAlg.Hash;
        }
    }
}
