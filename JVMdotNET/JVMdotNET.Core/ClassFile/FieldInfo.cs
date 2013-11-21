using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JVMdotNET.Core.ClassFile
{
    class FieldInfo
    {
        private FieldAccessFlag accessFlag;
        private int nameIndex;
        private int descriptorIndex;


        public FieldInfo(FieldAccessFlag accessFlag, int nameIndex, int descriptorIndex)
        {
            this.accessFlag = accessFlag;
            this.nameIndex = nameIndex;
            this.descriptorIndex = descriptorIndex;
        }
    }
}
