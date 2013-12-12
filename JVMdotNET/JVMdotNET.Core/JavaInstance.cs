using JVMdotNET.Core.ClassFile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JVMdotNET.Core
{
    internal sealed class JavaInstance
    {
        public object[] Fields { get; private set; }
        public JavaClass JavaClass { get; private set; }
        
        public JavaInstance(JavaClass javaClass)
        {
            this.Fields = new object[javaClass.GetInstanceFieldsCount()];
            this.JavaClass = javaClass;
        }
    }
}
