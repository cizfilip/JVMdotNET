using JVMdotNET.Core.ClassFile.Attributes;
using JVMdotNET.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JVMdotNET.Core.ClassFile
{
    internal abstract class AttributeContainer
    {
        protected IDictionary<string, AttributeBase> attributes;
               

        public T GetAttribute<T>(string attributeName) where T : Attributes.AttributeBase
        {
            AttributeBase attribute;
            if (attributes != null && attributes.TryGetValue(attributeName, out attribute))
            {
                return (T)attribute;
            }
            else
            {
                throw new AttributeNotFoundException(string.Format("Attribute with name {0} not found.", attributeName));
            }
        }


        //private static readonly string[] ValidMethodAttributeNames = { 
        //                                                           CodeAttribute.Name,
        //                                                           SyntheticAttribute.Name, 
        //                                                           SignatureAttribute.Name,
        //                                                           DeprecatedAttribute.Name,
        //                                                           RuntimeVisibleAnnotationsAttribute.Name,
        //                                                           RuntimeInvisibleAnnotationsAttribute.Name,
        //                                                           RuntimeVisibleParameterAnnotationsAttribute.Name,
        //                                                           RuntimeInvisibleParameterAnnotationsAttribute.Name,
        //                                                           AnnotationDefaultAttribute.Name
        //                                                       };

        //private static readonly string[] ValidFieldAttributeNames = { 
        //                                                           ConstantValueAttribute.Name,
        //                                                           SyntheticAttribute.Name, 
        //                                                           SignatureAttribute.Name,
        //                                                           DeprecatedAttribute.Name,
        //                                                           RuntimeVisibleAnnotationsAttribute.Name,
        //                                                           RuntimeInvisibleAnnotationsAttribute.Name
        //                                                       };

        //private static readonly string[] ValidClassAttributeNames = { 
        //                                                           InnerClassesAttribute.Name,
        //                                                           EnclosingMethodAttribute.Name, 
        //                                                           SyntheticAttribute.Name,
        //                                                           SignatureAttribute.Name,
        //                                                           SourceFileAttribute.Name,
        //                                                           SourceDebugExtensionAttribute.Name,
        //                                                           DeprecatedAttribute.Name,
        //                                                           RuntimeVisibleAnnotationsAttribute.Name,
        //                                                           RuntimeInvisibleAnnotationsAttribute.Name,
        //                                                           BootstrapMethodsAttribute.Name
        //                                                       };

        //private static readonly string[] ValidCodeAttributeNames = { 
        //                                                           LineNumberTableAttribute.Name,
        //                                                           LocalVariableTableAttribute.Name, 
        //                                                           LocalVariableTypeTableAttribute.Name,
        //                                                           StackMapTableAttribute.Name
        //                                                       };
    }
}
