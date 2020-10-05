﻿using System;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Caliburn.Micro;
using StEn.FinCalcR.Calculations;
using StEn.FinCalcR.Common.LanguageResources;
using StEn.FinCalcR.Common.Services.Localization;
using StEn.FinCalcR.WinUi.Commanding;
using StEn.FinCalcR.WinUi.Events;
using StEn.FinCalcR.WinUi.Types;

namespace StEn.FinCalcR.WinUi.ViewModels
{
	public class FinCalcViewModel : Screen
	{
		private const int LongTouchDelay = 2;
#pragma warning disable S1450 // Private fields only used as local variables in methods should become local variables
		private readonly ILocalizationService localizationService;
#pragma warning restore S1450 // Private fields only used as local variables in methods should become local variables
		private readonly IEventAggregator eventAggregator;
		private string displayText;
		private double displayNumber;
		private bool isDecimalSeparatorActive = false;
		private string activeMathOperator = string.Empty;
		private string leftSide;
		private string rightSide;
		private double firstNumber = 0;
		private double secondNumber = 0;
		private double interestNumber = 0;
		private int ratesPerAnnumNumber = 12;
		private bool calcCommandLock = false;
		private string advanceStatusBarText;
		private string yearsStatusBarText;
		private string interestStatusBarText;
		private string startStatusBarText;
		private string rateStatusBarText;
		private string endStatusBarText;

		public FinCalcViewModel(
			ILocalizationService localizationService,
			IEventAggregator eventAggregator)
		{
			this.localizationService = localizationService;
			this.eventAggregator = eventAggregator;

			this.OnClearPressed();
		}

		public LastPressedOperation LastPressedOperation { get; set; } = LastPressedOperation.None;

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

		public string AdvanceStatusBarText
		{
			get => this.advanceStatusBarText;
			set
			{
				this.advanceStatusBarText = value;
				this.NotifyOfPropertyChange(() => this.AdvanceStatusBarText);
			}
		}

		public string YearsStatusBarText
		{
			get => this.yearsStatusBarText;
			set
			{
				this.yearsStatusBarText = value;
				this.NotifyOfPropertyChange(() => this.YearsStatusBarText);
			}
		}

		public string InterestStatusBarText
		{
			get => this.interestStatusBarText;
			set
			{
				this.interestStatusBarText = value;
				this.NotifyOfPropertyChange(() => this.InterestStatusBarText);
			}
		}

		public string StartStatusBarText
		{
			get => this.startStatusBarText;
			set
			{
				this.startStatusBarText = value;
				this.NotifyOfPropertyChange(() => this.StartStatusBarText);
			}
		}

		public string RateStatusBarText
		{
			get => this.rateStatusBarText;
			set
			{
				this.rateStatusBarText = value;
				this.NotifyOfPropertyChange(() => this.RateStatusBarText);
			}
		}

		public string EndStatusBarText
		{
			get => this.endStatusBarText;
			set
			{
				this.endStatusBarText = value;
				this.NotifyOfPropertyChange(() => this.EndStatusBarText);
			}
		}

		public string ThousandsSeparator => Resources.CALC_THOUSANDS_SEPARATOR;

		public string DecimalSeparator => Resources.CALC_DECIMAL_SEPARATOR;

		public ICommand DigitPressedCommand => new SyncCommand<object>(this.OnDigitPressed);

		public ICommand OperatorPressedCommand => new SyncCommand<object>(this.OnOperatorPressed);

		public ICommand AlgebSignCommand => new SyncCommand(this.OnAlgebSignPressed);

		public ICommand DecimalSeparatorPressedCommand => new SyncCommand(this.OnDecimalSeparatorPressed);

		public ICommand ClearPressedCommand => new SyncCommand<bool>(this.OnClearPressed);

		public ICommand CalculatePressedCommand => new SyncCommand(this.OnCalculatePressed);

		public ICommand YearsPressedCommand => new SyncCommand<bool>(this.OnYearsPressed);

		public IAsyncCommand<IGestureHandler> InterestPressedCommand => new AsyncCommand<IGestureHandler>(this.OnInterestPressedAsync);

