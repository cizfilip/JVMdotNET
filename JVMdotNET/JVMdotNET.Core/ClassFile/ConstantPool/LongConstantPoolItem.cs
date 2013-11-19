using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JVMdotNET.Core.ClassFile.ConstantPool
{
    class LongConstantPoolItem : ValueConstantPoolItem<long>
    {
        public LongConstantPoolItem(long value) : base(value) { }
    }
}
