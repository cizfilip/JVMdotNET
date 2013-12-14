using JVMdotNET.Core.ClassFile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JVMdotNET.Core.ClassLibrary
{
    internal sealed class ObjectClass : LibraryClass
    {
        public static readonly string Name = "java/lang/Object";

        public ObjectClass() 
            : base(
                Name,
                ClassAccessFlags.Public | ClassAccessFlags.Super,
                null
            ) 
        {
            //Constructor
            base.AddMethod(new NativeMethodInfo(this, MethodAccessFlags.Public, ClassDefaults.ConstructorMethodName, ClassDefaults.VoidMethodDescriptor, null));

            //hashCode
            base.AddMethod(new NativeMethodInfo(this, MethodAccessFlags.Public, "hashCode", "()I", HashCode));

            //equals
            base.AddMethod(new NativeMethodInfo(this, MethodAccessFlags.Public, "equals", "(Ljava/lang/Object;)Z", Equals));

            //toString
            base.AddMethod(new NativeMethodInfo(this, MethodAccessFlags.Public, "toString", "()Ljava/lang/String;", ToString));
        }

        
                
        private object HashCode(JavaInstance instance, object[] parameters, RuntimeEnvironment environment)
        {
            return instance.GetHashCode();
        }

        private object Equals(JavaInstance instance, object[] parameters, RuntimeEnvironment environment)
        {
            if (instance == (JavaInstance)parameters[0])
            {
                return 1;
            }
            return 0;
        }

        private object ToString(JavaInstance instance, object[] parameters, RuntimeEnvironment environment)
        {
            return environment.CreateStringInstance(string.Format("{0}@{1}", instance.JavaClass.Name.Replace('/','.'), instance.GetHashCode().ToString("X")));
        }

       
         
    }
}
