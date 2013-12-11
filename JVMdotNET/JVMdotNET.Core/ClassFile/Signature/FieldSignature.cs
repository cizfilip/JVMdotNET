using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JVMdotNET.Core.ClassFile.Signature
{
    internal class FieldSignature : Signature
    {
        public TypeInfo Type { get; private set; }

        public FieldSignature(TypeInfo typeInfo)
        {
            this.Type = typeInfo;
        }
    }
}
