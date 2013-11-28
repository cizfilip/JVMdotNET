using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JVMdotNET.Core.ClassFile.Attributes.ExceptionTable
{
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
