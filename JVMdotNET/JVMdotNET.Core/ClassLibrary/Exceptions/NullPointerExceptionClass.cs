using JVMdotNET.Core.ClassFile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JVMdotNET.Core.ClassLibrary.Exceptions
{
    internal sealed class NullPointerExceptionClass : LibraryClass
    {
        public static readonly string Name = "java/lang/NullPointerException";

        public NullPointerExceptionClass()
            : base(
                Name,
                ClassAccessFlags.Public,
                RuntimeExceptionClass.Name
            )
        {

            //Constructors
            base.AddMethod(new NativeMethodInfo(this, MethodAccessFlags.Public, ClassDefaults.ConstructorMethodName, ClassDefaults.VoidMethodDescriptor, Cinit));
            base.AddMethod(new NativeMethodInfo(this, MethodAccessFlags.Public, ClassDefaults.ConstructorMethodName, ClassDefaults.StringVoidMethodDescriptor, Cinit_String));

        }

        private object Cinit(JavaInstance instance, object[] parameters, RuntimeEnvironment environment)
        {
            RunNativeSuperConstructor(ClassDefaults.VoidMethodDescriptor, instance, parameters, environment);
            return null;
        }

        private object Cinit_String(JavaInstance instance, object[] parameters, RuntimeEnvironment environment)
        {
            RunNativeSuperConstructor(ClassDefaults.StringVoidMethodDescriptor, instance, parameters, environment);
            return null;
        }

    }
}
