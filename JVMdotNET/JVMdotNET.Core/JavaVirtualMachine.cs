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
        private RuntimeClassArea classArea;
        private RuntimeEnvironment runtime;

        public JavaVirtualMachine(params string[] classFiles)
        {
            this.classArea = new RuntimeClassArea();
            foreach (var classFile in classFiles)
            {
                classArea.LoadClassFile(classFile);
            }
        }

        public void Run()
        {
            //runtime.ExecuteProgram()
        }


    }
}
