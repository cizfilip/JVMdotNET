using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JVMdotNET.Core.ClassFile.ConstantPool
{
    internal abstract class ValueConstantPoolItem : ConstantPoolItemBase
    {
        public abstract object GetValue();

        protected override void ResolveInternal(ConstantPoolItemBase[] constantPool, int index)
        {
            //noop
        }
    }
}
