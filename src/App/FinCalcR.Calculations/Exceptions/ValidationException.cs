using System;

namespace StEn.FinCalcR.Calculations.Exceptions
{
    public class ValidationException : Exception
    {
        public ValidationException(string message)
            : base(message)
        {
            this.Message = message;
        }

        public override string Message { get; }
    }
}
