using System;
using System.Runtime.Serialization;

namespace StEn.FinCalcR.Calculations.Exceptions
{
    [Serializable]
    public class CalculationException : Exception
    {
        public CalculationException(string message)
            : base(message)
        {
            this.Message = message;
        }

        public CalculationException()
        {
        }

        public CalculationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        // Without this constructor, deserialization will fail
        protected CalculationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public override string Message { get; }
    }
}
