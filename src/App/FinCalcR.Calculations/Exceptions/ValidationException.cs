using System;
using System.Runtime.Serialization;

namespace StEn.FinCalcR.Calculations.Exceptions
{
    [Serializable]
    public class ValidationException : Exception
    {
        public ValidationException(string message)
            : base(message)
        {
            this.Message = message;
        }

        public ValidationException()
        {
        }

        public ValidationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        // Without this constructor, deserialization will fail
        protected ValidationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public override string Message { get; }
    }
}
