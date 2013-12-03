using JVMdotNET.Core.ClassFile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JVMdotNET.Core.TypeSystem
{
    internal sealed class JavaArray : JavaReference
    {
        public object[] Array { get; private set; }
        public JavaClass ArrayClass { get; private set; }

        public JavaArray(int arrayLength, JavaClass arrayClass)
        {
            this.Array = new object[arrayLength];
            this.ArrayClass = arrayClass;
        }
    }
}
