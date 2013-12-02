using JVMdotNET.Core.ClassFile.Attributes;
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

        public MethodInfo(MethodAccessFlags accessFlags, string name, string descriptor, IDictionary<string, AttributeBase> attributes)
            : base(name, descriptor, attributes)
        {
            this.AccessFlags = accessFlags;
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
