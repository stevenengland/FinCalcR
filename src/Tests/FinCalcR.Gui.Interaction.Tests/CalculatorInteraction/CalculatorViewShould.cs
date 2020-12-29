using System;
using System.IO;
using System.Threading.Tasks;
using FinCalcR.Gui.Interaction.Tests.Framework;
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
        private readonly TimeSpan touchDelayWithOffset = TimeSpan.FromMilliseconds(2200);

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
        public void ShowCorrectNumber_WhenDigitsArePressed()
        {
            // Arrange
            var mainScreen = this.Application.GetMainWindow(this.Automation);

            // Act
            var resultLbl = this.WaitForElement(() => mainScreen.FindFirstDescendant(cf => cf.ByAutomationId(UiIds.ClassicCalculator.EvaluationResultLbl)).AsLabel());
            this.WaitForElement(() => mainScreen.FindFirstDescendant(cf => cf.ByAutomationId(UiIds.ClassicCalculator.Digit1Btn)).AsButton()).Click();
            this.WaitForElement(() => mainScreen.FindFirstDescendant(cf => cf.ByAutomationId(UiIds.ClassicCalculator.Digit2Btn)).AsButton()).Click();
            this.WaitForElement(() => mainScreen.FindFirstDescendant(cf => cf.ByAutomationId(UiIds.ClassicCalculator.Digit3Btn)).AsButton()).Click();
            this.WaitForElement(() => mainScreen.FindFirstDescendant(cf => cf.ByAutomationId(UiIds.ClassicCalculator.Digit4Btn)).AsButton()).Click();
            this.WaitForElement(() => mainScreen.FindFirstDescendant(cf => cf.ByAutomationId(UiIds.ClassicCalculator.Digit5Btn)).AsButton()).Click();
            this.WaitForElement(() => mainScreen.FindFirstDescendant(cf => cf.ByAutomationId(UiIds.ClassicCalculator.Digit6Btn)).AsButton()).Click();
            this.WaitForElement(() => mainScreen.FindFirstDescendant(cf => cf.ByAutomationId(UiIds.ClassicCalculator.Digit7Btn)).AsButton()).Click();
            this.WaitForElement(() => mainScreen.FindFirstDescendant(cf => cf.ByAutomationId(UiIds.ClassicCalculator.Digit8Btn)).AsButton()).Click();
            this.WaitForElement(() => mainScreen.FindFirstDescendant(cf => cf.ByAutomationId(UiIds.ClassicCalculator.Digit9Btn)).AsButton()).Click();
            this.WaitForElement(() => mainScreen.FindFirstDescendant(cf => cf.ByAutomationId(UiIds.ClassicCalculator.Digit0Btn)).AsButton()).Click();
            Wait.UntilInputIsProcessed();

            // Assert
            resultLbl.Text.Should().Be("1,234,567,890.");
        }

        [Fact]
        public void ProcessKeyboardEvents_WhenKeyboardKeyIsPressed()
        {
            // Arrange
            var mainScreen = this.Application.GetMainWindow(this.Automation);

            // Act
            var resultLbl = this.WaitForElement(() => mainScreen.FindFirstDescendant(cf => cf.ByAutomationId(UiIds.ClassicCalculator.EvaluationResultLbl)).AsLabel());
            Keyboard.Type("1234567890");
            Wait.UntilInputIsProcessed();

            // Assert
            resultLbl.Text.Should().Be("1,234,567,890.");
        }

        [Fact]
        public void ShowNegativeNumber_WhenAlgebSignIsPressed()
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

        [Fact]
        public async Task ShowSavedYearsValue_WhenYearsButtonIsClickedLongAsync()
        {
            // Arrange
            var mainScreen = this.Application.GetMainWindow(this.Automation);

            // Act
            var resultLbl = this.WaitForElement(() => mainScreen.FindFirstDescendant(cf => cf.ByAutomationId(UiIds.ClassicCalculator.EvaluationResultLbl)).AsLabel());
            var yearsBtn = this.WaitForElement(() => mainScreen.FindFirstDescendant(cf => cf.ByAutomationId(UiIds.ClassicCalculator.YearsBtn)).AsButton());
            Mouse.MoveTo(yearsBtn.GetClickablePoint());
            await ExtendedMouseInput.LongLeftMouseClickAsync(this.touchDelayWithOffset);
            Wait.UntilInputIsProcessed();

            // Assert
            resultLbl.Text.Should().Be("0.00");
        }

        [Fact(Skip = "https://github.com/FlaUI/FlaUI/issues/389")]
        public void ShowSavedYearsValue_WhenYearsButtonIsTouchedLong()
        {
            // Arrange
            var mainScreen = this.Application.GetMainWindow(this.Automation);

            // Act
            var resultLbl = this.WaitForElement(() => mainScreen.FindFirstDescendant(cf => cf.ByAutomationId(UiIds.ClassicCalculator.EvaluationResultLbl)).AsLabel());
            var yearsBtn = this.WaitForElement(() => mainScreen.FindFirstDescendant(cf => cf.ByAutomationId(UiIds.ClassicCalculator.YearsBtn)).AsButton());
            Mouse.MoveTo(yearsBtn.GetClickablePoint());
            Touch.Hold(this.touchDelayWithOffset, yearsBtn.GetClickablePoint());
            Wait.UntilInputIsProcessed(touchDelayWithOffset);

            // Assert
            resultLbl.Text.Should().Be("0.00");
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
