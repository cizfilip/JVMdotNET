using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JVMdotNET.Core.ClassFile.ConstantPool
{
    class InterfaceMethodRefConstantPoolItem : RefConstantPoolItem
    {
        public InterfaceMethodRefConstantPoolItem(int classIndex, int nameAndTypeIndex)
            : base(classIndex, nameAndTypeIndex) { }
    }
}
