using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JVMdotNET.Core.ClassFile.ConstantPool
{
    class StringConstantPoolItem : ConstantPoolItem
    {
        private int stringIndex;

        public StringConstantPoolItem(int stringIndex)
        {
            this.stringIndex = stringIndex;
        }
    }
}
