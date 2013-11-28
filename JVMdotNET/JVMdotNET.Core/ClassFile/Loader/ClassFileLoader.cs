using JVMdotNET.Core.ClassFile.Attributes;
using JVMdotNET.Core.ClassFile.ConstantPool;
using JVMdotNET.Core.ClassFile.Utils;
using JVMdotNET.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JVMdotNET.Core.ClassFile.Loader
{
    //TODO: Stop metoda na threadu vyvolava asynchroni exception nejak resit az budu delat thready viz specifikace- kap. 2.10.
    internal class ClassFileLoader
    {
        private ClassFile classFile;

        private ConstantPoolItemBase[] constantPool;

        private ConstantPoolLoader constantPoolLoader;
        private AttributeLoader attributeLoader;

        public ClassFile Load(Stream input)
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
            //TODO: validation
            //if (IsInterface && (super_class == 0 || this.SuperClass != "java.lang.Object"))
            //{
            //    throw new ClassFormatError("{0} (Interfaces must have java.lang.Object as superclass)", Name);
            //}

            //interfaces
            string[] interfaces = LoadInterfaces(reader);


            //fields
            FieldInfo[] fields = LoadFields(reader);

            //methods
            MethodInfo[] methods = LoadMethods(reader);

            IDictionary<string, AttributeBase> attributes = attributeLoader.Load(reader);
            


            //Remove utf8 items (not needed after all is loaded and resolved) or maybe needed :)
            //for (int i = 0; i < constantPool.Length; i++)
            //{
            //    var item = constantPool[i];
            //    if (item != null && item.Type == ConstantPoolItemType.Utf8)
            //    {
            //        constantPool[i] = null;
            //    }
            //}

            return new ClassFile(versionInfo, constantPool, accessFlags, thisClass, superClass, interfaces, fields, methods, attributes);
        }

        private MethodInfo[] LoadMethods(BigEndianBinaryReader reader)
        {
            int methodsLength = reader.ReadUInt16();
            MethodInfo[] methods = new MethodInfo[methodsLength];

            for (int i = 0; i < methodsLength; i++)
            {
                methods[i] = LoadMethodInfo(reader);
            }

            return methods;
        }

        private MethodInfo LoadMethodInfo(BigEndianBinaryReader reader)
        {
            //TODO: Checkovat zda-li plati pri nacitani metody init nasledujici:
            //In a  class file whose version number is 51.0 or above, the method must
            //additionally have its  ACC_STATIC flag (§4.6) set in order to be the class or interface
            //initialization method.

            MethodAccessFlags flags = (MethodAccessFlags)reader.ReadUInt16();
            string name = constantPool.GetItem<Utf8ConstantPoolItem>(reader.ReadUInt16()).String;
            string descriptor = constantPool.GetItem<Utf8ConstantPoolItem>(reader.ReadUInt16()).String;

            var attributes = attributeLoader.Load(reader);

            return new MethodInfo(flags, name, descriptor, attributes);
        }

        private FieldInfo[] LoadFields(BigEndianBinaryReader reader)
        {
            int fieldsLength = reader.ReadUInt16();
            FieldInfo[] fields = new FieldInfo[fieldsLength];

            for (int i = 0; i < fieldsLength; i++)
            {
                fields[i] = LoadFieldInfo(reader);
            }

            return fields;
        }

        private FieldInfo LoadFieldInfo(BigEndianBinaryReader reader)
        {
            FieldAccessFlags flags = (FieldAccessFlags)reader.ReadUInt16();
            string name = constantPool.GetItem<Utf8ConstantPoolItem>(reader.ReadUInt16()).String;
            string descriptor = constantPool.GetItem<Utf8ConstantPoolItem>(reader.ReadUInt16()).String;

            var attributes = attributeLoader.Load(reader);

            return new FieldInfo(flags, name, descriptor, attributes);
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
