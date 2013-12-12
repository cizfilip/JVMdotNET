using JVMdotNET.Core.ClassFile.Attributes;
using JVMdotNET.Core.ClassFile.Signature;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JVMdotNET.Core.ClassFile
{
    internal class FieldInfo : ClassItemInfo
    {
        public FieldAccessFlags AccessFlags { get; private set; }
        public FieldSignature Signature { get; private set; }
        

        public FieldInfo(JavaClass @class, FieldAccessFlags accessFlags, string name, string descriptor, IDictionary<string, AttributeBase> attributes)
            : base(@class, name, descriptor, attributes)
        {
            this.AccessFlags = accessFlags;
            this.Signature = JVMdotNET.Core.ClassFile.Signature.Signature.ParseFieldSignature(descriptor);
        }
        
    }
}
