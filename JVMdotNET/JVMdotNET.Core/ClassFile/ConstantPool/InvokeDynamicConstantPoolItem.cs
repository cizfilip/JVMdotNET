using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JVMdotNET.Core.ClassFile.ConstantPool
{
    //Only for future extensions of this JVM. Currently invokedynamic not supported => This constant pool item is not resolved
    internal class InvokeDynamicConstantPoolItem : ConstantPoolItemBase
    {
        private int bootstrapMethodIndex;
        private int nameAndTypeIndex;

        public InvokeDynamicConstantPoolItem(int bootstrapMethodIndex, int nameAndTypeIndex)
        {
            this.bootstrapMethodIndex = bootstrapMethodIndex;
            this.nameAndTypeIndex = nameAndTypeIndex;
        }

        protected override void ResolveInternal(ConstantPoolItemBase[] constantPool)
        {
            //noop
            //not implemented, invokedynamic not suported
        }

        public override ConstantPoolItemType Type
        {
            get { return ConstantPoolItemType.InvokeDynamic; }
        }
    }
}
