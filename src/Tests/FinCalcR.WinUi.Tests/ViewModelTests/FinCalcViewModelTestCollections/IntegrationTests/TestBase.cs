using System.Globalization;
using System.Threading;

namespace FinCalcR.WinUi.Tests.ViewModelTests.FinCalcViewModelTestCollections.IntegrationTests
{
    public class TestBase
    {
        public TestBase()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("de");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("de");
        }
    }
}
