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
