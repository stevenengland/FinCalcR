using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FinCalcR.WinUi.Tests.Mocks;
using FluentAssertions;
using StEn.FinCalcR.WinUi.ViewModels;
using Xunit;

namespace FinCalcR.WinUi.Tests.ViewModelTests.FinCalcViewModelTestCollections
{
    // ToDo: Pull all the related tests for this command to this place and dito for other commands.
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global", Justification = "MemberData needs public property")]
    public class DecimalCommandShould
    {
        public DecimalCommandShould()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("de");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("de");
        }

        public static IEnumerable<object[]> SpecialDecimalDigit =>
            new List<object[]>
            {
                new object[] { new Ca[] { Ca.Nr1, Ca.SetYears }, Ca.Dec, new Ca[] { Ca.Nr2 }, "0,2" },
                new object[] { new Ca[] { Ca.Nr1, Ca.GetYears }, Ca.Dec, new Ca[] { Ca.Nr2 }, "0,2" },
                new object[] { new Ca[] { Ca.Nr1, Ca.SetInt }, Ca.Dec, new Ca[] { Ca.Nr2 }, "0,2" },
                new object[] { new Ca[] { Ca.Nr1, Ca.GetInt }, Ca.Dec, new Ca[] { Ca.Nr2 }, "0,2" },
                new object[] { new Ca[] { Ca.Nr1, Ca.SetStart }, Ca.Dec, new Ca[] { Ca.Nr2 }, "0,2" },
                new object[] { new Ca[] { Ca.Nr1, Ca.GetStart }, Ca.Dec, new Ca[] { Ca.Nr2 }, "0,2" },
                new object[] { new Ca[] { Ca.Nr1, Ca.SetRate }, Ca.Dec, new Ca[] { Ca.Nr2 }, "0,2" },
                new object[] { new Ca[] { Ca.Nr1, Ca.GetRate }, Ca.Dec, new Ca[] { Ca.Nr2 }, "0,2" },
                new object[] { new Ca[] { Ca.Nr1, Ca.SetEnd }, Ca.Dec, new Ca[] { Ca.Nr2 }, "0,2" },
                new object[] { new Ca[] { Ca.Nr1, Ca.GetEnd}, Ca.Dec, new Ca[] { Ca.Nr2 }, "0,2" },
                new object[] { new Ca[] { Ca.Nr1, Ca.SetRpa }, Ca.Dec, new Ca[] { Ca.Nr2 }, "0,2" },
                new object[] { new Ca[] { Ca.Nr1, Ca.GetRpa }, Ca.Dec, new Ca[] { Ca.Nr2 }, "0,2" },
                new object[] { new Ca[] { Ca.Nr1, Ca.SetNom }, Ca.Dec, new Ca[] { Ca.Nr2 }, "0,2" },
                new object[] { new Ca[] { Ca.Nr1, Ca.GetNom }, Ca.Dec, new Ca[] { Ca.Nr2 }, "0,2" },
                new object[] { new Ca[] { Ca.Nr1, Ca.ToggleAdv }, Ca.Dec, new Ca[] { Ca.Nr2 }, "0,2" },
                new object[] { new Ca[] { Ca.Nr1, Ca.SetRep }, Ca.Dec, new Ca[] { Ca.Nr2 }, "0,2" },
                new object[] { new Ca[] { Ca.Nr1, Ca.GetRep }, Ca.Dec, new Ca[] { Ca.Nr2 }, "0,2" },
                new object[] { new Ca[] { Ca.PerA }, Ca.Dec, new Ca[] { Ca.Nr2 }, "0,2" },
            };

        public static IEnumerable<object[]> SpecialDecimal =>
            new List<object[]>
            {
                new object[] { new Ca[] { Ca.Nr1, Ca.SetYears }, Ca.Dec, null, "1,00" },
                new object[] { new Ca[] { Ca.Nr1, Ca.SetYears, Ca.ClearP, Ca.GetYears }, Ca.Dec, null, "1,00" },
                new object[] { new Ca[] { Ca.Nr1, Ca.SetInt }, Ca.Dec, null, "1,000" },
                new object[] { new Ca[] { Ca.Nr1, Ca.SetInt, Ca.ClearP, Ca.GetInt }, Ca.Dec, null, "1,000" },
                new object[] { new Ca[] { Ca.Nr1, Ca.SetStart }, Ca.Dec, null, "1,00" },
                new object[] { new Ca[] { Ca.Nr1, Ca.SetStart, Ca.ClearP, Ca.GetStart }, Ca.Dec, null, "1,00" },
                new object[] { new Ca[] { Ca.Nr1, Ca.SetRate }, Ca.Dec, null, "1,00" },
                new object[] { new Ca[] { Ca.Nr1, Ca.SetRate, Ca.ClearP, Ca.GetRate }, Ca.Dec, null, "1,00" },
                new object[] { new Ca[] { Ca.Nr1, Ca.SetEnd }, Ca.Dec, null, "1,00" },
                new object[] { new Ca[] { Ca.Nr1, Ca.SetEnd, Ca.ClearP, Ca.GetEnd }, Ca.Dec, null, "1,00" },
                new object[] { new Ca[] { Ca.Nr1, Ca.SetRpa }, Ca.Dec, null, "1," },
                new object[] { new Ca[] { Ca.Nr1, Ca.SetRpa, Ca.ClearP, Ca.GetRpa }, Ca.Dec, null, "1," },
                new object[] { new Ca[] { Ca.Nr1, Ca.SetNom }, Ca.Dec, null, "1,005" },
                new object[] { new Ca[] { Ca.Nr1, Ca.SetNom, Ca.ClearP, Ca.GetNom }, Ca.Dec, null, "1,000" },
                new object[] { new Ca[] { Ca.Nr1, Ca.ToggleAdv }, Ca.Dec, null, "1," }, // ToDo: special to the physical calculator: Should be "0,"
                new object[] { new Ca[] { Ca.Nr1, Ca.SetRep }, Ca.Dec, null, "0,00" },
                new object[] { new Ca[] { Ca.Nr1, Ca.SetRep, Ca.ClearP, Ca.GetRep }, Ca.Dec, null, "0,00" },
                new object[] { new Ca[] { Ca.PerA }, Ca.Dec, null, "210,00" },
            };

        [Theory]
        [MemberData(nameof(SpecialDecimalDigit))]
        [MemberData(nameof(SpecialDecimal))]
        public void ResetInputState_WhenPressedAfterSpecialFunction(Ca[] preOperations, Ca mainOperation, Ca[] postOperations, string expectedOutputTextAfterAllOperations)
        {
            // Arrange
            var mockObjects = MockFactories.GetMockObjects();
            var vm = MockFactories.FinCalcViewModel2WithCalculatorImplementationFactory(mockObjects);
            if (preOperations == null)
            {
                preOperations = new Ca[] { };
            }

            if (postOperations == null)
            {
                postOperations = new Ca[] { };
            }

            // Act
            FinCalcViewModelHelper.ExecuteOperations(vm, preOperations.Concat(new Ca[] { mainOperation }).Concat(postOperations).ToArray());

            // Assert
            vm.DisplayText.Should().Be(expectedOutputTextAfterAllOperations);
        }
    }
}
