using JVMdotNET.Core.ClassFile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JVMdotNET.Core.ClassLibrary.Io
{

    //Only print(char), print(int), println(int) and println() methods supported!
    internal sealed class PrintStreamClass : LibraryClass
    {
        public static readonly string Name = "java/io/PrintStream";

        public PrintStreamClass()
            : base(
                Name,
                ClassAccessFlags.Public | ClassAccessFlags.Super | ClassAccessFlags.Final,
                ObjectClass.Name
            )
        {
            
            string printName = "print";

            //print(char)
            base.AddMethod(new NativeMethodInfo(this, MethodAccessFlags.Public, printName, "(C)V", PrintChar));

            //print(int)
            base.AddMethod(new NativeMethodInfo(this, MethodAccessFlags.Public, printName, "(I)V", PrintInt));

            //println()
            base.AddMethod(new NativeMethodInfo(this, MethodAccessFlags.Public, "println", ClassDefaults.VoidMethodDescriptor, PrintLn));

            //println(int)
            base.AddMethod(new NativeMethodInfo(this, MethodAccessFlags.Public, "println", "(I)V", PrintLnInt));
        }
        
        private object PrintChar(JavaInstance instance, object[] parameters, RuntimeEnvironment environment)
        {
            char ch = (char)(int)parameters[0];
            Console.Write(ch);
            return null;
        }

        private object PrintInt(JavaInstance instance, object[] parameters, RuntimeEnvironment environment)
        {
            int i = (int)parameters[0];
            Console.Write(i);
            return null;
        }

        private object PrintLn(JavaInstance instance, object[] parameters, RuntimeEnvironment environment)
        {
            Console.WriteLine();
            return null;
        }

        private object PrintLnInt(JavaInstance instance, object[] parameters, RuntimeEnvironment environment)
        {
            int i = (int)parameters[0];
            Console.WriteLine(i);
            return null;
        }
    }
}
