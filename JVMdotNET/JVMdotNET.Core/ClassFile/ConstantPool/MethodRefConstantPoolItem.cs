using JVMdotNET.Core.ClassFile.Signature;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JVMdotNET.Core.ClassFile.ConstantPool
{
    internal class MethodRefConstantPoolItem : RefConstantPoolItem
    {
        public MethodRefConstantPoolItem(int classIndex, int nameAndTypeIndex)
            : base(classIndex, nameAndTypeIndex) { }

        public MethodSignature Signature { get; private set; }
        public string Key { get; private set; }

        protected override void ResolveInternal(ConstantPoolItemBase[] constantPool)
        {
            base.ResolveInternal(constantPool);

            NameAndType.Resolve(constantPool);

            this.Key = NameAndType.Name + NameAndType.Descriptor;

            this.Signature = JVMdotNET.Core.ClassFile.Signature.Signature.ParseMethodSignature(NameAndType.Descriptor);
        }

        public override ConstantPoolItemType Type
        {
            get { return ConstantPoolItemType.MethodRef; }
        }
    }
}
