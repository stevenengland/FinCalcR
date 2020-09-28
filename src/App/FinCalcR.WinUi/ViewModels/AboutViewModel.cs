using System;
using Caliburn.Micro;

namespace StEn.FinCalcR.WinUi.ViewModels
{
	public class AboutViewModel : Screen
	{
		private string appVersionText;

		public AboutViewModel()
		{
			var assembly = System.Reflection.Assembly.GetExecutingAssembly();
			this.Version = assembly.GetName().Version;
			this.AppVersionText = this.Version.Major + "." + this.Version.Minor + "." + this.Version.Build;
		}

		public string AppVersionText
		{
			get => this.appVersionText;
			set
			{
				if (value == this.appVersionText)
				{
					return;
				}

				this.appVersionText = value;
				this.NotifyOfPropertyChange(() => this.AppVersionText);
			}
		}

		public Version Version
		{
			get;
			set;
		}
	}
}
