using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace JVMdotNET.Core.Exceptions
{
    [Serializable]
    public class FieldNotFoundException : Exception
    {
        public FieldNotFoundException()
        {
        }

        public FieldNotFoundException(string message)
            : base(message)
        {
        }

        public FieldNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected FieldNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
