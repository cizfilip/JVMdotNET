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
            this.Name = constantPool.GetItem<Utf8ConstantPoolItem>(nameIndex).String;
            this.Descriptor = constantPool.GetItem<Utf8ConstantPoolItem>(descriptorIndex).String;
        }

        public override ConstantPoolItemType Type
        {
            get { return ConstantPoolItemType.NameAndType; }
        }
    }
}
