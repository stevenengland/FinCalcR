using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StEn.FinCalcR.WinUi.Types
{
	public interface IGestureHandler
	{
		Task<bool> IsLongTouchAsync(TimeSpan duration);
	}
}
