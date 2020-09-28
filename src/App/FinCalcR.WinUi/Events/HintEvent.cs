namespace StEn.FinCalcR.WinUi.Events
{
	public class HintEvent
	{
		public HintEvent(string message)
		{
			this.Message = message;
		}

		public string Message { get; }
	}
}
