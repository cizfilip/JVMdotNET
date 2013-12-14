using JVMdotNET.Core.ClassFile.Attributes;
using JVMdotNET.Core.ClassFile.ConstantPool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JVMdotNET.Core.ClassLibrary
{
    internal static class ClassDefaults
    {
        
        public static readonly string ConstructorMethodName = "<init>";
        public static readonly string ClassConstructorMethodName = "<clinit>";
        public static readonly string VoidMethodDescriptor = "()V";
        public static readonly string StringVoidMethodDescriptor = "(Ljava/lang/String;)V";
        public static readonly string ThrowableVoidMethodDescriptor = "(Ljava/lang/Throwable;)V";
        public static readonly string StringThrowableVoidMethodDescriptor = "(Ljava/lang/String;Ljava/lang/Throwable;)V";
        public static readonly string IntVoidMethodDescriptor = "(I)V";

        
        public static readonly string StringFieldType = "Ljava/lang/String;";


        public static readonly object[] EmptyParameters = new object[0];
        public static readonly ConstantPoolItemBase[] EmptyConstantPool = new ConstantPoolItemBase[0];
        public static readonly string[] EmptyInterfaces = new string[0];
        public static readonly IDictionary<string, AttributeBase> EmptyAttributes = new Dictionary<string, AttributeBase>();

        public static readonly Func<JavaInstance, object[], RuntimeEnvironment, object> EmptyImplementation = (i, p, env) => { return null; };
    }
}
