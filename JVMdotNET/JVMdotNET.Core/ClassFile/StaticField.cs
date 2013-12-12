using JVMdotNET.Core.ClassFile.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JVMdotNET.Core.ClassFile
{
    internal class StaticField
    {
        public FieldInfo Info { get; private set; }
        public object Value { get; set; }

        public StaticField(FieldInfo fieldInfo)
        {
            this.Info = fieldInfo;
            this.Value = null;

            //Load constantValue from attribute
            ConstantValueAttribute constantAttribute;
            if (fieldInfo.TryGetAttribute<ConstantValueAttribute>(ConstantValueAttribute.Name, out constantAttribute))
            {
                this.Value = constantAttribute.Constant;    
            }
        }
    }
}
