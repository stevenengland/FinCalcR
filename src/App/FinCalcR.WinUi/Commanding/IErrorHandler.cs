using System;

namespace StEn.FinCalcR.WinUi.Commanding
{
    public interface IErrorHandler
    {
        void HandleError(Exception ex);
    }
}
