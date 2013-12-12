using JVMdotNET.Core.ClassFile.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JVMdotNET.Core.ClassFile
{
    internal abstract class ClassItemInfo : AttributeContainer
    {
        public string Name { get; private set; }
        public string Descriptor { get; private set; }
        public JavaClass Class { get; private set; }

        public ClassItemInfo(JavaClass @class, string name, string descriptor, IDictionary<string, AttributeBase> attributes)
        {
            this.Class = @class;
            this.Name = name;
            this.Descriptor = descriptor;
            base.attributes = attributes;
        }
    }
}
