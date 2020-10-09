using System;
using System.Threading.Tasks;

namespace StEn.FinCalcR.WinUi.Types
{
	public interface IGestureHandler
	{
		Task<bool> IsLongTouchAsync(TimeSpan duration);
	}
}
