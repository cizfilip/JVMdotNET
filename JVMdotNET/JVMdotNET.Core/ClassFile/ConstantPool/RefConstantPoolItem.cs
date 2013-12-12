using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JVMdotNET.Core.ClassFile.ConstantPool
{
    internal abstract class RefConstantPoolItem : ConstantPoolItemBase
    {
        private int classIndex;
        private int nameAndTypeIndex;

        public ClassConstantPoolItem Class { get; private set; }
        public NameAndTypeConstantPoolItem NameAndType { get; private set; }

        public RefConstantPoolItem(int classIndex, int nameAndTypeIndex)
        {
            this.classIndex = classIndex;
            this.nameAndTypeIndex = nameAndTypeIndex;
        }

        protected override void ResolveInternal(ConstantPoolItemBase[] constantPool)
        {
            this.Class = constantPool.GetItem<ClassConstantPoolItem>(classIndex);
            
            this.NameAndType = constantPool[nameAndTypeIndex] as NameAndTypeConstantPoolItem;
        }
    }
}
