using System;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Caliburn.Micro;
using StEn.FinCalcR.Common.LanguageResources;
using StEn.FinCalcR.Common.Services.Localization;
using StEn.FinCalcR.WinUi.Commanding;

namespace StEn.FinCalcR.WinUi.ViewModels
{
	public class FinCalcViewModel : Screen
	{
		private ILocalizationService localizationService;
		private string resultText = "0";

		public FinCalcViewModel(ILocalizationService localizationService)
		{
			this.localizationService = localizationService;
		}


		public string ResultText
		{
			get => this.resultText;
			set
			{
				this.resultText = value;
				this.NotifyOfPropertyChange(() => this.ResultText);
			}
		}

		public string ThousandsSeparator => Resources.CALC_THOUSANDS_SEPARATOR;

		public string DecimalSeparator => Resources.CALC_DECIMAL_SEPARATOR;

		public ICommand DigitPressedCommand => new SyncCommand<object>(this.OnDigitPressed);

		private void OnDigitPressed(object digit)
		{
			throw new NotImplementedException();
		}
	}
}
