using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JVMdotNET.Core.ClassFile.ConstantPool
{
    internal abstract class ValueConstantPoolItem<T> : ValueConstantPoolItem
    {
        public T Value { get; protected set; }

        public ValueConstantPoolItem(T value)
        {
            this.Value = value;
        }

        protected override void ResolveInternal(ConstantPoolItemBase[] constantPool, int index)
        {
            
        }

        public override object GetValue()
        {
            return Value;
        }
    }
}
