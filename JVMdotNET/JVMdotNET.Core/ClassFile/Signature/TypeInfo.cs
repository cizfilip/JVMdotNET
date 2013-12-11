using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JVMdotNET.Core.ClassFile.Signature
{
    internal class TypeInfo
    {
        public JVMType Type { get; private set; }

        public TypeInfo(JVMType type)
        {
            this.Type = type;
        }
    }

    internal class ObjectTypeInfo : TypeInfo
    {
        public string ClassName { get; private set; }

        public ObjectTypeInfo(JVMType type, string className) 
            : base(type)
        {
            this.ClassName = className;
        }
    }

    internal class ArrayTypeInfo : TypeInfo
    {
        public int Dimensions { get; private set; }

        public TypeInfo ElementType { get; private set; }

        public ArrayTypeInfo(JVMType type, int dimensions, TypeInfo elementType)
            : base(type)
        {
            this.Dimensions = dimensions;
            this.ElementType = elementType;
        }
    }

    
}
