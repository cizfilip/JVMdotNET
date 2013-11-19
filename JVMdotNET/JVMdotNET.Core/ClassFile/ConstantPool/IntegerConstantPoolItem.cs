using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JVMdotNET.Core.ClassFile.ConstantPool
{
    class IntegerConstantPoolItem : ValueConstantPoolItem<int>
    {
        public IntegerConstantPoolItem(int value) : base(value) { }
    }
}