		public ICommand StartPressedCommand => new SyncCommand<bool>(this.OnStartPressed);

		public ICommand RatePressedCommand => new SyncCommand<bool>(this.OnRatePressed);

		public ICommand EndPressedCommand => new SyncCommand<bool>(this.OnEndPressed);

		public async Task OnClearPressedAsync(object sender, MouseButtonEventArgs e) // Public wrapper so that Caliburn can access it.
		{
			var element = (FrameworkElement)sender;
			var gestureHandler = new FrameworkElementGestureHandler(element);
			var isLongTouch = await gestureHandler.IsLongTouchAsync(TimeSpan.FromSeconds(LongTouchDelay));
			this.ClearPressedCommand.Execute(isLongTouch);
		}

		public async Task OnYearsPressedAsync(object sender, MouseButtonEventArgs e) // Public wrapper so that Caliburn can access it.
		{
			var element = (FrameworkElement)sender;
			var gestureHandler = new FrameworkElementGestureHandler(element);
			var isLongTouch = await gestureHandler.IsLongTouchAsync(TimeSpan.FromSeconds(LongTouchDelay));
			this.YearsPressedCommand.Execute(isLongTouch);
		}

		public async Task OnInterestPressedAsync(object sender, MouseButtonEventArgs e) // Public wrapper so that Caliburn can access it.
		{
			var element = (FrameworkElement)sender;
			var gestureHandler = new FrameworkElementGestureHandler(element);
			await this.OnInterestPressedAsync(gestureHandler);
		}

		public async Task OnStartPressedAsync(object sender, MouseButtonEventArgs e) // Public wrapper so that Caliburn can access it.
		{
			var element = (FrameworkElement)sender;
			var gestureHandler = new FrameworkElementGestureHandler(element);
			var isLongTouch = await gestureHandler.IsLongTouchAsync(TimeSpan.FromSeconds(LongTouchDelay));
			this.StartPressedCommand.Execute(isLongTouch);
		}

		public async Task OnRatePressedAsync(object sender, MouseButtonEventArgs e) // Public wrapper so that Caliburn can access it.
		{
			var element = (FrameworkElement)sender;
			var gestureHandler = new FrameworkElementGestureHandler(element);
			var isLongTouch = await gestureHandler.IsLongTouchAsync(TimeSpan.FromSeconds(LongTouchDelay));
			this.RatePressedCommand.Execute(isLongTouch);
		}

		public async Task OnEndPressedAsync(object sender, MouseButtonEventArgs e) // Public wrapper so that Caliburn can access it.
		{
			var element = (FrameworkElement)sender;
			var gestureHandler = new FrameworkElementGestureHandler(element);
			var isLongTouch = await gestureHandler.IsLongTouchAsync(TimeSpan.FromSeconds(LongTouchDelay));
			this.EndPressedCommand.Execute(isLongTouch);
		}

		private void OnClearPressed(bool isLongTouch = false)
		{
			if (isLongTouch)
			{
				this.eventAggregator.PublishOnUIThread(new HintEvent(Resources.HINT_SPECIAL_FUNCTION_MEMORY_RESET));
				this.ResetSpecialFunctionLabels(true);
				this.ResetNumbers(true);
			}
			else
			{
				this.ResetSpecialFunctionLabels();
				this.ResetNumbers();
			}

			this.ResetSides();
			this.ActiveMathOperator = string.Empty;
			this.calcCommandLock = false;

			this.SetDisplayText();

			this.LastPressedOperation = LastPressedOperation.Clear;
		}

		private void OnYearsPressed(bool isLongTouch = false)
		{
			this.YearsStatusBarText = Resources.FinCalcFunctionYears;
			if (isLongTouch)
			{

			}
			else
			{
				
			}

			this.LastPressedOperation = LastPressedOperation.Years;
		}

