using JVMdotNET.Core.ClassFile;
using JVMdotNET.Core.ClassLibrary;
using JVMdotNET.Core.ClassLibrary.Exceptions;
using JVMdotNET.Core.ClassLibrary.Io;
using JVMdotNET.Core.Exceptions;
using System;

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
        }
                

        public void Run()
        {
            RuntimeEnvironment environment = new RuntimeEnvironment(classArea);

            var mainMethod = classArea.FindMainMethod();

            classArea.LoadSupportedClassFromJavaClassLibrary();

            var exception = environment.ExecuteProgram(mainMethod);

            if (exception != null)
            {
                throw new UnhandledJavaException(string.Format("Java program ended with exception {0} : {1}",
                   exception.JavaClass.Name.Replace('/','.'), ExtractExceptionMessage(exception)));
            }
        }

        private string ExtractExceptionMessage(JavaInstance exception)
        {
            JavaInstance message = (JavaInstance)exception.Fields[0];

            if (message == null)
            {
                return string.Empty;
            }
            else
            {
                return (string)message.Fields[0];
            }
        }


    }
}
