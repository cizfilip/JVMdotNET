using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JVMdotNET.Core.ClassFile.ConstantPool
{
    internal class MethodTypeConstantPoolItem : ConstantPoolItemBase
    {
        private int descriptorIndex;

        public MethodTypeConstantPoolItem(int descriptorIndex)
        {
            this.descriptorIndex = descriptorIndex;
        }

        protected override void ResolveInternal(ConstantPoolItemBase[] constantPool, int index)
        {
            //TODO: implement this
            throw new NotImplementedException();
        }

        public override ConstantPoolItemType Type
        {
            get { return ConstantPoolItemType.MethodType; }
        }
    }
}