		private async Task OnInterestPressedAsync(IGestureHandler handler)
		{
			var longTouch = await handler.IsLongTouchAsync(TimeSpan.FromSeconds(LongTouchDelay));
			this.InterestStatusBarText = Resources.FinCalcFunctionInterest;
			if (longTouch)
			{
				// Display the value in the memory
				this.ResetNumbers();
				this.firstNumber = this.interestNumber;
				this.BuildSidesFromNumber(this.interestNumber);
				this.ActiveMathOperator = string.Empty;
				this.SetDisplayText(true, 3);
			}
			else
			{
				// Write the value to the memory

				// If last input was an operator restore the firstNumber for upcoming operations
				if (this.LastPressedOperation == LastPressedOperation.Operator)
				{
					this.BuildSidesFromNumber(this.firstNumber);
				}

				this.SetNumber(out this.interestNumber);
				this.ResetNumbers();
				this.firstNumber = this.interestNumber;
				this.BuildSidesFromNumber(this.interestNumber);
				this.ActiveMathOperator = string.Empty;
				this.SetDisplayText(true, 3);
			}

			this.LastPressedOperation = LastPressedOperation.Interest;
		}

		private void OnStartPressed(bool isLongTouch = false)
		{
			this.StartStatusBarText = Resources.FinCalcFunctionStart;
			if (isLongTouch)
			{

			}
			else
			{

			}

			this.LastPressedOperation = LastPressedOperation.Start;
		}

		private void OnRatePressed(bool isLongTouch = false)
		{
			this.RateStatusBarText = Resources.FinCalcFunctionRate;
			if (isLongTouch)
			{

			}
			else
			{

			}

			this.LastPressedOperation = LastPressedOperation.Rate;
		}

		private void OnEndPressed(bool isLongTouch = false)
		{
			this.EndStatusBarText = Resources.FinCalcFunctionEnd;
			if (isLongTouch)
			{

			}
			else
			{

			}

			this.LastPressedOperation = LastPressedOperation.End;
		}

		private void OnAlgebSignPressed()
		{
			this.ResetSpecialFunctionLabels();

			if (this.leftSide.StartsWith("-"))
			{
				this.leftSide = this.leftSide.Substring(1);
			}
			else
			{
				this.leftSide = "-" + this.leftSide;
			}

			this.SetDisplayText();

			this.LastPressedOperation = LastPressedOperation.AlgebSign;
		}

		private void OnDigitPressed(object digitObj)
		{
			this.ResetSpecialFunctionLabels();

			if (this.calcCommandLock)
			{
				this.ResetSides();
				this.calcCommandLock = false;
			}

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

			this.LastPressedOperation = LastPressedOperation.Digit;
		}

		private void OnOperatorPressed(object mathOperatorObj)
		{
			this.ResetSpecialFunctionLabels();

			if (this.ActiveMathOperator != string.Empty)
			{
				this.OnCalculatePressed();
				this.ActiveMathOperator = (string)mathOperatorObj;
			}
			else
			{
				this.ActiveMathOperator = (string)mathOperatorObj;
				this.SetNumber(out this.firstNumber);
			}

			this.LastPressedOperation = LastPressedOperation.Operator;
		}

		private void OnDecimalSeparatorPressed()
		{
			this.ResetSpecialFunctionLabels();

			if (this.LastPressedOperation == LastPressedOperation.Years
			    || this.LastPressedOperation == LastPressedOperation.Interest
			    || this.LastPressedOperation == LastPressedOperation.Start
			    || this.LastPressedOperation == LastPressedOperation.Rate
			    || this.LastPressedOperation == LastPressedOperation.End)
			{
				this.ResetNumbers();
				this.ResetSides();
				this.ActiveMathOperator = string.Empty;
			}

			this.isDecimalSeparatorActive = true;

			this.LastPressedOperation = LastPressedOperation.Decimal;
		}

