using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JVMdotNET.Core.ClassFile.ConstantPool
{
    internal abstract class ValueConstantPoolItem<T> : ConstantPoolItemBase
    {
        public T Value { get; protected set; }

        public ValueConstantPoolItem(T value)
        {
            this.Value = value;
        }

        protected override void ResolveInternal(ConstantPoolItemBase[] constantPool, int index)
        {
            //noop
        }

        //public override ConstantPoolItemType Type
        //{
        //    get { throw new NotImplementedException(); }
        //}
    }
}
