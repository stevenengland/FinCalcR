using Xunit;

namespace FinCalcR.WinUi.Tests.ViewModelTests.FinCalcViewModelTestCollections.IntegrationTests
{
    public class EndBtnShould : TestBase
    {
        [Theory]
        [InlineData("10,00", new[] { Ca.Nr2, Ca.Nr0, Ca.Nr0, Ca.OpM, Ca.Nr5, Ca.Alg, Ca.Alg, Ca.SetEnd })]
        [InlineData("10,00", new[] { Ca.Nr2, Ca.Nr0, Ca.Nr0, Ca.OpM, Ca.Nr5, Ca.Dec, Ca.SetEnd })]
        public void TriggerPercentCalculation_WhenAllConditionsAreMet(string expectedOutputTextAfterAllOperations, Ca[] actions) => FinCalcViewModelHelper.ExecuteDummyActionsAndCheckOutput(actions, expectedOutputTextAfterAllOperations);
    }
}
