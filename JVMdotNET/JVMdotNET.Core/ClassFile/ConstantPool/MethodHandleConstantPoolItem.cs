using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JVMdotNET.Core.ClassFile.ConstantPool
{
    class MethodHandleConstantPoolItem : ConstantPoolItem
    {
        private byte referenceKind;
        private int referenceIndex;

        public MethodHandleConstantPoolItem(byte referenceKind, int referenceIndex)
        {
            this.referenceKind = referenceKind;
            this.referenceIndex = referenceIndex;
        }
    }
}
