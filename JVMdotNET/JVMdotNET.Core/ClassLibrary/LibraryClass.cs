﻿using JVMdotNET.Core.ClassFile;
using JVMdotNET.Core.ClassFile.Attributes;
using JVMdotNET.Core.ClassFile.ConstantPool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JVMdotNET.Core.ClassLibrary
{
    internal class LibraryClass : JavaClass
    {
        public LibraryClass(string name, ClassAccessFlags accessFlags, string superName) : base()
        {
            base.Name = name;
            //TODO: anebo rovnou JavaClass??
            base.Super = superName;
            base.AccessFlags = accessFlags;

            base.Version = VersionInfo.Java70;
            base.ConstantPool = ClassDefaults.EmptyConstantPool;
            base.Interfaces = ClassDefaults.EmptyInterfaces;

            base.Attributes = ClassDefaults.EmptyAttributes;
        }

        protected void AddMethod(NativeMethodInfo methodInfo)
        {
            base.Methods.Add(methodInfo.Key, methodInfo);
        }

        protected void AddInstaceField(InstanceField fieldInfo)
        {
            base.InstanceFields.Add(fieldInfo.Info.Name, fieldInfo);
        }

        protected void AddStaticField(StaticField fieldInfo)
        {
            base.StaticFields.Add(fieldInfo.Info.Name, fieldInfo);
        }
    }
}
