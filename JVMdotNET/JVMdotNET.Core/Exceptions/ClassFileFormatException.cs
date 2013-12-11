using System;
using System.Runtime.Serialization;

namespace JVMdotNET.Core.Exceptions
{
    [Serializable]
    public class ClassFileFormatException : Exception
    {
        public ClassFileFormatException()
        {
        }

        public ClassFileFormatException(string message)
            : base(message)
        {
        }

        public ClassFileFormatException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected ClassFileFormatException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }

}
