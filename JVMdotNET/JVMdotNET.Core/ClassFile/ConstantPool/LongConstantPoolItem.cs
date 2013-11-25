using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JVMdotNET.Core.ClassFile.ConstantPool
{
    internal class LongConstantPoolItem : ValueConstantPoolItem<long>
    {
        public LongConstantPoolItem(long value) : base(value) { }

        public override ConstantPoolItemType Type
        {
            get { return ConstantPoolItemType.Long; }
        }
    }
}
