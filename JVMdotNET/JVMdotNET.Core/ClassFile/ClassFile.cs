using JVMdotNET.Core.ClassFile.ConstantPool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JVMdotNET.Core.ClassFile
{
    internal class ClassFile
    {
        private ConstantPoolItemBase[] constantPool;

        public ClassFile(ConstantPoolItemBase[] constantPool)
        {
            this.constantPool = constantPool;
        }

    }
}
