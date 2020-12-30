using System;
using System.Threading.Tasks;

namespace StEn.FinCalcR.WinUi.Types
{
	public interface IGestureHandler
	{
		Task<bool> IsLongMouseClickAsync(TimeSpan duration);

		Task<bool> IsLongTouchAsync(TimeSpan duration);
	}
}
