using JVMdotNET.Core.ClassFile.Attributes.ExceptionTable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JVMdotNET.Core.ClassFile.Attributes
{
    internal class CodeAttribute : AttributeBase, IAttributteContainer
    {
        private static readonly string[] ValidAttributeNames = { 
                                                                   LineNumberTableAttribute.Name,
                                                                   LocalVariableTableAttribute.Name, 
                                                                   LocalVariableTypeTableAttribute.Name,
                                                                   StackMapTableAttribute.Name
                                                               };

        internal const string Name = "Code";

        private IDictionary<string, AttributeBase> attributes;

        public int MaxStack { get; private set; }
        public int MaxLocals { get; private set; }

        public byte[] Code { get; private set; }

        public ExceptionTableEntry[] ExceptionTable { get; private set; }

        public CodeAttribute(int maxStack, int maxLocals, byte[] code, 
            ExceptionTableEntry[] exceptionTable, IDictionary<string, AttributeBase> attributes)
        {
            this.MaxStack = maxStack;
            this.MaxLocals = maxLocals;
            this.Code = code;
            this.ExceptionTable = exceptionTable;
            this.attributes = attributes;
        }


        public string[] ValidAttributes
        {
            get { return ValidAttributeNames; }
        }

        public T GetAttribute<T>(string attributeName) where T : AttributeBase
        {
            return attributes[attributeName] as T;
        }
    }
}
