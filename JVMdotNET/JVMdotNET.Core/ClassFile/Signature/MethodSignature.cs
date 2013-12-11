using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JVMdotNET.Core.ClassFile.Signature
{
    internal class MethodSignature : Signature
    {
        public TypeInfo ReturnType { get; private set; }
        public TypeInfo[] Parameters { get; private set; }

        public int ParametersCount
        {
            get
            {
                return Parameters.Length;
            }
        }

        public MethodSignature(TypeInfo returnType, TypeInfo[] parameters)
        {
            this.ReturnType = returnType;
            this.Parameters = parameters;
        }
    }
}
