using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JVMdotNET.Core.ClassFile.ConstantPool
{
    internal class ClassConstantPoolItem : ConstantPoolItem
    {
        private int nameIndex;

        public ClassConstantPoolItem(int nameIndex)
        {
            this.nameIndex = nameIndex;
        }
    }
}
