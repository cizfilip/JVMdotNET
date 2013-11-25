using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JVMdotNET.Core.ClassFile.ConstantPool
{
    internal class MethodHandleConstantPoolItem : ConstantPoolItemBase
    {
        private byte referenceKind;
        private int referenceIndex;

        public MethodHandleConstantPoolItem(byte referenceKind, int referenceIndex)
        {
            this.referenceKind = referenceKind;
            this.referenceIndex = referenceIndex;
        }

        protected override void ResolveInternal(ConstantPoolItemBase[] constantPool, int index)
        {
            //TODO: implement this
            throw new NotImplementedException();
        }

        public override ConstantPoolItemType Type
        {
            get { return ConstantPoolItemType.MethodHandle; }
        }
    }
}
