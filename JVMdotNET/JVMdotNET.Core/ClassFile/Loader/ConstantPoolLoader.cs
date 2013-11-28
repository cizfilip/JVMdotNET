using JVMdotNET.Core.ClassFile.ConstantPool;
using JVMdotNET.Core.ClassFile.Utils;
using JVMdotNET.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JVMdotNET.Core.ClassFile.Loader
{
    internal class ConstantPoolLoader
    {
        public ConstantPoolItemBase[] Load(BigEndianBinaryReader reader)
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
