using System.Threading.Tasks;
using JetBrains.Annotations;
using StEn.FinCalcR.WinUi.ViewModels;
using StEn.FinCalcR.WinUi.Views;

namespace StEn.FinCalcR.WinUi.LibraryMapper.DialogHost
{
    [UsedImplicitly] // Via DI
    public class DialogHostMapper : IDialogHostMapper
    {
        public Task<object> ShowAsync(object content, object dialogIdentifier) => MaterialDesignThemes.Wpf.DialogHost.Show(content, dialogIdentifier);

        public ErrorView GetErrorView(
            string errorMessage,
            string innerErrorMessage = "",
            string shutdownMessage = "") => new ErrorView()
            {
                DataContext = new ErrorViewModel()
                {
                    ErrorMessage = errorMessage,
                    InnerErrorMessage = innerErrorMessage,
                    ShutdownMessage = shutdownMessage,
                },
            };

        public HintView GetHintView(
            string message) => new HintView()
            {
                DataContext = new HintViewModel()
                {
                    Message = message,
                },
            };
    }
}
