using JVMdotNET.Core.ClassLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JVMdotNET.Core.ClassFile
{
    internal class NativeMethodInfo : MethodInfo
    {
        public Func<JavaInstance, object[], RuntimeEnvironment, object> Implementation { get; private set; }

        public NativeMethodInfo(JavaClass @class, MethodAccessFlags accessFlags, string name, string descriptor,
            Func<JavaInstance, object[], RuntimeEnvironment, object> implementation)
            :base(@class, accessFlags | MethodAccessFlags.Native, name, descriptor, ClassDefaults.EmptyAttributes)
        {
            if (implementation == null)
            {
                this.Implementation = ClassDefaults.EmptyImplementation;
            }
            else
            {
                this.Implementation = implementation;
            }
        }

    }
}
