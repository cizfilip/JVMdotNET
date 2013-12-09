using JVMdotNET.Core.ClassFile.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JVMdotNET.Core.ClassFile
{
    internal abstract class ClassItemInfo : IAttributteContainer
    {
        public string Name { get; private set; }
        public string Descriptor { get; private set; }
        public IDictionary<string, AttributeBase> Attributes { get; private set; }

        public ClassItemInfo(string name, string descriptor, IDictionary<string, AttributeBase> attributes)
        {
            this.Name = name;
            this.Descriptor = descriptor;
            this.Attributes = attributes;
        }

        public abstract string[] ValidAttributes { get; }

        public T GetAttribute<T>(string attributeName) where T : AttributeBase
        {
            AttributeBase attribute;
            if (Attributes.TryGetValue(attributeName, out attribute))
            {
                return (T)attribute;
            }
            else
            {
                throw new InvalidOperationException(string.Format("Attribute with name {0} not found in class item {1}.", attributeName, Name));
            }
        }
    }
}
