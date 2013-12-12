using JVMdotNET.Core.ClassFile.Attributes;
using JVMdotNET.Core.ClassFile.ConstantPool;
using JVMdotNET.Core.ClassFile.Utils;
using JVMdotNET.Core.Exceptions;
using System.Collections.Generic;
using System.IO;

namespace JVMdotNET.Core.ClassFile.Loader
{
    internal class ClassFileLoader
    {
        private ConstantPoolItemBase[] constantPool;

        private ConstantPoolLoader constantPoolLoader;
        private AttributeLoader attributeLoader;

        public JavaClass Load(Stream input)
        {
            this.constantPoolLoader = new ConstantPoolLoader();
            var reader = new BigEndianBinaryReader(input);

            //Magic check
            uint magic = reader.ReadUInt32();
            if (magic != 0xCAFEBABE)
                throw new ClassFileFormatException("Invalid magic number (0xCAFEBABE).");

            //Class file version
            var versionInfo = new VersionInfo(reader.ReadUInt16(), reader.ReadUInt16());

            //ConstantPool Loading
            this.constantPool = constantPoolLoader.Load(reader);

            this.attributeLoader = new AttributeLoader(constantPool);

            //Class access flags
            var accessFlags = (ClassAccessFlags)reader.ReadUInt16();

            //this class
            string thisClass = constantPool.GetItem<ClassConstantPoolItem>(reader.ReadUInt16()).Name;

            //super class
            string superClass = constantPool.GetItem<ClassConstantPoolItem>(reader.ReadUInt16()).Name;

            //interfaces
            string[] interfaces = LoadInterfaces(reader);

            //create class
            JavaClass javaClass = new JavaClass(versionInfo, constantPool, accessFlags, thisClass, superClass, interfaces);

            //fields
            FieldInfo[] fields = LoadFields(reader, javaClass);
            javaClass.AddFields(fields);

            //methods
            MethodInfo[] methods = LoadMethods(reader, javaClass);
            javaClass.AddMethods(methods);

            //attributes
            IDictionary<string, AttributeBase> attributes = attributeLoader.Load(reader);
            javaClass.Attributes = attributes;

            return javaClass;
        }

        private MethodInfo[] LoadMethods(BigEndianBinaryReader reader, JavaClass javaClass)
        {
            int methodsLength = reader.ReadUInt16();
            MethodInfo[] methods = new MethodInfo[methodsLength];

            for (int i = 0; i < methodsLength; i++)
            {
                methods[i] = LoadMethodInfo(reader, javaClass);
            }

            return methods;
        }

        private MethodInfo LoadMethodInfo(BigEndianBinaryReader reader, JavaClass javaClass)
        {
            MethodAccessFlags flags = (MethodAccessFlags)reader.ReadUInt16();
            string name = constantPool.GetItem<Utf8ConstantPoolItem>(reader.ReadUInt16()).String;
            string descriptor = constantPool.GetItem<Utf8ConstantPoolItem>(reader.ReadUInt16()).String;

            var attributes = attributeLoader.Load(reader);

            return new MethodInfo(javaClass, flags, name, descriptor, attributes);
        }

        private FieldInfo[] LoadFields(BigEndianBinaryReader reader, JavaClass javaClass)
        {
            int fieldsLength = reader.ReadUInt16();
            FieldInfo[] fields = new FieldInfo[fieldsLength];

            for (int i = 0; i < fieldsLength; i++)
            {
                fields[i] = LoadFieldInfo(reader, javaClass);
            }

            return fields;
        }

        private FieldInfo LoadFieldInfo(BigEndianBinaryReader reader, JavaClass javaClass)
        {
            FieldAccessFlags flags = (FieldAccessFlags)reader.ReadUInt16();
            string name = constantPool.GetItem<Utf8ConstantPoolItem>(reader.ReadUInt16()).String;
            string descriptor = constantPool.GetItem<Utf8ConstantPoolItem>(reader.ReadUInt16()).String;

            var attributes = attributeLoader.Load(reader);

            return new FieldInfo(javaClass, flags, name, descriptor, attributes);
        }

        private string[] LoadInterfaces(BigEndianBinaryReader reader)
        {
            int interfacesLength = reader.ReadUInt16();
            var interfaces = new string[interfacesLength];

            for (int i = 0; i < interfacesLength; i++)
            {
                interfaces[i] = constantPool.GetItem<ClassConstantPoolItem>(reader.ReadUInt16()).Name;
            }

            return interfaces;
        }


    }
}
