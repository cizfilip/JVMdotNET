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
        private static readonly string[] ValidAttributeNames = { 
                                                                   CodeAttribute.Name,
                                                                   SyntheticAttribute.Name, 
                                                                   SignatureAttribute.Name,
                                                                   DeprecatedAttribute.Name,
                                                                   RuntimeVisibleAnnotationsAttribute.Name,
                                                                   RuntimeInvisibleAnnotationsAttribute.Name,
                                                                   RuntimeVisibleParameterAnnotationsAttribute.Name,
                                                                   RuntimeInvisibleParameterAnnotationsAttribute.Name,
                                                                   AnnotationDefaultAttribute.Name

                                                               };

        public MethodAccessFlags AccessFlags { get; private set; }
        public MethodSignature Signature { get; private set; }
        

        public MethodInfo(MethodAccessFlags accessFlags, string name, MethodSignature signature, IDictionary<string, AttributeBase> attributes)
            : base(name, attributes)
        {
            this.AccessFlags = accessFlags;
            this.Signature = signature;
        }
        
        public bool IsSpecialMethod
        {
            get
            {
                return Name == "<init>" || Name == "<clinit>";
            }
        }

        public override string[] ValidAttributes
        {
            get { return ValidAttributeNames; }
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
