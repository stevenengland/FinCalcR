using System.Threading.Tasks;
using StEn.FinCalcR.WinUi.ViewModels;
using StEn.FinCalcR.WinUi.Views;

namespace StEn.FinCalcR.WinUi.LibraryMapper.DialogHost
{
    public class DialogHostMapper : IDialogHostMapper
    {
        public Task<object> ShowAsync(object content, object dialogIdentifier)
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
