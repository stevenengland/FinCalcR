using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FinCalcR.Tests.Shared.TestData;
using FinCalcR.WinUi.Tests.Mocks;
using StEn.FinCalcR.Calculations.Calculator.Commands;
using Xunit;

namespace FinCalcR.WinUi.Tests.ViewModelTests.FinCalcViewModelTestCollections.IntegrationTests
{
    public class FinancialCalculationShould
    {
        public FinancialCalculationShould()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("de");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("de");
        }

        [Theory(Skip = "Needs more implementation work. Algeb Sign & Assert")]
        [MemberData(nameof(FinancialCalculation.KnWanted), MemberType = typeof(FinancialCalculation))]
        public void CalculateAndDisplayKn_GivenAllNeededVariables(double m, double n, double p, double k0, double e, double expectedKn)
        {
            // Arrange
            var mockObjects = MockFactories.GetMockObjects();
            var vm = MockFactories.FinCalcViewModel2WithCalculatorImplementationFactory(mockObjects);

            // Act
            FinCalcViewModelHelper.SetFinancialValue(vm, m, CommandWord.RatesPerAnnum);
            FinCalcViewModelHelper.SetFinancialValue(vm, n, CommandWord.Years);
            FinCalcViewModelHelper.SetFinancialValue(vm, p, CommandWord.NominalInterestRate);
            FinCalcViewModelHelper.SetFinancialValue(vm, k0, CommandWord.Start);
            FinCalcViewModelHelper.SetFinancialValue(vm, e, CommandWord.Rate);
            vm.EndPressedCommand.Execute(false);

            // Assert
        }
    }
}
