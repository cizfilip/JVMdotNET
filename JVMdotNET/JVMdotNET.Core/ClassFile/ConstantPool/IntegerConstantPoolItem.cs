using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JVMdotNET.Core.ClassFile.ConstantPool
{
    internal class IntegerConstantPoolItem : ValueConstantPoolItem<int>
    {
        public IntegerConstantPoolItem(int value) : base(value) { }

        public override ConstantPoolItemType Type
        {
            get { return ConstantPoolItemType.Integer; }
        }
    }
}
