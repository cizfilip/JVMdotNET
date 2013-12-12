using JVMdotNET.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JVMdotNET.Core.ClassFile.ConstantPool
{
    internal class ClassConstantPoolItem : ConstantPoolItemBase
    {
        private int nameIndex;

        public string Name { get; private set; }

        public ClassConstantPoolItem(int nameIndex)
        {
            this.nameIndex = nameIndex;
        }

        protected override void ResolveInternal(ConstantPoolItemBase[] constantPool)
        {
            this.Name = constantPool.GetItem<Utf8ConstantPoolItem>(nameIndex).String;
        }

        public override ConstantPoolItemType Type
        {
            get { return ConstantPoolItemType.Class; }
        }
    }
}
