using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StEn.FinCalcR.WinUi.Types
{
	public enum LastPressedOperation
	{
		None,
		AlgebSign,
		Digit,
		Calculate,
		Operator,
		Decimal,
		Clear,

		// Special Functions
		Interest,
	}
}
