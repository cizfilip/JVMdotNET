using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace JVMdotNET.Core.Exceptions
{
    [Serializable]
    public class UnhandledJavaException : Exception
    {
        public UnhandledJavaException()
        {
        }

        public UnhandledJavaException(string message)
            : base(message)
        {
        }

        public UnhandledJavaException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected UnhandledJavaException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
