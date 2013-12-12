using JVMdotNET.Core.ClassFile.Attributes;
using JVMdotNET.Core.ClassFile.Signature;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JVMdotNET.Core.ClassFile
{
    internal class MethodInfo : ClassItemInfo
    {
        public MethodAccessFlags AccessFlags { get; private set; }
        public MethodSignature Signature { get; private set; }
        public string Key { get; private set; }
        
        public MethodInfo(JavaClass @class, MethodAccessFlags accessFlags, string name, string descriptor, IDictionary<string, AttributeBase> attributes)
            : base(@class, name, descriptor, attributes)
        {
            this.AccessFlags = accessFlags;
            this.Signature = JVMdotNET.Core.ClassFile.Signature.Signature.ParseMethodSignature(descriptor);
            this.Key = Name + Descriptor;
        }

        public bool IsInitializationMethod
        {
            get
            {
                return Name == "<init>" || Name == "<clinit>";
            }
        }

        public bool IsNative
        {
            get
            {
                return AccessFlags.HasFlag(MethodAccessFlags.Native);
            }
        }

        public CodeAttribute Code
        {
            get
            {
                return GetAttribute<CodeAttribute>(CodeAttribute.Name);
            }
        }

    }
}
