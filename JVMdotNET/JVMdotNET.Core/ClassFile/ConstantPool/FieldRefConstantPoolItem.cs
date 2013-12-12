using JVMdotNET.Core.ClassFile.Signature;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JVMdotNET.Core.ClassFile.ConstantPool
{
    internal class FieldRefConstantPoolItem : RefConstantPoolItem
    {
        public FieldRefConstantPoolItem(int classIndex, int nameAndTypeIndex)
            : base(classIndex, nameAndTypeIndex) { }

        public FieldSignature Signature { get; set; }

        protected override void ResolveInternal(ConstantPoolItemBase[] constantPool)
        {
            base.ResolveInternal(constantPool);

            NameAndType.Resolve(constantPool);

            this.Signature = JVMdotNET.Core.ClassFile.Signature.Signature.ParseFieldSignature(NameAndType.Descriptor);
        }

        public override ConstantPoolItemType Type
        {
            get { return ConstantPoolItemType.FieldRef; }
        }
    }
}
