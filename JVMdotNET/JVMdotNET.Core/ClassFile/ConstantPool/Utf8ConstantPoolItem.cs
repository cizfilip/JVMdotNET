using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JVMdotNET.Core.ClassFile.ConstantPool
{
    internal class Utf8ConstantPoolItem : ConstantPoolItemBase
    {
        public string String { get; private set; }

        public Utf8ConstantPoolItem(string value) 
        {
            this.String = value;
        }

        public override ConstantPoolItemType Type
        {
            get { return ConstantPoolItemType.Utf8; }
        }

        protected override void ResolveInternal(ConstantPoolItemBase[] constantPool)
        {
            //noop
        }
    }
}
