using System.IO;
using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Input;
using FlaUI.UIA3;
using FluentAssertions;
using Xunit;

namespace FinCalcR.Gui.Interaction.Tests.CalculatorInteraction
{
    public class CalculatorViewShould : TestBase
    {
        public CalculatorViewShould(GuiAutomationFixture fixture)
			: base(fixture)
        {
        }

        protected override string AppPath { get; } = Path.Combine(
        "../../../../..",
        "App",
        "FinCalcR.WinUi",
        "bin",
        $"{Configuration}",
        "FinCalcR.exe");

        [Fact]
        public void ProcessDigitClicks()
        {
            // Arrange
            var mainScreen = this.Application.GetMainWindow(this.Automation);

            // Act
            var resultLbl = this.WaitForElement(() => mainScreen.FindFirstDescendant(cf => cf.ByAutomationId(UiIds.ClassicCalculator.EvaluationResultLbl)).AsLabel());
            var digitBtn = this.WaitForElement(() => mainScreen.FindFirstDescendant(cf => cf.ByAutomationId(UiIds.ClassicCalculator.Digit1Btn)).AsButton());
            digitBtn?.Invoke();

            // Assert
            resultLbl.Text.Should().Be("1.");
        }

        [Fact]
        public void ProcessAlgebSignClick()
        {
            // Arrange
            var mainScreen = this.Application.GetMainWindow(this.Automation);

            // Act
            var resultLbl = this.WaitForElement(() => mainScreen.FindFirstDescendant(cf => cf.ByAutomationId(UiIds.ClassicCalculator.EvaluationResultLbl)).AsLabel());
            var algebSignBtn = this.WaitForElement(() => mainScreen.FindFirstDescendant(cf => cf.ByAutomationId(UiIds.ClassicCalculator.AlgebSignBtn)).AsButton());
            algebSignBtn?.Invoke();
            Wait.UntilInputIsProcessed();

            // Assert
            resultLbl.Text.Should().Be("-0.");
        }

        #region protected Overrides

        protected override AutomationBase GetAutomation()
        {
            var automation = new UIA3Automation();
            return automation;
        }

        protected override Application StartApplication()
        {
            var application = Application.Launch(this.AppPath);
            application.WaitWhileMainHandleIsMissing();

            // Give the application some additional time to start
            System.Threading.Thread.Sleep(200);
            return application;
        }

        #endregion
    }
}