		private void OnCalculatePressed()
		{
			this.ResetSpecialFunctionLabels();

			if (this.calcCommandLock)
			{
				return;
			}

			this.SetNumber(out this.secondNumber);
			try
			{
				var calculatedResult = SimpleCalculator.Calculate(this.firstNumber, this.secondNumber, this.ActiveMathOperator);
				if (double.IsNaN(calculatedResult) || double.IsNegativeInfinity(calculatedResult) || double.IsPositiveInfinity(calculatedResult))
				{
					throw new NotFiniteNumberException();
				}

				this.ResetNumbers();
				this.firstNumber = calculatedResult;
				this.BuildSidesFromNumber(calculatedResult);
				this.ActiveMathOperator = string.Empty;
				this.SetDisplayText();
				this.calcCommandLock = true;
				this.LastPressedOperation = LastPressedOperation.Calculate;
			}
			catch (NotFiniteNumberException ex)
			{
				this.eventAggregator.PublishOnUIThread(new ErrorEvent(ex, string.Format(CultureInfo.InvariantCulture, Resources.EXC_NOT_FINITE_NUMBER, this.firstNumber, this.secondNumber)));
				this.OnClearPressed();
			}
			catch (DivideByZeroException ex)
			{
				this.eventAggregator.PublishOnUIThread(new ErrorEvent(ex, string.Format(CultureInfo.InvariantCulture, Resources.EXC_DIVISION_BY_ZERO)));
				this.OnClearPressed();
			}
			catch (OverflowException ex)
			{
				this.eventAggregator.PublishOnUIThread(new ErrorEvent(ex, string.Format(CultureInfo.InvariantCulture, Resources.EXC_OVERFLOW_EXCEPTION, this.firstNumber, this.secondNumber)));
				this.OnClearPressed();
			}
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

		private void SetDisplayText(bool isSpecialFunctionNumber = false, int specialNumberDecimalCount = 2)
		{
			var displayRightSide = this.rightSide;
			if (isSpecialFunctionNumber)
			{
				if (string.IsNullOrWhiteSpace(displayRightSide))
				{
					displayRightSide = "0";
				}

				if (displayRightSide.Length > specialNumberDecimalCount)
				{
					var roundingIdentifier = int.Parse(displayRightSide[specialNumberDecimalCount].ToString());
					if (roundingIdentifier >= 5)
					{
						var charToRound = displayRightSide.Substring(0, specialNumberDecimalCount).TrimStart('0');
						var numberToRound = string.IsNullOrEmpty(charToRound) ? "0" : charToRound;
						displayRightSide = (int.Parse(numberToRound) + 1).ToString();
						for (var i = displayRightSide.Length; i < specialNumberDecimalCount; i++)
						{
#pragma warning disable S1643 // Strings should not be concatenated using '+' in a loop
							displayRightSide = "0" + displayRightSide;
#pragma warning restore S1643 // Strings should not be concatenated using '+' in a loop
						}
					}
					else
					{
						displayRightSide = displayRightSide.Substring(0, specialNumberDecimalCount);
					}
				}

				for (var i = displayRightSide.Length; i < specialNumberDecimalCount; i++)
				{
#pragma warning disable S1643 // Strings should not be concatenated using '+' in a loop
					displayRightSide += "0";
#pragma warning restore S1643 // Strings should not be concatenated using '+' in a loop
				}
			}

			this.DisplayText = this.leftSide + Resources.CALC_DECIMAL_SEPARATOR + displayRightSide;
			this.SetDisplayNumber();
		}

		private void BuildSidesFromNumber(double number)
		{
			var roundedResult = Math.Round(number, 9);
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

		private void ResetNumbers(bool resetSpecialFunctionNumbers = false)
		{
			this.firstNumber = 0;
			this.secondNumber = 0;

			if (resetSpecialFunctionNumbers)
			{
				this.interestNumber = 0;
				this.ratesPerAnnumNumber = 12;
			}
		}

		private void ResetSpecialFunctionLabels(bool resetAdvanceStatusBarToo = false)
		{
			if (resetAdvanceStatusBarToo)
			{
				this.AdvanceStatusBarText = string.Empty;
			}

			this.YearsStatusBarText = string.Empty;
			this.InterestStatusBarText = string.Empty;
			this.StartStatusBarText = string.Empty;
			this.RateStatusBarText = string.Empty;
			this.EndStatusBarText = string.Empty;
		}
	}
}
