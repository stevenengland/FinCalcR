using Caliburn.Micro;

namespace StEn.FinCalcR.WinUi.ViewModels
{
    public class ErrorViewModel : Screen
    {
        private string errorMessage;
        private string innerErrorMessage;
        private string shutdownMessage;

        public ErrorViewModel()
        {
        }

        public string ErrorMessage
        {
            get => this.errorMessage;
            set
            {
                this.errorMessage = value;
                this.NotifyOfPropertyChange(() => this.ErrorMessage);
            }
        }

        public string InnerErrorMessage
        {
            get => this.innerErrorMessage;
            set
            {
                this.innerErrorMessage = value;
                this.NotifyOfPropertyChange(() => this.InnerErrorMessage);
            }
        }

        public string ShutdownMessage
        {
            get => this.shutdownMessage;
            set
            {
                this.shutdownMessage = value;
                this.NotifyOfPropertyChange(() => this.ShutdownMessage);
            }
        }
    }
}
