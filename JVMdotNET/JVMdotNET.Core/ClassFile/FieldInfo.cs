using JVMdotNET.Core.ClassFile.Attributes;
using JVMdotNET.Core.ClassFile.Signature;
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
        public FieldSignature Signature { get; private set; }
        

        public FieldInfo(FieldAccessFlags accessFlags, string name, FieldSignature signature, IDictionary<string, AttributeBase> attributes)
            : base(name, attributes)
        {
            this.AccessFlags = accessFlags;
            this.Signature = signature;
        }

        public override string[] ValidAttributes
        {
            get { return ValidAttributeNames; }
        }

    }
}
