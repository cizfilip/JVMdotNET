using JVMdotNET.Core.ClassFile.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JVMdotNET.Core.ClassFile
{
    internal class FieldInfo : IAttributteContainer
    {
        private static readonly string[] ValidAttributeNames = { 
                                                                   ConstantValueAttribute.Name,
                                                                   SyntheticAttribute.Name, 
                                                                   SignatureAttribute.Name,
                                                                   DeprecatedAttribute.Name,
                                                                   RuntimeVisibleAnnotationsAttribute.Name,
                                                                   RuntimeInvisibleAnnotationsAttribute.Name
                                                               };

        public FieldAccessFlags AccessFlag { get; private set; }
        public string Name { get; private set; }
        public string Descriptor { get; private set; }
        public IDictionary<string, AttributeBase> Attributes { get; private set; }


        public FieldInfo(FieldAccessFlags accessFlag, string name, string descriptor, IDictionary<string, AttributeBase> attributes)
        {
            this.AccessFlag = accessFlag;
            this.Name = name;
            this.Descriptor = descriptor;
            this.Attributes = attributes;
        }

        public string[] ValidAttributes
        {
            get { return ValidAttributeNames; }
        }

        //TODO: validace
        public T GetAttribute<T>(string attributeName) where T : AttributeBase
        {
            return Attributes[attributeName] as T;
        }
    }
}
