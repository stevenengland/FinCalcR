using System.Threading.Tasks;
using StEn.FinCalcR.Ui.Views;

namespace StEn.FinCalcR.Ui.LibraryMapper.DialogHost
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
