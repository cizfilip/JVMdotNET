using JVMdotNET.Core.ClassFile.ConstantPool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JVMdotNET.Core.ClassFile.Attributes
{
    internal class ExceptionsAttribute : AttributeBase
    {
        internal const string Name = "Exceptions";

        public string[] ExceptionClasses { get; private set; }

        public ExceptionsAttribute(string[] exceptionClasses)
        {
            this.ExceptionClasses = exceptionClasses;
        }
    }
}
