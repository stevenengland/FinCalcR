using Caliburn.Micro;

namespace StEn.FinCalcR.WinUi.ViewModels
{
	public class HintViewModel : Screen
	{
		private string message;

		public string Message
		{
			get => this.message;
			set
			{
				this.message = value;
				this.NotifyOfPropertyChange(() => this.Message);
			}
		}
	}
}
