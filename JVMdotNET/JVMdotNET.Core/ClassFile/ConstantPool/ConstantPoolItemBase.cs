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
        //TODO: typ nejspis odstranit.... pokud se neukaze ze bude nekde potreba
        public abstract ConstantPoolItemType Type { get; }
        

        public void Resolve(ConstantPoolItemBase[] constantPool, int index)
        {
            if (!isResolved)
            {
                ResolveInternal(constantPool, index);
            }
        }

        protected abstract void ResolveInternal(ConstantPoolItemBase[] constantPool, int index);
    }


    internal static class ConstantPoolItemBaseExtensions
    {
        public static T GetItem<T>(this ConstantPoolItemBase[] constantPool, int index) where T : ConstantPoolItemBase
        {
            if (index <= 0 || index >= constantPool.Length)
            {
                throw new ClassFileFormatException("Constant pool index out of range.");
            }

            T result = constantPool[index] as T;

            if (result == null)
            {
                throw new ClassFileFormatException(string.Format("Invalid constant pool item at {0}.", index));
            }

            return result;
        }
    }
}
