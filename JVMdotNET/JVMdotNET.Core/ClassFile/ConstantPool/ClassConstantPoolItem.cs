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

        protected override void ResolveInternal(ConstantPoolItemBase[] constantPool, int index)
        {
            var name = constantPool[nameIndex] as Utf8ConstantPoolItem;
            if (name == null)
            {
                ThrowInvalidConstantPoolIndex();
            }
            this.Name = name.Value;
        }

        public override ConstantPoolItemType Type
        {
            get { return ConstantPoolItemType.Class; }
        }
    }
}
