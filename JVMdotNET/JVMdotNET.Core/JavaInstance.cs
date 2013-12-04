﻿using JVMdotNET.Core.ClassFile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JVMdotNET.Core
{
    internal sealed class JavaInstance
    {
        public object[] Fields { get; private set; }
        public JavaClass JavaClass { get; private set; }
        
        public JavaInstance(int fieldsLength, JavaClass javaClass)
        {
            this.Fields = new object[fieldsLength];
            this.JavaClass = javaClass;
        }
    }
}