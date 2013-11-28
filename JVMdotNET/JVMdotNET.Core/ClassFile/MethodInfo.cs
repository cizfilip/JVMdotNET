using JVMdotNET.Core.ClassFile.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JVMdotNET.Core.ClassFile
{
    internal class MethodInfo : IAttributteContainer
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
