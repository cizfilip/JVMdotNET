using JVMdotNET.Core.Bytecode;
using JVMdotNET.Core.ClassFile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JVMdotNET.Core
{
    public class JavaVirtualMachine
    {
        private IDictionary<string, JavaClass> classes;
        private ByteCodeInterpreter interpreter;

        public JavaVirtualMachine()
        {

        }


    }
}
