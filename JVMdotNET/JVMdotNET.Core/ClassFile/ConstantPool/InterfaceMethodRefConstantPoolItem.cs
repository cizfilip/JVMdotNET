using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JVMdotNET.Core.ClassFile.ConstantPool
{
    internal class InterfaceMethodRefConstantPoolItem : RefConstantPoolItem
    {
        public InterfaceMethodRefConstantPoolItem(int classIndex, int nameAndTypeIndex)
            : base(classIndex, nameAndTypeIndex) { }

        public override ConstantPoolItemType Type
        {
            get { return ConstantPoolItemType.InterfaceMethodRef; }
        }
    }
}
