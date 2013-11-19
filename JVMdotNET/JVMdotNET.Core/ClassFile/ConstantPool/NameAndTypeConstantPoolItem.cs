using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JVMdotNET.Core.ClassFile.ConstantPool
{
    class NameAndTypeConstantPoolItem : ConstantPoolItem
    {
        private int nameIndex;
        private int descriptorIndex;

        public NameAndTypeConstantPoolItem(int nameIndex, int descriptorIndex)
        {
            this.nameIndex = nameIndex;
            this.descriptorIndex = descriptorIndex;
        }
    }
}
