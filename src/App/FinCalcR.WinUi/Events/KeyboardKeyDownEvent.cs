using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StEn.FinCalcR.WinUi.Events.EventArgs;

namespace StEn.FinCalcR.WinUi.Events
{
	public class KeyboardKeyDownEvent
	{
		public KeyboardKeyDownEvent(MappedKeyEventArgs e)
		{
			this.KeyEventArgs = e;
		}

		public MappedKeyEventArgs KeyEventArgs { get; }
	}
}
