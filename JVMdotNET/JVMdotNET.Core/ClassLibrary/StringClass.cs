﻿using JVMdotNET.Core.ClassFile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JVMdotNET.Core.ClassLibrary
{
    internal sealed class StringClass : LibraryClass
    {
        public static readonly string Name = "java/lang/String";

        public StringClass() 
            : base(
                Name,
                ClassAccessFlags.Public | ClassAccessFlags.Super | ClassAccessFlags.Final,
                ObjectClass.Name
            ) 
        {
            //Value Field
            base.AddInstaceField(new InstanceField(
                new FieldInfo(this, FieldAccessFlags.Private | FieldAccessFlags.Final, "value", "[C", ClassDefaults.EmptyAttributes), 0));


            //Constructor
            base.AddMethod(new NativeMethodInfo(this, MethodAccessFlags.Public, ClassDefaults.ConstructorMethodName, ClassDefaults.VoidMethodDescriptor, Cinit));

            
        }

        private object Cinit(JavaInstance instance, object[] parameters, RuntimeEnvironment environment)
        {
            RunNativeSuperConstructor(ClassDefaults.VoidMethodDescriptor, instance, parameters, environment);
            instance.Fields[0] = "";
            return null;
        }
       
    }
}
