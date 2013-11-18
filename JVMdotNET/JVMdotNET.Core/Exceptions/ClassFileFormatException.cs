using System;
using System.Runtime.Serialization;

namespace JVMdotNET.Core.Exceptions
{
    /// <summary>
    /// Represents errors that occur inside the Ef Model Migrations pipeline.
    /// </summary>
    [Serializable]
    public class ClassFileFormatException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the ModelMigrationException class.
        /// </summary>
        public ClassFileFormatException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the ModelMigrationException class.
        /// </summary>
        /// <param name="message"> The message that describes the error. </param>
        public ClassFileFormatException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the ModelMigrationException class.
        /// </summary>
        /// <param name="message"> The message that describes the error. </param>
        /// <param name="innerException"> The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified. </param>
        public ClassFileFormatException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the ModelMigrationException class with serialized data.
        /// </summary>
        /// <param name="info">
        /// The <see cref="SerializationInfo" /> that holds the serialized object data about the exception being thrown.
        /// </param>
        /// <param name="context">
        /// The <see cref="StreamingContext" /> that contains contextual information about the source or destination.
        /// </param>
        protected ClassFileFormatException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }

}
