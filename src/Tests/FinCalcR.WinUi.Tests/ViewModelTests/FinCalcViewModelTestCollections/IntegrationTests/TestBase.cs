using System.Globalization;
using System.Threading;

namespace FinCalcR.WinUi.Tests.ViewModelTests.FinCalcViewModelTestCollections.IntegrationTests
{
    public class TestBase
    {
        protected TestBase()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("de");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("de");
        }

        protected double Tolerance { get; set; } = 0.00000001;
    }
}
