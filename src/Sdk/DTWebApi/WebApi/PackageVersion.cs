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
using System.Globalization;
using System.Runtime.Serialization;

namespace GitHub.DistributedTask.WebApi
{
    [DataContract]
    public class PackageVersion : IComparable<PackageVersion>, IEquatable<PackageVersion>
    {
        public PackageVersion()
        {
        }

        public PackageVersion(String version)
        {
            Int32 major, minor, patch;
            String semanticVersion;

            VersionParser.ParseVersion(version, out major, out minor, out patch, out semanticVersion);
            Major = major;
            Minor = minor;
            Patch = patch;
        }

        public static Boolean TryParse(String versionStr, out PackageVersion version)
        {
            version = null;

            try
            {
                version = new PackageVersion(versionStr);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private PackageVersion(PackageVersion versionToClone)
        {
            this.Major = versionToClone.Major;
            this.Minor = versionToClone.Minor;
            this.Patch = versionToClone.Patch;
        }

        [DataMember]
        public Int32 Major
        {
            get;
            set;
        }

        [DataMember]
        public Int32 Minor
        {
            get;
            set;
        }

        [DataMember]
        public Int32 Patch
        {
            get;
            set;
        }

        public PackageVersion Clone()
        {
            return new PackageVersion(this);
        }

        public static implicit operator String(PackageVersion version)
        {
            return version.ToString();
        }

        public override String ToString()
        {
            return String.Format(CultureInfo.InvariantCulture, "{0}.{1}.{2}", Major, Minor, Patch);
        }

        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }

        public Int32 CompareTo(PackageVersion other)
        {
            Int32 rc = Major.CompareTo(other.Major);
            if (rc == 0)
            {
                rc = Minor.CompareTo(other.Minor);
                if (rc == 0)
                {
                    rc = Patch.CompareTo(other.Patch);
                }
            }

            return rc;
        }

        public Boolean Equals(PackageVersion other)
        {
            return this.CompareTo(other) == 0;
        }
    }
}
