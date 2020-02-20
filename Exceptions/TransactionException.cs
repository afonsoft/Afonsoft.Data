using System;
using System.Runtime.Serialization;

namespace Afonsoft.Data.Exceptions
{
    public class TransactionException : SystemException
    {
        public TransactionException() : base() { }
        public TransactionException(string message) : base(message) { }
        public TransactionException(string message, Exception innerException) : base(message, innerException) { }
        protected TransactionException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}