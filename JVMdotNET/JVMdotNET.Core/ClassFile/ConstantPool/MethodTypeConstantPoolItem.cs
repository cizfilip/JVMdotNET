using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JVMdotNET.Core.ClassFile.ConstantPool
{
    internal class MethodTypeConstantPoolItem : ConstantPoolItemBase
    {
        private int descriptorIndex;

        public string MethodDescriptor { get; private set; }

        public MethodTypeConstantPoolItem(int descriptorIndex)
        {
            this.descriptorIndex = descriptorIndex;
        }

        protected override void ResolveInternal(ConstantPoolItemBase[] constantPool)
        {
            //Method type is resolved here but not suported in this JVM.
            this.MethodDescriptor = constantPool.GetItem<Utf8ConstantPoolItem>(descriptorIndex).String;
        }

        public override ConstantPoolItemType Type
        {
            get { return ConstantPoolItemType.MethodType; }
        }
    }
}
