using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JVMdotNET.Core.ClassFile.Attributes
{
    internal class SourceFileAttribute : AttributeBase
    {
        internal const string Name = "SourceFile";

        public string SourceFileName { get; private set; }

        public SourceFileAttribute(string sourceFileName)
        {
            this.SourceFileName = sourceFileName;
        }
    }
}
