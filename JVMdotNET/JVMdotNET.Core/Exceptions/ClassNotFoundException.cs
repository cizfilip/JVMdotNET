using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace JVMdotNET.Core.Exceptions
{
    [Serializable]
    public class ClassNotFoundException : Exception
    {
        public ClassNotFoundException()
        {
        }

        public ClassNotFoundException(string message)
            : base(message)
        {
        }

        public ClassNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected ClassNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
    
}
