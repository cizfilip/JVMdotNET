using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JVMdotNET.Core.ClassFile
{
    class FieldInfo
    {
        public FieldAccessFlag AccessFlag { get; private set; }
        public string Name { get; private set; }
        public string Descriptor { get; private set; }

        public FieldInfo(FieldAccessFlag accessFlag, string name, string descriptor)
        {
            this.AccessFlag = accessFlag;
            this.Name = name;
            this.Descriptor = descriptor;
        }
    }
}
