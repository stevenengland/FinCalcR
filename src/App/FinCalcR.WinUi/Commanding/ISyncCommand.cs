using System.Windows.Input;

namespace StEn.FinCalcR.WinUi.Commanding
{
    public interface ISyncCommand : ICommand
    {
        void ExecuteSync();

        bool CanExecute();
    }

    public interface ISyncCommand<in T> : ICommand
    {
        void ExecuteSync(T parameter);

        bool CanExecute(T parameter);
    }
}
