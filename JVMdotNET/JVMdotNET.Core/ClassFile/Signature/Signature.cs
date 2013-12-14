using JVMdotNET.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JVMdotNET.Core.ClassFile.Signature
{
    internal class Signature
    {
        public static MethodSignature ParseMethodSignature(string descriptor)
        {
            string[] parts = descriptor.Split(new char[] { '(', ')' }, StringSplitOptions.None);
            if (!(parts.Length == 3 && string.IsNullOrEmpty(parts[0])))
            {
                throw new ClassFileFormatException(string.Format("Invalid signature {0}", descriptor));
            }

            var parameters = ParseTypes(parts[1]);
            var returnType = ParseSingleType(parts[2]);

            return new MethodSignature(returnType, parameters);
        }

        public static FieldSignature ParseFieldSignature(string descriptor)
        {
            return new FieldSignature(ParseSingleType(descriptor));
        }

        public static ArrayTypeInfo ParseArrayType(string descriptor)
        {
            TypeInfo arrayTypeInfo = ParseSingleType(descriptor);

            if (arrayTypeInfo is ArrayTypeInfo)
            {
                return (ArrayTypeInfo)arrayTypeInfo;
            }

            throw new ArgumentException("Specified descriptor was not array type descriptor!");
        }

        private static TypeInfo[] ParseTypes(string descriptorPart)
        {
            List<TypeInfo> parsedTypes = new List<TypeInfo>();

            int startIndex = 0;
            while (startIndex < descriptorPart.Length)
            {
                parsedTypes.Add(ParseType(descriptorPart, ref startIndex));
            }
            
            return parsedTypes.ToArray();
        }

        private static TypeInfo ParseType(string descriptorPart, ref int startIndex)
        {
            if (string.IsNullOrEmpty(descriptorPart))
            {
                throw new ArgumentException("descriptorPart can not be empty!");
            }
            
            StringBuilder nameBuilder = new StringBuilder();
            char ch = descriptorPart[startIndex++];
            switch (ch)
            {
                case 'B':
                    return new TypeInfo(JVMType.Byte);
                case 'C':
                    return new TypeInfo(JVMType.Char);
                case 'D':
                    return new TypeInfo(JVMType.Double);
                case 'F':
                    return new TypeInfo(JVMType.Float);
                case 'I':
                    return new TypeInfo(JVMType.Int);
                case 'J':
                    return new TypeInfo(JVMType.Long);
                case 'V':
                    return new TypeInfo(JVMType.Void);
                case 'S':
                    return new TypeInfo(JVMType.Short);
                case 'Z':
                    return new TypeInfo(JVMType.Boolean);
                case 'L':
                    nameBuilder.Clear();
                    while (descriptorPart[startIndex] != ';')
                    {
                        nameBuilder.Append(descriptorPart[startIndex]);
                        startIndex++;
                    }
                    startIndex++;
                    return new ObjectTypeInfo(JVMType.Object, nameBuilder.ToString());
                case '[':
                    int dimensions = 1;
                    while (descriptorPart[startIndex] == '[')
                    {
                        dimensions++;
                        startIndex++;
                    }
                    var elementType = ParseType(descriptorPart, ref startIndex);
                    return new ArrayTypeInfo(JVMType.Array, dimensions, elementType);

                default:
                    throw new ClassFileFormatException(string.Format("Unknown type in signature '{0}'", ch));
            }
        }

        private static TypeInfo ParseSingleType(string descriptorPart)
        {
            int startIndex = 0;
            return ParseType(descriptorPart, ref startIndex);
        }
    }

    
}
