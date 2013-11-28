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

        public IDictionary<string, AttributeBase> Attributes { get; private set; }

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
            this.Attributes = attributes;
        }


        public string[] ValidAttributes
        {
            get { return ValidAttributeNames; }
        }

        public T GetAttribute<T>(string attributeName) where T : AttributeBase
        {
            throw new NotImplementedException();
        }
    }


    internal class ExceptionTableEntry
    {
        public int StartPC { get; private set; }
        public int EndPC { get; private set; }
        public int HandlerPC { get; private set; }
        public int CatchTypeIndex { get; private set; }

        public ExceptionTableEntry(int startPC, int endPC, int handlerPC, int catchTypeIndex)
        {
            this.StartPC = startPC;
            this.EndPC = endPC;
            this.HandlerPC = handlerPC;
            this.CatchTypeIndex = catchTypeIndex;
        }
    }
}
