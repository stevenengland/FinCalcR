using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace StEn.FinCalcR.WinUi.Events.EventArgs
{
	public class MappedKeyEventArgs
	{
		public MappedKeyEventArgs(string key, object activeWindowContent)
		{
			this.Key = key;
			this.ActiveWindowContent = activeWindowContent;
		}

		public string Key { get; }

		public bool IsShiftPressed { get; set; }

		public object ActiveWindowContent { get; }
	}
}
