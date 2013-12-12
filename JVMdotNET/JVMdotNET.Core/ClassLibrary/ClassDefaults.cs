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
        public static string ConstructorMethodName = "<init>";
        public static string ClassConstructorMethodName = "<clinit>";
        public static string VoidMethodDescriptor = "()V";




        public static ConstantPoolItemBase[] EmptyConstantPool = new ConstantPoolItemBase[0];
        public static string[] EmptyInterfaces = new string[0];
        public static IDictionary<string, AttributeBase> EmptyAttributes = new Dictionary<string, AttributeBase>();

        public static Func<JavaInstance, object[], RuntimeEnvironment, object> EmptyImplementation = (i, p, env) => { return null; };
    }
}
