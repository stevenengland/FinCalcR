using System;
using System.IO;
using System.Threading.Tasks;
using FinCalcR.Gui.Interaction.Tests.Framework;
using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Input;
using FlaUI.Core.WindowsAPI;
using FlaUI.UIA3;
using FluentAssertions;
using Xunit;

namespace FinCalcR.Gui.Interaction.Tests.CalculatorInteraction
{
    /// <summary>
    /// A test class to check the bindings. The rest is covered via ViewModel testing.
    /// </summary>
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
        public void ShowCorrectNumber_WhenDigitButtonsArePressed()
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
        public void ActivateDecimalSeparator_WhenDecimalButtonIsPressed()
        {
            // Arrange
            var mainScreen = this.Application.GetMainWindow(this.Automation);

            // Act
            var resultLbl = this.WaitForElement(() => mainScreen.FindFirstDescendant(cf => cf.ByAutomationId(UiIds.ClassicCalculator.EvaluationResultLbl)).AsLabel());
            this.WaitForElement(() => mainScreen.FindFirstDescendant(cf => cf.ByAutomationId(UiIds.ClassicCalculator.Digit1Btn)).AsButton()).Click();
            this.WaitForElement(() => mainScreen.FindFirstDescendant(cf => cf.ByAutomationId(UiIds.ClassicCalculator.DecimalBtn)).AsButton()).Click();
            this.WaitForElement(() => mainScreen.FindFirstDescendant(cf => cf.ByAutomationId(UiIds.ClassicCalculator.Digit3Btn)).AsButton()).Click();
            Wait.UntilInputIsProcessed();

            // Assert
            resultLbl.Text.Should().Be("1.3");
        }

