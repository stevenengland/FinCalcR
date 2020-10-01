using System;
using System.Diagnostics.Eventing.Reader;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Caliburn.Micro;
using StEn.FinCalcR.Calculations;
using StEn.FinCalcR.Common.LanguageResources;
using StEn.FinCalcR.Common.Services.Localization;
using StEn.FinCalcR.WinUi.Commanding;
using StEn.FinCalcR.WinUi.Events;
using WPFLocalizeExtension.Deprecated.Extensions;

namespace StEn.FinCalcR.WinUi.ViewModels
{
	public class FinCalcViewModel : Screen
	{
		private readonly ILocalizationService localizationService;
		private readonly IEventAggregator eventAggregator;
		private string displayText;
		private double displayNumber;
		private bool isDecimalSeparatorActive = false;
		private string activeMathOperator = string.Empty;
		private string leftSide;
		private string rightSide;
		private double firstNumber = 0;
		private double secondNumber = 0;
		private bool resetNeeded = false;

		public FinCalcViewModel(
			ILocalizationService localizationService,
			IEventAggregator eventAggregator)
		{
			this.localizationService = localizationService;
			this.eventAggregator = eventAggregator;

			this.OnClearPressed();
		}

		public string DisplayText
		{
			get => this.displayText;
			set
			{
				this.displayText = value;
				this.NotifyOfPropertyChange(() => this.DisplayText);
			}
		}

		public double DisplayNumber
		{
			get => this.displayNumber;
			set
			{
				this.displayNumber = value;
				this.NotifyOfPropertyChange(() => this.DisplayNumber);
			}
		}

		public string ActiveMathOperator
		{
			get => this.activeMathOperator;
			set
			{
				this.activeMathOperator = value;
				this.NotifyOfPropertyChange(() => this.ActiveMathOperator);
			}
		}

		public string ThousandsSeparator => Resources.CALC_THOUSANDS_SEPARATOR;

		public string DecimalSeparator => Resources.CALC_DECIMAL_SEPARATOR;

		public ICommand DigitPressedCommand => new SyncCommand<object>(this.OnDigitPressed);

		public ICommand OperatorPressedCommand => new SyncCommand<object>(this.OnOperatorPressed);

		public ICommand AlgebSignCommand => new SyncCommand(this.OnAlgebSignPressed);

		public ICommand DecimalSeparatorPressedCommand => new SyncCommand(this.OnDecimalSeparatorPressed);

		public ICommand ClearPressedCommand => new SyncCommand(this.OnClearPressed);

		public ICommand CalculatePressedCommand => new SyncCommand(this.OnCalculatePressed);

		private void OnAlgebSignPressed()
		{
			if (this.leftSide.StartsWith("-"))
			{
				this.leftSide = this.leftSide.Substring(1);
			}
			else
			{
				this.leftSide = "-" + this.leftSide;
			}

			this.SetDisplayText();
		}

		private void OnDigitPressed(object digitObj)
		{
			var digit = digitObj.ToString();
			if (this.isDecimalSeparatorActive)
			{
				if (this.rightSide.Length < 9)
				{
					this.rightSide += digit;
				}
			}
			else
			{
				if (this.leftSide.Length < 10)
				{
					if (!this.leftSide.StartsWith("0") && !this.leftSide.StartsWith("-0"))
					{
						this.leftSide += digit;
					}
					else
					{
						this.leftSide = digit;
					}
				}
			}

			this.SetDisplayText();
		}

		private void OnOperatorPressed(object mathOperatorObj)
		{
			this.ActiveMathOperator = (string)mathOperatorObj;
			this.SetNumber(out this.firstNumber);
		}

		private void OnDecimalSeparatorPressed()
		{
			this.isDecimalSeparatorActive = true;
		}

		private void OnClearPressed()
		{
			this.ResetNumbers();
			this.ResetSides();
			this.ActiveMathOperator = string.Empty;
			this.resetNeeded = false;

			this.SetDisplayText();
		}

		private void OnCalculatePressed()
		{
			if (this.resetNeeded)
			{
				return;
			}

			this.SetNumber(out this.secondNumber);
			var calculatedResult = SimpleCalculator.Calculate(this.firstNumber, this.secondNumber, this.ActiveMathOperator);
			this.ResetNumbers();
			this.firstNumber = calculatedResult;
			this.BuildSidesFromNumber(calculatedResult);
			this.ActiveMathOperator = string.Empty;
			this.SetDisplayText();
			this.resetNeeded = true;

		}

		private void SetNumber(out double number)
		{
			var realRightSide = string.IsNullOrEmpty(this.rightSide) ? "0" : this.rightSide;
			var numberString = this.leftSide + Resources.CALC_DECIMAL_SEPARATOR + realRightSide;
			if (!double.TryParse(numberString, out number))
			{
				this.eventAggregator.PublishOnUIThread(new ErrorEvent(new ArgumentException(Resources.EXC_PARSE_DOUBLE_IMPOSSIBLE), numberString));
			}

			this.ResetSides();
		}

		private void SetDisplayNumber()
		{
			var realRightSide = string.IsNullOrEmpty(this.rightSide) ? "0" : this.rightSide;
			var concatenatedSides = this.leftSide + Resources.CALC_DECIMAL_SEPARATOR + realRightSide;
			if (!double.TryParse(concatenatedSides, out var parsedNumber))
			{
				this.eventAggregator.PublishOnUIThread(new ErrorEvent(new ArgumentException(Resources.EXC_PARSE_DOUBLE_IMPOSSIBLE), concatenatedSides));
			}
			else
			{
				this.DisplayNumber = parsedNumber;
			}
		}

		private void SetDisplayText()
		{
			this.DisplayText = this.leftSide + Resources.CALC_DECIMAL_SEPARATOR + this.rightSide;
			this.SetDisplayNumber();
		}

		private void BuildSidesFromNumber(double calculatedResult)
		{
			var roundedResult = Math.Round(calculatedResult, 9);
			var s = roundedResult.ToString(CultureInfo.InvariantCulture);
			var parts = s.Split('.');
			this.leftSide = parts[0];
			this.rightSide = parts.Length < 2 ? string.Empty : parts[1];
		}

		private void ResetSides()
		{
			this.rightSide = string.Empty;
			this.leftSide = "0";
			this.isDecimalSeparatorActive = false;
		}

		private void ResetNumbers()
		{
			this.firstNumber = 0;
			this.secondNumber = 0;
		}
	}
}
