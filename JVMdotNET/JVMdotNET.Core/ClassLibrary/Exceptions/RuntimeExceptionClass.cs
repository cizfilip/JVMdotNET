using JVMdotNET.Core.ClassFile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JVMdotNET.Core.ClassLibrary.Exceptions
{
    internal sealed class RuntimeExceptionClass : LibraryClass
    {
        public static readonly string Name = "java/lang/RuntimeException";

        public RuntimeExceptionClass()
            : base(
                Name,
                ClassAccessFlags.Public,
                ExceptionClass.Name
            )
        {

            //Constructors
            base.AddMethod(new NativeMethodInfo(this, MethodAccessFlags.Public, ClassDefaults.ConstructorMethodName, ClassDefaults.VoidMethodDescriptor, Cinit));
            base.AddMethod(new NativeMethodInfo(this, MethodAccessFlags.Public, ClassDefaults.ConstructorMethodName, ClassDefaults.StringVoidMethodDescriptor, Cinit_String));
            base.AddMethod(new NativeMethodInfo(this, MethodAccessFlags.Public, ClassDefaults.ConstructorMethodName, ClassDefaults.ThrowableVoidMethodDescriptor, Cinit_Throwable));
            base.AddMethod(new NativeMethodInfo(this, MethodAccessFlags.Public, ClassDefaults.ConstructorMethodName, ClassDefaults.StringThrowableVoidMethodDescriptor, Cinit_StringThrowable));

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

        private object Cinit_Throwable(JavaInstance instance, object[] parameters, RuntimeEnvironment environment)
        {
            RunNativeSuperConstructor(ClassDefaults.ThrowableVoidMethodDescriptor, instance, parameters, environment);
            return null;
        }

        private object Cinit_StringThrowable(JavaInstance instance, object[] parameters, RuntimeEnvironment environment)
        {
            RunNativeSuperConstructor(ClassDefaults.StringThrowableVoidMethodDescriptor, instance, parameters, environment);
            return null;
        }
    }
}