        [Fact]
        public void ProcessOperators_WhenOperatorButtonsArePressed()
        {
            // Arrange
            var mainScreen = this.Application.GetMainWindow(this.Automation);

            // Act
            var resultLbl = this.WaitForElement(() => mainScreen.FindFirstDescendant(cf => cf.ByAutomationId(UiIds.ClassicCalculator.EvaluationResultLbl)).AsLabel());
            Keyboard.Type("1");
            this.WaitForElement(() => mainScreen.FindFirstDescendant(cf => cf.ByAutomationId(UiIds.ClassicCalculator.OperatorAddBtn)).AsButton()).Click();
            Keyboard.Type("9");
            Keyboard.Type(VirtualKeyShort.ENTER);
            Wait.UntilInputIsProcessed();
            var addResult = resultLbl.Text;
            this.WaitForElement(() => mainScreen.FindFirstDescendant(cf => cf.ByAutomationId(UiIds.ClassicCalculator.OperatorSubtractBtn)).AsButton()).Click();
            Keyboard.Type("4");
            Keyboard.Type(VirtualKeyShort.ENTER);
            Wait.UntilInputIsProcessed();
            var subtractResult = resultLbl.Text;
            this.WaitForElement(() => mainScreen.FindFirstDescendant(cf => cf.ByAutomationId(UiIds.ClassicCalculator.OperatorMultiplyBtn)).AsButton()).Click();
            Keyboard.Type("10");
            Keyboard.Type(VirtualKeyShort.ENTER);
            Wait.UntilInputIsProcessed();
            var multiplicationResult = resultLbl.Text;
            this.WaitForElement(() => mainScreen.FindFirstDescendant(cf => cf.ByAutomationId(UiIds.ClassicCalculator.OperatorDivideBtn)).AsButton()).Click();
            Keyboard.Type("6");
            Keyboard.Type(VirtualKeyShort.ENTER);
            Wait.UntilInputIsProcessed();
            var divisionResult = resultLbl.Text;

            // Assert
            addResult.Should().Be("10.");
            subtractResult.Should().Be("6.");
            multiplicationResult.Should().Be("60.");
            divisionResult.Should().Be("10.");
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
        public void ShowNegativeNumber_WhenAlgebSignButtonIsPressed()
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
        public void ClearValues_WhenClearButtonIsPressedAShortTime()
        {
            // Arrange
            var mainScreen = this.Application.GetMainWindow(this.Automation);

            // Act
            var resultLbl = this.WaitForElement(() => mainScreen.FindFirstDescendant(cf => cf.ByAutomationId(UiIds.ClassicCalculator.EvaluationResultLbl)).AsLabel());
            Keyboard.Type("1234567890");
            Wait.UntilInputIsProcessed();
            this.WaitForElement(() => mainScreen.FindFirstDescendant(cf => cf.ByAutomationId(UiIds.ClassicCalculator.ClearBtn)).AsButton()).Click();
            Wait.UntilInputIsProcessed();

            // Assert
            resultLbl.Text.Should().Be("0.");
        }

        [Fact]
        public async Task ClearValuesAndShowHint_WhenClearButtonIsClickedALongTimeAsync()
        {
            // Arrange
            var mainScreen = this.Application.GetMainWindow(this.Automation);

            // Act
            var resultLbl = this.WaitForElement(() => mainScreen.FindFirstDescendant(cf => cf.ByAutomationId(UiIds.ClassicCalculator.EvaluationResultLbl)).AsLabel());
            Keyboard.Type("1234567890");
            Wait.UntilInputIsProcessed();
            var clearBtn = this.WaitForElement(() => mainScreen.FindFirstDescendant(cf => cf.ByAutomationId(UiIds.ClassicCalculator.ClearBtn)).AsButton());
            Mouse.MoveTo(clearBtn.GetClickablePoint());
            await ExtendedMouseInput.LongLeftMouseClickAsync(this.touchDelayWithOffset);
            Wait.UntilInputIsProcessed();
            var hintMessageTxt = this.WaitForElement(() => mainScreen.FindFirstDescendant(cf => cf.ByAutomationId(UiIds.HintView.HintMessageTxt)).AsLabel());

            // Assert
            hintMessageTxt.Text.Should().NotBeNullOrWhiteSpace();
            resultLbl.Text.Should().Be("0.");
        }

        [Fact]
        public void SpecialFunctionsAreProcessed_WhenSpecialFunctionButtonsArePressedAShortTime()
        {
            // Arrange
            var mainScreen = this.Application.GetMainWindow(this.Automation);

            // Act
            var resultLbl = this.WaitForElement(() => mainScreen.FindFirstDescendant(cf => cf.ByAutomationId(UiIds.ClassicCalculator.EvaluationResultLbl)).AsLabel());
            Keyboard.Type("1");
            this.WaitForElement(() => mainScreen.FindFirstDescendant(cf => cf.ByAutomationId(UiIds.ClassicCalculator.YearsBtn)).AsButton()).Click();
            Wait.UntilInputIsProcessed();
            var yearsResult = resultLbl.Text;
            Keyboard.Type("2");
            this.WaitForElement(() => mainScreen.FindFirstDescendant(cf => cf.ByAutomationId(UiIds.ClassicCalculator.InterestBtn)).AsButton()).Click();
            Wait.UntilInputIsProcessed();
            var interestResult = resultLbl.Text;
            Keyboard.Type("3");
            this.WaitForElement(() => mainScreen.FindFirstDescendant(cf => cf.ByAutomationId(UiIds.ClassicCalculator.StartBtn)).AsButton()).Click();
            Wait.UntilInputIsProcessed();
            var startResult = resultLbl.Text;
            Keyboard.Type("4");
            this.WaitForElement(() => mainScreen.FindFirstDescendant(cf => cf.ByAutomationId(UiIds.ClassicCalculator.RateBtn)).AsButton()).Click();
            Wait.UntilInputIsProcessed();
            var rateResult = resultLbl.Text;
            Keyboard.Type("5");
            this.WaitForElement(() => mainScreen.FindFirstDescendant(cf => cf.ByAutomationId(UiIds.ClassicCalculator.EndBtn)).AsButton()).Click();
            Wait.UntilInputIsProcessed();
            var endResult = resultLbl.Text;

            // Assert
            yearsResult.Should().Be("1.00");
            interestResult.Should().Be("2.000");
            startResult.Should().Be("3.00");
            rateResult.Should().Be("4.00");
            endResult.Should().Be("5.00");
        }

        [Fact]
        public async Task ShowSavedSpecialFunctionValues_WhenSpecialFunctionButtonIsClickedALongTimeAsync()
        {
            // Arrange
            var mainScreen = this.Application.GetMainWindow(this.Automation);

            // Act
            var resultLbl = this.WaitForElement(() => mainScreen.FindFirstDescendant(cf => cf.ByAutomationId(UiIds.ClassicCalculator.EvaluationResultLbl)).AsLabel());

            Keyboard.Type("1");
            var yearsBtn = this.WaitForElement(() => mainScreen.FindFirstDescendant(cf => cf.ByAutomationId(UiIds.ClassicCalculator.YearsBtn)).AsButton());
            yearsBtn.Click();
            Wait.UntilInputIsProcessed();
            Keyboard.Type("2");
            var interestBtn = this.WaitForElement(() => mainScreen.FindFirstDescendant(cf => cf.ByAutomationId(UiIds.ClassicCalculator.InterestBtn)).AsButton());
            interestBtn.Click();
            Wait.UntilInputIsProcessed();
            Keyboard.Type("3");
            var startBtn = this.WaitForElement(() => mainScreen.FindFirstDescendant(cf => cf.ByAutomationId(UiIds.ClassicCalculator.StartBtn)).AsButton());
            startBtn.Click();
            Wait.UntilInputIsProcessed();
            Keyboard.Type("4");
            var rateBtn = this.WaitForElement(() => mainScreen.FindFirstDescendant(cf => cf.ByAutomationId(UiIds.ClassicCalculator.RateBtn)).AsButton());
            rateBtn.Click();
            Wait.UntilInputIsProcessed();
            Keyboard.Type("5");
            var endBtn = this.WaitForElement(() => mainScreen.FindFirstDescendant(cf => cf.ByAutomationId(UiIds.ClassicCalculator.EndBtn)).AsButton());
            endBtn.Click();
            Wait.UntilInputIsProcessed();

            Mouse.MoveTo(yearsBtn.GetClickablePoint());
            await ExtendedMouseInput.LongLeftMouseClickAsync(this.touchDelayWithOffset);
            Wait.UntilInputIsProcessed();
            var yearsResult = resultLbl.Text;
            Mouse.MoveTo(interestBtn.GetClickablePoint());
            await ExtendedMouseInput.LongLeftMouseClickAsync(this.touchDelayWithOffset);
            Wait.UntilInputIsProcessed();
            var interestResult = resultLbl.Text;
            Mouse.MoveTo(startBtn.GetClickablePoint());
            await ExtendedMouseInput.LongLeftMouseClickAsync(this.touchDelayWithOffset);
            Wait.UntilInputIsProcessed();
            var startResult = resultLbl.Text;
            Mouse.MoveTo(rateBtn.GetClickablePoint());
            await ExtendedMouseInput.LongLeftMouseClickAsync(this.touchDelayWithOffset);
            Wait.UntilInputIsProcessed();
            var rateResult = resultLbl.Text;
            Mouse.MoveTo(endBtn.GetClickablePoint());
            await ExtendedMouseInput.LongLeftMouseClickAsync(this.touchDelayWithOffset);
            Wait.UntilInputIsProcessed();
            var endResult = resultLbl.Text;

            // Assert
            yearsResult.Should().Be("1.00");
            interestResult.Should().Be("2.000");
            startResult.Should().Be("3.00");
            rateResult.Should().Be("4.00");
            endResult.Should().Be("5.00");
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
