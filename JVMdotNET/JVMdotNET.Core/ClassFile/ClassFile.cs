using JVMdotNET.Core.ClassFile.ConstantPool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JVMdotNET.Core.ClassFile
{
    public class ClassFile
    {
        private ConstantPoolItem[] constantPool;

        public ClassFile(ConstantPoolItem[] constantPool)
        {
            this.constantPool = constantPool;
        }

    }
}
