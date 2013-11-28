using JVMdotNET.Core.ClassFile.Attributes;
using JVMdotNET.Core.ClassFile.ConstantPool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JVMdotNET.Core.ClassFile
{
    internal class ClassFile : IAttributteContainer
    {
        private static readonly string[] ValidAttributeNames = { 
                                                                   InnerClassesAttribute.Name,
                                                                   EnclosingMethodAttribute.Name, 
                                                                   SyntheticAttribute.Name,
                                                                   SignatureAttribute.Name,
                                                                   SourceFileAttribute.Name,
                                                                   SourceDebugExtensionAttribute.Name,
                                                                   DeprecatedAttribute.Name,
                                                                   RuntimeVisibleAnnotationsAttribute.Name,
                                                                   RuntimeInvisibleAnnotationsAttribute.Name,
                                                                   BootstrapMethodsAttribute.Name
                                                               };

        public VersionInfo Version { get; private set; }
        public ConstantPoolItemBase[] ConstantPool { get; private set; }
        public ClassAccessFlags AccessFlags { get; private set; }
        public string Name { get; private set; }
        public string Super { get; private set; }
        public string[] Interfaces { get; private set; }
        public FieldInfo[] Fields { get; private set; }
        public MethodInfo[] Methods { get; private set; }
        public IDictionary<string, AttributeBase> Attributes { get; private set; }

        public ClassFile(VersionInfo version, ConstantPoolItemBase[] constantPool, 
            ClassAccessFlags accessFlags, string name, string super,
            string[] interfaces, FieldInfo[] fields, MethodInfo[] methods,
            IDictionary<string, AttributeBase> attributes)
        {
            this.Version = version;
            this.ConstantPool = constantPool;
            this.AccessFlags = accessFlags;
            this.Name = name;
            this.Super = super;
            this.Interfaces = interfaces;
            this.Fields = fields;
            this.Methods = methods;
            this.Attributes = attributes;
        }



        public string[] ValidAttributes
        {
            get { return ValidAttributeNames; }
        }

        public T GetAttribute<T>(string attributeName) where T : Attributes.AttributeBase
        {
            throw new NotImplementedException();
        }
    }
}
