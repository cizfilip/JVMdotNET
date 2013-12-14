using JVMdotNET.Core.ClassFile;
using JVMdotNET.Core.ClassLibrary.Io;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JVMdotNET.Core.ClassLibrary
{
    internal sealed class SystemClass : LibraryClass
    {
        public static readonly string Name = "java/lang/System";

        public SystemClass()
            : base(
                Name,
                ClassAccessFlags.Public | ClassAccessFlags.Super | ClassAccessFlags.Final,
                ObjectClass.Name
            )
        {
            //out static field
            base.AddStaticField(new StaticField(
                new FieldInfo(this, FieldAccessFlags.Public | FieldAccessFlags.Static | FieldAccessFlags.Final, "out", "Ljava/io/PrintStream;", ClassDefaults.EmptyAttributes)));


            //Constructor
            base.AddMethod(new NativeMethodInfo(this, MethodAccessFlags.Static, ClassDefaults.ClassConstructorMethodName, ClassDefaults.VoidMethodDescriptor, CLinit));


        }

        private object CLinit(JavaInstance instance, object[] parameters, RuntimeEnvironment environment)
        {
            JavaInstance printStreamInstance = new JavaInstance(environment.ClassArea.GetClass(PrintStreamClass.Name));

            base.SetStaticFieldValue("out", printStreamInstance);
            return null;
        }

    }
    
}
