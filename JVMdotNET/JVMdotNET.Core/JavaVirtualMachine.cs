using JVMdotNET.Core.Bytecode;
using JVMdotNET.Core.ClassFile;
using JVMdotNET.Core.ClassLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JVMdotNET.Core
{
    public class JavaVirtualMachine
    {
        private RuntimeClassArea classArea;

        public JavaVirtualMachine(string[] classFiles)
        {
            this.classArea = new RuntimeClassArea();
            foreach (var classFile in classFiles)
            {
                classArea.LoadClassFile(classFile);
            }

            LoadSupportedClassFromJavaClassLibrary();
        }

        private void LoadSupportedClassFromJavaClassLibrary()
        {
            classArea.AddJavaClass(new ObjectClass());
        }

        public void Run()
        {
            RuntimeEnvironment environment = new RuntimeEnvironment(classArea);

            var javaClass = classArea.GetClass("Program");
            var mainMethod = javaClass.GetMethodInfo("main([Ljava/lang/String;)V");

            environment.ExecuteProgram(mainMethod);
            
        }



    }
}
