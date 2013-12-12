using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JVMdotNET.Core.ClassFile.Attributes
{
    internal class ConstantValueAttribute : AttributeBase
    {
        internal const string Name = "ConstantValue";

        public object Constant { get; private set; }

        public ConstantValueAttribute(object constant)
        {
            this.Constant = constant;
        }
    }
}
