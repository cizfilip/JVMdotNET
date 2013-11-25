using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JVMdotNET.Core.ClassFile.ConstantPool
{
    internal class StringConstantPoolItem : ValueConstantPoolItem<string>
    {
        private int stringIndex;

        public StringConstantPoolItem(int stringIndex) : base(null)
        {
            this.stringIndex = stringIndex;
        }

        protected override void ResolveInternal(ConstantPoolItemBase[] constantPool, int index)
        {
            var value = constantPool[stringIndex] as Utf8ConstantPoolItem;

            if(value == null)
            {
                ThrowInvalidConstantPoolIndex();
            }

            this.Value = value.Value;
        }

        public override ConstantPoolItemType Type
        {
            get { return ConstantPoolItemType.String; }
        }
    }
}
