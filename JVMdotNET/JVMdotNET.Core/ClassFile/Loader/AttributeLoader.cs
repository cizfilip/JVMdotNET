using JVMdotNET.Core.ClassFile.Attributes;
using JVMdotNET.Core.ClassFile.Attributes.ExceptionTable;
using JVMdotNET.Core.ClassFile.ConstantPool;
using JVMdotNET.Core.ClassFile.Utils;
using JVMdotNET.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JVMdotNET.Core.ClassFile.Loader
{
    internal class AttributeLoader
    {
        private ConstantPoolItemBase[] constantPool;

        public AttributeLoader(ConstantPoolItemBase[] constantPool)
        {
            this.constantPool = constantPool;
        }

        public IDictionary<string, AttributeBase> Load(BigEndianBinaryReader reader)
        {
            int attributesLength = reader.ReadUInt16();
            var attributes = new Dictionary<string, AttributeBase>(attributesLength);

            for (int i = 0; i < attributesLength; i++)
            {
                string attributeName;
                var attribute = LoadAttribute(reader, out attributeName);
                attributes.Add(attributeName, attribute);
            }

            return attributes;
        }

        private AttributeBase LoadAttribute(BigEndianBinaryReader reader, out string attributeName)
        {
            attributeName = constantPool.GetItem<Utf8ConstantPoolItem>(reader.ReadUInt16()).String;

            int attributeLength = reader.ReadInt32();

            switch(attributeName)
            {
                case ConstantValueAttribute.Name:
                    return LoadConstantValue(reader, attributeLength);
                case CodeAttribute.Name:
                    return LoadCode(reader, attributeLength);
                case StackMapTableAttribute.Name:

                    break;
                case ExceptionsAttribute.Name:

                    break;
                case InnerClassesAttribute.Name:

                    break;
                case EnclosingMethodAttribute.Name:

                    break;
                case SyntheticAttribute.Name:
                    return LoadSynthetic(reader, attributeLength);
                case SignatureAttribute.Name:
                    return LoadSignature(reader, attributeLength);
                case SourceFileAttribute.Name:
                    return LoadSourceFile(reader, attributeLength);
                case SourceDebugExtensionAttribute.Name:
                    //TODO: Implement attribute
                    SkipAttribute(reader, attributeLength);
                    break;
                case LineNumberTableAttribute.Name:

                    break;
                case LocalVariableTableAttribute.Name:

                    break;
                case LocalVariableTypeTableAttribute.Name:

                    break;
                case DeprecatedAttribute.Name:
                    return LoadDeprecated(reader, attributeLength);
                case RuntimeVisibleAnnotationsAttribute.Name:

                    break;
                case RuntimeInvisibleAnnotationsAttribute.Name:

                    break;
                case RuntimeVisibleParameterAnnotationsAttribute.Name:

                    break;
                case RuntimeInvisibleParameterAnnotationsAttribute.Name:

                    break;
                case AnnotationDefaultAttribute.Name:

                    break;
                case BootstrapMethodsAttribute.Name:

                    break;

                default:
                    //TODO: silent ignore (but log)
                    //skip length of the attribute
                    SkipAttribute(reader, attributeLength);
                    break;
            }

            return null;
        }

        

        private void SkipAttribute(BigEndianBinaryReader reader, int attributeLength)
        {
            reader.ReadBytes(attributeLength);
        }
        
        private void ValidateLength(int actualLength, int expectedLength)
        {
            if (actualLength != expectedLength)
            {
                throw new ClassFileFormatException("Invalid attribute length.");
            }
        }

        private AttributeBase LoadConstantValue(BigEndianBinaryReader reader, int attributeLength)
        {
            ValidateLength(attributeLength, 2);

            int constantIndex = reader.ReadUInt16();

            var item = constantPool.GetItem<ValueConstantPoolItem>(constantIndex);

            return new ConstantValueAttribute(item.GetValue());
        }

        private AttributeBase LoadCode(BigEndianBinaryReader reader, int attributeLength)
        {
            int maxStack = reader.ReadUInt16();
            int maxLocals = reader.ReadUInt16();

            int codeLength = reader.ReadInt32();
            byte[] code = reader.ReadBytes(codeLength);

            int exceptionTableLength = reader.ReadUInt16();
            var exceptionTable = new ExceptionTableEntry[exceptionTableLength];

            for (int i = 0; i < exceptionTableLength; i++)
            {
                int startPC = reader.ReadUInt16();
                int endPC = reader.ReadUInt16();
                int handlerPC = reader.ReadUInt16();
                int catchTypeIndex = reader.ReadUInt16();

                exceptionTable[i] = new ExceptionTableEntry(startPC, endPC, handlerPC, catchTypeIndex);
            }

            var attributes = Load(reader);

            return new CodeAttribute(maxStack, maxLocals, code, exceptionTable, attributes);
        }

        private AttributeBase LoadSourceFile(BigEndianBinaryReader reader, int attributeLength)
        {
            ValidateLength(attributeLength, 2);

            int constantIndex = reader.ReadUInt16();
            return new SourceFileAttribute(constantPool.GetItem<Utf8ConstantPoolItem>(constantIndex).String);
        }

        private AttributeBase LoadDeprecated(BigEndianBinaryReader reader, int attributeLength)
        {
            ValidateLength(attributeLength, 0);
            return new DeprecatedAttribute();
        }

        private AttributeBase LoadSynthetic(BigEndianBinaryReader reader, int attributeLength)
        {
            ValidateLength(attributeLength, 0);
            return new SyntheticAttribute();
        }

        private AttributeBase LoadSignature(BigEndianBinaryReader reader, int attributeLength)
        {
            ValidateLength(attributeLength, 2);

            int constantIndex = reader.ReadUInt16();
            return new SignatureAttribute(constantPool.GetItem<Utf8ConstantPoolItem>(constantIndex).String);
        }
    }
}
