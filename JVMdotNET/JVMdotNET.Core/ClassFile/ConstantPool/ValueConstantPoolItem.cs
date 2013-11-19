using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JVMdotNET.Core.ClassFile.ConstantPool
{
    class ValueConstantPoolItem<T> : ConstantPoolItem where T : struct
    {
        private T value;

        public ValueConstantPoolItem(T value)
        {
            this.value = value;
        }
    }
}
