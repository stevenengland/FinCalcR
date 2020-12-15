using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FinCalcR.Tests.Shared.TestData;
using FinCalcR.WinUi.Tests.Mocks;
using FluentAssertions;
using StEn.FinCalcR.Calculations;
using StEn.FinCalcR.Calculations.Calculator.Commands;
using Xunit;

namespace FinCalcR.WinUi.Tests.ViewModelTests.FinCalcViewModelTestCollections.IntegrationTests
{
    public class FinancialCalculationShould
    {
        private const double Tolerance = 0.01;

        public FinancialCalculationShould()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("de");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("de");
        }

        [Theory]
        [MemberData(nameof(FinancialCalculation.KnWanted), MemberType = typeof(FinancialCalculation))]
        public void CalculateAndDisplayKn_GivenAllNeededVariables(double m, double n, double p, double k0, double e, double expectedKn)
        {
            // Arrange
            var mockObjects = MockFactories.GetMockObjects();
            var vm = MockFactories.FinCalcViewModel2WithCalculatorImplementationFactory(mockObjects);

            // Since I am sharing the test data with the plain calculation tests I have to manually adjust this value:
            expectedKn *= -1;

            // Act
            FinCalcViewModelHelper.SetFinancialValue(vm, m, CommandWord.RatesPerAnnum);
            FinCalcViewModelHelper.SetFinancialValue(vm, n, CommandWord.Years);
            FinCalcViewModelHelper.SetFinancialValue(vm, p, CommandWord.NominalInterestRate);
            FinCalcViewModelHelper.SetFinancialValue(vm, k0, CommandWord.Start);
            FinCalcViewModelHelper.SetFinancialValue(vm, e, CommandWord.Rate);
            vm.EndPressedCommand.Execute(false);

            // Assert
            vm.DisplayNumber.Should().BeApproximately(expectedKn, Tolerance);
        }

        [Theory]
        [MemberData(nameof(FinancialCalculation.K0Wanted), MemberType = typeof(FinancialCalculation))]
        public void CalculateAndDisplayK0_GivenAllNeededVariables(double m, double n, double p, double e, double kn, double expectedK0)
        {
            // Arrange
            var mockObjects = MockFactories.GetMockObjects();
            var vm = MockFactories.FinCalcViewModel2WithCalculatorImplementationFactory(mockObjects);

            // Act
            FinCalcViewModelHelper.SetFinancialValue(vm, m, CommandWord.RatesPerAnnum);
            FinCalcViewModelHelper.SetFinancialValue(vm, n, CommandWord.Years);
            FinCalcViewModelHelper.SetFinancialValue(vm, p, CommandWord.NominalInterestRate);
            FinCalcViewModelHelper.SetFinancialValue(vm, e, CommandWord.Rate);
            FinCalcViewModelHelper.SetFinancialValue(vm, kn, CommandWord.End);
            vm.StartPressedCommand.Execute(false);

            // Assert
            vm.DisplayNumber.Should().BeApproximately(expectedK0, Tolerance);
        }

        [Theory]
        [MemberData(nameof(FinancialCalculation.EWanted), MemberType = typeof(FinancialCalculation))]
        public void CalculateAndDisplayE_GivenAllNeededVariables(bool advance, double m, double n, double p, double k0, double kn, double expectedE)
        {
            // Arrange
            var mockObjects = MockFactories.GetMockObjects();
            var vm = MockFactories.FinCalcViewModel2WithCalculatorImplementationFactory(mockObjects);

            // Since I am sharing the test data with the plain calculation tests I have to manually adjust this value:
            expectedE *= -1;

            // Act
            if (advance)
            {
                FinCalcViewModelHelper.ExecuteDummyOperation(vm, Ca.ToggleAdv);
            }

            FinCalcViewModelHelper.SetFinancialValue(vm, m, CommandWord.RatesPerAnnum);
            FinCalcViewModelHelper.SetFinancialValue(vm, n, CommandWord.Years);
            FinCalcViewModelHelper.SetFinancialValue(vm, p, CommandWord.NominalInterestRate);
            FinCalcViewModelHelper.SetFinancialValue(vm, k0, CommandWord.Start);
            FinCalcViewModelHelper.SetFinancialValue(vm, kn, CommandWord.End);
            vm.RatePressedCommand.Execute(false);

            // Assert
            vm.DisplayNumber.Should().BeApproximately(expectedE, Tolerance);
        }

        [Theory]
        [MemberData(nameof(FinancialCalculation.NWanted), MemberType = typeof(FinancialCalculation))]
        public void CalculateAndDisplayN_GivenAllNeededVariables(double m, double p, double k0, double e, double kn, double expectedN)
        {
            // Arrange
            var mockObjects = MockFactories.GetMockObjects();
            var vm = MockFactories.FinCalcViewModel2WithCalculatorImplementationFactory(mockObjects);

            // Act
            FinCalcViewModelHelper.SetFinancialValue(vm, m, CommandWord.RatesPerAnnum);
            FinCalcViewModelHelper.SetFinancialValue(vm, p, CommandWord.NominalInterestRate);
            FinCalcViewModelHelper.SetFinancialValue(vm, k0, CommandWord.Start);
            FinCalcViewModelHelper.SetFinancialValue(vm, e, CommandWord.Rate);
            FinCalcViewModelHelper.SetFinancialValue(vm, kn, CommandWord.End);
            vm.YearsPressedCommand.Execute(false);

            // Assert
            vm.DisplayNumber.Should().BeApproximately(expectedN, Tolerance);
        }

        [Theory]
        [MemberData(nameof(FinancialCalculation.PWanted), MemberType = typeof(FinancialCalculation))]
        public void CalculateAndDisplayP_GivenAllNeededVariables(double m, double n, double k0, double e, double kn, double expectedP)
        {
            // Arrange
            var mockObjects = MockFactories.GetMockObjects();
            var vm = MockFactories.FinCalcViewModel2WithCalculatorImplementationFactory(mockObjects);

            var localTolerance = 0.001;

            // Act
            FinCalcViewModelHelper.SetFinancialValue(vm, m, CommandWord.RatesPerAnnum);
            FinCalcViewModelHelper.SetFinancialValue(vm, n, CommandWord.Years);
            FinCalcViewModelHelper.SetFinancialValue(vm, k0, CommandWord.Start);
            FinCalcViewModelHelper.SetFinancialValue(vm, e, CommandWord.Rate);
            FinCalcViewModelHelper.SetFinancialValue(vm, kn, CommandWord.End);
            vm.InterestPressedCommand.Execute(false);

            // Assert
            vm.DisplayNumber.Should().BeApproximately(vm.InterestNumber, localTolerance);
        }
    }
}
