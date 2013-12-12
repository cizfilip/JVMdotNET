using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace JVMdotNET.Core.Exceptions
{
    [Serializable]
    public class AttributeNotFoundException : Exception
    {
        public AttributeNotFoundException()
        {
        }

        public AttributeNotFoundException(string message)
            : base(message)
        {
        }

        public AttributeNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected AttributeNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
