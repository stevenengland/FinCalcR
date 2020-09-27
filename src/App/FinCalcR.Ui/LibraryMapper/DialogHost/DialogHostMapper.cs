using System.Threading.Tasks;
using StEn.FinCalcR.Ui.ViewModels;
using StEn.FinCalcR.Ui.Views;

namespace StEn.FinCalcR.Ui.LibraryMapper.DialogHost
{
	public class DialogHostMapper : IDialogHostMapper
	{
		public Task<object> Show(object content, object dialogIdentifier)
		{
			return MaterialDesignThemes.Wpf.DialogHost.Show(content, dialogIdentifier);
		}

		public ErrorView GetErrorView(
			string errorMessage,
			string innerErrorMessage = "",
			string shutdownMessage = "")
		{
			return new ErrorView()
			{
				DataContext = new ErrorViewModel()
				{
					ErrorMessage = errorMessage,
					InnerErrorMessage = innerErrorMessage,
					ShutdownMessage = shutdownMessage,
				},
			};
		}

		public HintView GetHintView(
			string message)
		{
			return new HintView()
			{
				DataContext = new HintViewModel()
				{
					Message = message,
				},
			};
		}
	}
}
