using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JVMdotNET.Core.ClassFile.ConstantPool
{
    class RefConstantPoolItem : ConstantPoolItem
    {
        private int classIndex;
        private int nameAndTypeIndex;

        public RefConstantPoolItem(int classIndex, int nameAndTypeIndex)
        {
            this.classIndex = classIndex;
            this.nameAndTypeIndex = nameAndTypeIndex;
        }
    }
}
