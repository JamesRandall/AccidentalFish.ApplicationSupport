using System;
using System.Runtime.Serialization;

namespace NewApplicationResources
{
    internal class ConsoleAppException : Exception
    {
        public ConsoleAppException()
        {
        }

        public ConsoleAppException(string message) : base(message)
        {
        }

        public ConsoleAppException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ConsoleAppException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
