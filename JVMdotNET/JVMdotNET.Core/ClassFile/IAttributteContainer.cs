using JVMdotNET.Core.ClassFile.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JVMdotNET.Core.ClassFile
{
    internal interface IAttributteContainer
    {
        string[] ValidAttributes { get; }
        T GetAttribute<T>(string attributeName) where T : AttributeBase;
    }
}
