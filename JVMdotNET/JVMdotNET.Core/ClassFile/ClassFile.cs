using JVMdotNET.Core.ClassFile.ConstantPool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JVMdotNET.Core.ClassFile
{
    internal class ClassFile : IAttributteContainer
    {
        private ConstantPoolItemBase[] constantPool;

        public ClassFile(ConstantPoolItemBase[] constantPool)
        {
            this.constantPool = constantPool;
        }


        public string[] ValidAttributes
        {
            get { throw new NotImplementedException(); }
        }

        public T GetAttribute<T>(string attributeName) where T : Attributes.AttributeBase
        {
            throw new NotImplementedException();
        }
    }
}
