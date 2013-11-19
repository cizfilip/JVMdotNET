using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JVMdotNET.Core.ClassFile.ConstantPool
{
    class DoubleConstantPoolItem : ValueConstantPoolItem<double>
    {
        public DoubleConstantPoolItem(double value) : base(value) {}
    }
}
