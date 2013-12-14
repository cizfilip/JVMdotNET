using JVMdotNET.Core.ClassFile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JVMdotNET.Core.ClassLibrary.Exceptions
{
    internal sealed class ThrowableClass : LibraryClass
    {
        public static readonly string Name = "java/lang/Throwable";

        public ThrowableClass()
            : base(
                Name,
                ClassAccessFlags.Public,
                ObjectClass.Name
            )
        {
            //Fields
            base.AddInstaceField(new InstanceField(
                new FieldInfo(this, FieldAccessFlags.Private, "message", ClassDefaults.StringFieldType, ClassDefaults.EmptyAttributes), 0));
            base.AddInstaceField(new InstanceField(
                new FieldInfo(this, FieldAccessFlags.Private, "cause", NameToFieldType(Name), ClassDefaults.EmptyAttributes), 1));

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
            RunNativeSuperConstructor(ClassDefaults.VoidMethodDescriptor, instance, parameters, environment);
            instance.Fields[0] = parameters[0];
            return null;
        }

        private object Cinit_Throwable(JavaInstance instance, object[] parameters, RuntimeEnvironment environment)
        {
            RunNativeSuperConstructor(ClassDefaults.VoidMethodDescriptor, instance, parameters, environment);
            instance.Fields[1] = parameters[0];
            return null;
        }

        private object Cinit_StringThrowable(JavaInstance instance, object[] parameters, RuntimeEnvironment environment)
        {
            RunNativeSuperConstructor(ClassDefaults.VoidMethodDescriptor, instance, parameters, environment);
            instance.Fields[0] = parameters[0];
            instance.Fields[1] = parameters[1];
            return null;
        }

       
    }
    
}
