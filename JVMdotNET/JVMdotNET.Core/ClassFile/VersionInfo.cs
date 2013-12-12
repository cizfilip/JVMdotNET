using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JVMdotNET.Core.ClassFile
{
    internal class VersionInfo
    {
        public VersionInfo(uint majorVersion, uint minorVersion)
        {
            this.MajorVersion = majorVersion;
            this.MinorVersion = minorVersion;
        }

        public uint MajorVersion { get; private set; }
        public uint MinorVersion { get; private set; }

        public static readonly VersionInfo Java70 = new VersionInfo(51, 0);
        
    }
}
