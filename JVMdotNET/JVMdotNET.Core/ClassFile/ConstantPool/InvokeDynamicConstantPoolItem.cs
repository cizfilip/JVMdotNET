using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JVMdotNET.Core.ClassFile.ConstantPool
{
    //TODO: souvisi s instrukci invokedynamic kterou nemusim implementovat, jelikoz java compiler ji nikdy nevygeneruje, v JVM je jen pro podporu dynamickych jazyku
    internal class InvokeDynamicConstantPoolItem : ConstantPoolItemBase
    {
        private int bootstrapMethodIndex;
        private int nameAndTypeIndex;

        public InvokeDynamicConstantPoolItem(int bootstrapMethodIndex, int nameAndTypeIndex)
        {
            this.bootstrapMethodIndex = bootstrapMethodIndex;
            this.nameAndTypeIndex = nameAndTypeIndex;
        }

        protected override void ResolveInternal(ConstantPoolItemBase[] constantPool, int index)
        {
            //noop
            //not implemented because Java compiler language
        }

        public override ConstantPoolItemType Type
        {
            get { return ConstantPoolItemType.InvokeDynamic; }
        }
    }
}
