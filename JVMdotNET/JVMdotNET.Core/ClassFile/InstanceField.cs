using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JVMdotNET.Core.ClassFile
{
    internal class InstanceField
    {
        public FieldInfo Info { get; private set; }
        public int Index { get; private set; }

        public InstanceField(FieldInfo fieldInfo, int index)
        {
            this.Info = fieldInfo;
            this.Index = index;
        }
    }
}
