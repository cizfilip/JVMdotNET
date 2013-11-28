using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JVMdotNET.Core.ClassFile.Attributes
{
    internal class InnerClassesAttribute : AttributeBase
    {
        internal const string Name = "InnerClasses";

        public InnerClass[] InnerClasses { get; private set; }

        public InnerClassesAttribute(InnerClass[] innerClasses)
        {
            this.InnerClasses = innerClasses;
        }

    }

    internal class InnerClass
    {
        public string InnerClassName { get; private set; }
        public string OuterClassName { get; private set; }
        public string Name { get; private set; }
        public InnerClassAccessFlags AccessFlags { get; private set; }

        public InnerClass(string innerClassName, string outerClassName, string name, InnerClassAccessFlags accessFlags)
        {
            this.InnerClassName = innerClassName;
            this.OuterClassName = outerClassName;
            this.Name = name;
            this.AccessFlags = accessFlags;
        }
    }

}
