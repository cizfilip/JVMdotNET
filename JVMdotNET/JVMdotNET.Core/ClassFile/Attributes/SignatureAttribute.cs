using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JVMdotNET.Core.ClassFile.Attributes
{
    internal class SignatureAttribute : AttributeBase
    {
        internal const string Name = "Signature";

        public string Signature { get; set; }

        public SignatureAttribute(string signature)
        {
            this.Signature = signature;
        }
    }
}
