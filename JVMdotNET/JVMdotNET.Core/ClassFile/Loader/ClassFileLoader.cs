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
    public class ClassFileLoader
    {
        public ClassFile Load(Stream input)
        {
            var reader = new BigEndianBinaryReader(input);

            //Magic check
            uint magic = reader.ReadUInt32();
            if (magic != 0xCAFEBABE)
                throw new ClassFileFormatException("Invalid magic number (0xCAFEBABE).");

            //Class file version
            var versionInfo = new VersionInfo(reader.ReadUInt16(), reader.ReadUInt16());

            //ConstantPool Loading
            var constantPool = LoadConstantPool(reader);
            
            //Class access flags
            var accessFlag = (ClassAccessFlag)reader.ReadUInt16();

            //this class
            int thisIndex = reader.ReadUInt16();

            //super class
            int superIndex = reader.ReadUInt16();

            //interfaces
            int[] interfaces = LoadInterfaces(reader);

            //fields
            FieldInfo[] fields = LoadFields(reader);

            return null;
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
            FieldAccessFlag flag = (FieldAccessFlag)reader.ReadUInt16();
            int nameIndex = reader.ReadUInt16() - 1;
            int descriptorIndex = reader.ReadUInt16() - 1;

            //TODO: Load Attributes

            return new FieldInfo(flag, nameIndex, descriptorIndex);
        }

        private int[] LoadInterfaces(BigEndianBinaryReader reader)
        {
            int interfacesLength = reader.ReadUInt16();
            int[] interfaces = new int[interfacesLength];

            for (int i = 0; i < interfacesLength; i++)
            {
                interfaces[i] = reader.ReadUInt16() - 1;
            }

            return interfaces;
        }

        private ConstantPoolItem[] LoadConstantPool(BigEndianBinaryReader reader)
        {
            int constantPoolLength = reader.ReadUInt16() - 1; //constant_pool_count item is equal to the number of entries in the  constant_pool table plus one.
            ConstantPoolItem[] constantPool = new ConstantPoolItem[constantPoolLength];
            bool wasTwoConstantPoolEntries;
            for (int i = 0; i < constantPoolLength; i++)
            {
                constantPool[i] = LoadConstantPoolItem(reader, out wasTwoConstantPoolEntries);
                if (wasTwoConstantPoolEntries)
                {
                    i++;
                }
            }
            return constantPool;
        }

        private ConstantPoolItem LoadConstantPoolItem(BigEndianBinaryReader reader, out bool wasTwoConstantPoolEntries)
        {
            ConstantPoolItemType tag = (ConstantPoolItemType)reader.ReadByte();
            wasTwoConstantPoolEntries = false;

            switch (tag)
            {
                case ConstantPoolItemType.Class:
                    return new ClassConstantPoolItem(reader.ReadUInt16());
                case ConstantPoolItemType.FieldRef:
                    return new FieldRefConstantPoolItem(reader.ReadUInt16(), reader.ReadUInt16());
                case ConstantPoolItemType.MethodRef:
                    return new MethodRefConstantPoolItem(reader.ReadUInt16(), reader.ReadUInt16());
                case ConstantPoolItemType.InterfaceMethodRef:
                    return new InterfaceMethodRefConstantPoolItem(reader.ReadUInt16(), reader.ReadUInt16());
                case ConstantPoolItemType.String:
                    return new StringConstantPoolItem(reader.ReadUInt16());
                case ConstantPoolItemType.Integer:
                    return new IntegerConstantPoolItem(reader.ReadInt32());
                case ConstantPoolItemType.Float:
                    return new FloatConstantPoolItem(reader.ReadSingle());
                case ConstantPoolItemType.Long:
                    wasTwoConstantPoolEntries = true;
                    return new LongConstantPoolItem(reader.ReadInt64());
                case ConstantPoolItemType.Double:
                    wasTwoConstantPoolEntries = true;
                    return new DoubleConstantPoolItem(reader.ReadDouble());
                case ConstantPoolItemType.NameAndType:
                    return new NameAndTypeConstantPoolItem(reader.ReadUInt16(), reader.ReadUInt16());
                case ConstantPoolItemType.Utf8:
                    return new Utf8ConstantPoolItem(reader.ReadString());
                // Vše níž až od major version 51
                case ConstantPoolItemType.MethodHandle:
                    return new MethodHandleConstantPoolItem(reader.ReadByte(), reader.ReadUInt16());
                case ConstantPoolItemType.MethodType:
                    return new MethodTypeConstantPoolItem(reader.ReadUInt16());
                case ConstantPoolItemType.InvokeDynamic:
                    return new InvokeDynamicConstantPoolItem(reader.ReadUInt16(), reader.ReadUInt16());

                default:
                    throw new ClassFileFormatException(string.Format("Unknown constant pool tag {0}.", tag));
            }
        }
    }
}
