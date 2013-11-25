using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JVMdotNET.Core.ClassFile.ConstantPool
{
    internal class FieldRefConstantPoolItem : RefConstantPoolItem
    {
        public FieldRefConstantPoolItem(int classIndex, int nameAndTypeIndex)
            : base(classIndex, nameAndTypeIndex) { }

        public override ConstantPoolItemType Type
        {
            get { return ConstantPoolItemType.FieldRef; }
        }
    }
}
