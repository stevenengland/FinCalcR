using System;
using System.Collections.Generic;
using System.Text;

namespace StEn.FinCalcR.Calculations
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
