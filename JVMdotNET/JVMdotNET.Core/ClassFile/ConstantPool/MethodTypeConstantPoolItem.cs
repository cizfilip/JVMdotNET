using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JVMdotNET.Core.ClassFile.ConstantPool
{
    class MethodTypeConstantPoolItem : ConstantPoolItem
    {
        private int descriptorIndex;

        public MethodTypeConstantPoolItem(int descriptorIndex)
        {
            this.descriptorIndex = descriptorIndex;
        }
    }
}
