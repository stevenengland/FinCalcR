using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace StEn.FinCalcR.WinUi.Events.EventArgs
{
	public class MappedKeyEventArgs
	{
		public MappedKeyEventArgs(string key)
		{
			this.Key = key;
		}

		public string Key { get; }

		public bool IsShiftPressed { get; set; }
	}
}
