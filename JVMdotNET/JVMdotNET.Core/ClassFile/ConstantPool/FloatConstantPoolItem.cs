using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JVMdotNET.Core.ClassFile.ConstantPool
{
    class FloatConstantPoolItem : ValueConstantPoolItem<float>
    {
        public FloatConstantPoolItem(float value) : base(value) { }
    }
}
