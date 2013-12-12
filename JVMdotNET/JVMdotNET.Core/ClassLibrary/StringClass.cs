using JVMdotNET.Core.ClassFile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JVMdotNET.Core.ClassLibrary
{
    internal sealed class StringClass : LibraryClass
    {
        public StringClass() 
            : base(
                "java/lang/String",
                ClassAccessFlags.Public | ClassAccessFlags.Super
            ) 
        {
            //Constructor
            base.AddMethod(new NativeMethodInfo(this, MethodAccessFlags.Public, ClassDefaults.ConstructorMethodName, ClassDefaults.VoidMethodDescriptor, null));

            

          

            //TODO: toString
        }
       
    }
}
