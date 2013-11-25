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
            this.constantPool = LoadConstantPool(reader);
            
            //Class access flags
            var accessFlag = (ClassAccessFlag)reader.ReadUInt16();

            //this class
            ClassConstantPoolItem thisClass = GetConstantPoolItem<ClassConstantPoolItem>(reader.ReadUInt16());
            
            //super class
            ClassConstantPoolItem superClass = GetConstantPoolItem<ClassConstantPoolItem>(reader.ReadUInt16());
            //TODO: validation
            //if (IsInterface && (super_class == 0 || this.SuperClass != "java.lang.Object"))
            //{
            //    throw new ClassFormatError("{0} (Interfaces must have java.lang.Object as superclass)", Name);
            //}

            //interfaces
            ClassConstantPoolItem[] interfaces = LoadInterfaces(reader);


            //fields
            FieldInfo[] fields = LoadFields(reader);



            //TODO: Checkovat zda-li plati pri nacitani metody init nasledujici:
            //In a  class file whose version number is 51.0 or above, the method must
            //additionally have its  ACC_STATIC flag (§4.6) set in order to be the class or interface
            //initialization method.


            //Remove utf8 items (not needed after all is loaded and resolved)
            //for (int i = 0; i < constantPool.Length; i++)
            //{
            //    var item = constantPool[i];
            //    if (item != null && item.Type == ConstantPoolItemType.Utf8)
            //    {
            //        constantPool[i] = null;
            //    }
            //}

            return null;
        }

        private T GetConstantPoolItem<T>(int index) where T : ConstantPoolItemBase
        {
            if (index <= 0 || index >= constantPool.Length)
            {
                throw new ClassFileFormatException("Constant pool index out of range.");
            }

            T result = constantPool[index] as T;

            if (result == null)
            {
                throw new ClassFileFormatException(string.Format("Invalid constant pool item at {0}.", index));
            }

            return result;
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
            string name = GetConstantPoolItem<Utf8ConstantPoolItem>(reader.ReadUInt16()).Value;
            string descriptor = GetConstantPoolItem<Utf8ConstantPoolItem>(reader.ReadUInt16()).Value;

            //TODO: Load Attributes

            return new FieldInfo(flag, name, descriptor);
        }

        private ClassConstantPoolItem[] LoadInterfaces(BigEndianBinaryReader reader)
        {
            int interfacesLength = reader.ReadUInt16();
            var interfaces = new ClassConstantPoolItem[interfacesLength];

            for (int i = 0; i < interfacesLength; i++)
            {
                interfaces[i] = GetConstantPoolItem<ClassConstantPoolItem>(reader.ReadUInt16());
            }

            return interfaces;
        }

        private ConstantPoolItemBase[] LoadConstantPool(BigEndianBinaryReader reader)
        {
            int constantPoolLength = reader.ReadUInt16(); //constant_pool_count item is equal to the number of entries in the  constant_pool table plus one.
            ConstantPoolItemBase[] constantPool = new ConstantPoolItemBase[constantPoolLength];
            bool wasTwoConstantPoolEntries;
            for (int i = 1; i < constantPoolLength; i++)
            {
                constantPool[i] = LoadConstantPoolItem(reader, out wasTwoConstantPoolEntries);
                if (wasTwoConstantPoolEntries)
                {
                    i++;
                }
            }
            
            //Resolve items in constant pool
            for (int i = 0; i < constantPoolLength; i++)
            {
                var item = constantPool[i];
                if (item != null)
                {
                    item.Resolve(constantPool, i);
                }
            }

            

            return constantPool;
        }

        private ConstantPoolItemBase LoadConstantPoolItem(BigEndianBinaryReader reader, out bool wasTwoConstantPoolEntries)
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
