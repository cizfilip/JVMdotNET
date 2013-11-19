using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JVMdotNET.Core.ClassFile.ConstantPool
{
    class Utf8ConstantPoolItem : ValueConstantPoolItem<string>
    {
        public Utf8ConstantPoolItem(string value) : base(value) { }
    }
}
