using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JVMdotNET.Core.ClassFile.ConstantPool
{
    internal class NameAndTypeConstantPoolItem : ConstantPoolItemBase
    {
        private int nameIndex;
        private int descriptorIndex;

        public string Name { get; private set; }
        public string Descriptor { get; private set; }

        public NameAndTypeConstantPoolItem(int nameIndex, int descriptorIndex)
        {
            this.nameIndex = nameIndex;
            this.descriptorIndex = descriptorIndex;
        }

        protected override void ResolveInternal(ConstantPoolItemBase[] constantPool, int index)
        {
            var name = constantPool[nameIndex] as Utf8ConstantPoolItem;
            var descriptor = constantPool[descriptorIndex] as Utf8ConstantPoolItem;

            if (name == null || descriptor == null)
            {
                ThrowInvalidConstantPoolIndex();
            }

            this.Name = name.Value;
            this.Descriptor = descriptor.Value;
        }

        public override ConstantPoolItemType Type
        {
            get { return ConstantPoolItemType.NameAndType; }
        }
    }
}
