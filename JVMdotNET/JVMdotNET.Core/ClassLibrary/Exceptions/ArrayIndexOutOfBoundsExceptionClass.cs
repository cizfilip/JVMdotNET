using JVMdotNET.Core.ClassFile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JVMdotNET.Core.ClassLibrary.Exceptions
{
    internal sealed class ArrayIndexOutOfBoundsExceptionClass : LibraryClass
    {
        public static readonly string Name = "java/lang/ArrayIndexOutOfBoundsException";

        public ArrayIndexOutOfBoundsExceptionClass()
            : base(
                Name,
                ClassAccessFlags.Public,
                RuntimeExceptionClass.Name
            )
        {

            //Constructors
            base.AddMethod(new NativeMethodInfo(this, MethodAccessFlags.Public, ClassDefaults.ConstructorMethodName, ClassDefaults.VoidMethodDescriptor, Cinit));
            base.AddMethod(new NativeMethodInfo(this, MethodAccessFlags.Public, ClassDefaults.ConstructorMethodName, ClassDefaults.StringVoidMethodDescriptor, Cinit_String));
            base.AddMethod(new NativeMethodInfo(this, MethodAccessFlags.Public, ClassDefaults.ConstructorMethodName, ClassDefaults.IntVoidMethodDescriptor, Cinit_Int));

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

        private object Cinit_Int(JavaInstance instance, object[] parameters, RuntimeEnvironment environment)
        {
            string message = "Array index out of range: " + parameters[0].ToString();
            var stringInstance = environment.CreateStringInstance(message);
            RunNativeSuperConstructor(ClassDefaults.StringVoidMethodDescriptor, instance, new object[1] { stringInstance } , environment);
            return null;
        }

    }
}
