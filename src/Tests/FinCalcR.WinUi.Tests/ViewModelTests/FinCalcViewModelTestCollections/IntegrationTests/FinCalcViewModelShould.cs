using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using Xunit;

namespace FinCalcR.WinUi.Tests.ViewModelTests.FinCalcViewModelTestCollections.IntegrationTests
{
    public class FinCalcViewModelShould
    {
        public FinCalcViewModelShould()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("de");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("de");
        }

        [Fact]
        public void ShowExpectedOutputText_GivenVariousInputSequences()
        {
            var testData = new List<(Ca[] operations, string expectedOutputTextAfterAllOperations)>()
            {
                (new[] { Ca.Nr1, Ca.Dec, Ca.Dec, Ca.Nr2 }, "1,2"),
            };

            FinCalcViewModelHelper.ExecuteDummyActionsAndCheckOutput(testData);
        }
    }
}
