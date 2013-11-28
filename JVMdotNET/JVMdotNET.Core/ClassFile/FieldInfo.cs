using JVMdotNET.Core.ClassFile.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JVMdotNET.Core.ClassFile
{
    internal class FieldInfo : ClassItemInfo
    {
        private static readonly string[] ValidAttributeNames = { 
                                                                   ConstantValueAttribute.Name,
                                                                   SyntheticAttribute.Name, 
                                                                   SignatureAttribute.Name,
                                                                   DeprecatedAttribute.Name,
                                                                   RuntimeVisibleAnnotationsAttribute.Name,
                                                                   RuntimeInvisibleAnnotationsAttribute.Name
                                                               };

        public FieldAccessFlags AccessFlags { get; private set; }
        


        public FieldInfo(FieldAccessFlags accessFlags, string name, string descriptor, IDictionary<string, AttributeBase> attributes)
            : base(name, descriptor, attributes)
        {
            this.AccessFlags = accessFlags;
        }

        public override string[] ValidAttributes
        {
            get { return ValidAttributeNames; }
        }

    }
}
