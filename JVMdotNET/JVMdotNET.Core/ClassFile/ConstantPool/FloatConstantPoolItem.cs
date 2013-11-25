using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JVMdotNET.Core.ClassFile.ConstantPool
{
    internal class FloatConstantPoolItem : ValueConstantPoolItem<float>
    {
        public FloatConstantPoolItem(float value) : base(value) { }

        public override ConstantPoolItemType Type
        {
            get { return ConstantPoolItemType.Float; }
        }
    }
}
