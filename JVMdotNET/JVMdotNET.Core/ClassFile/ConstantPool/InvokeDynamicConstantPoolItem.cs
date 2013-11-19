using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JVMdotNET.Core.ClassFile.ConstantPool
{
    class InvokeDynamicConstantPoolItem : ConstantPoolItem
    {
        private int bootstrapMethodIndex;
        private int nameAndTypeIndex;

        public InvokeDynamicConstantPoolItem(int bootstrapMethodIndex, int nameAndTypeIndex)
        {
            this.bootstrapMethodIndex = bootstrapMethodIndex;
            this.nameAndTypeIndex = nameAndTypeIndex;
        }
    }
}
