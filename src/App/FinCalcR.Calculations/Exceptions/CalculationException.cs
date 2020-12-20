using System;

namespace StEn.FinCalcR.Calculations.Exceptions
{
	public class CalculationException : Exception
	{
		public CalculationException(string message)
			: base(message)
		{
			this.Message = message;
		}

		public override string Message { get; }
	}
}
