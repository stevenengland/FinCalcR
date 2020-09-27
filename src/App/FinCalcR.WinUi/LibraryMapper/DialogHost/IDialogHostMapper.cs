using System.Threading.Tasks;
using StEn.FinCalcR.WinUi.Views;

namespace StEn.FinCalcR.WinUi.LibraryMapper.DialogHost
{
	public interface IDialogHostMapper
	{
		Task<object> Show(object content, object dialogIdentifier);

		ErrorView GetErrorView(
			string errorMessage,
			string innerErrorMessage = "",
			string shutdownMessage = "");

		HintView GetHintView(string message);
	}
}
