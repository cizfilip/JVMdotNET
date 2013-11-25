using JVMdotNET.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JVMdotNET.Core.ClassFile.ConstantPool
{
    internal abstract class ConstantPoolItemBase
    {
        protected bool isResolved = false;
        public abstract ConstantPoolItemType Type { get; }
        

        public void Resolve(ConstantPoolItemBase[] constantPool, int index)
        {
            if (!isResolved)
            {
                ResolveInternal(constantPool, index);
            }
        }

        protected abstract void ResolveInternal(ConstantPoolItemBase[] constantPool, int index);

        protected void ThrowInvalidConstantPoolIndex()
        {
            throw new ClassFileFormatException("Invalid constant pool index");
        }
    }
}
