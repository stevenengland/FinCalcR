using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Caliburn.Micro;
using StEn.FinCalcR.Calculations;
using StEn.FinCalcR.Calculations.Calculator;
using StEn.FinCalcR.Calculations.Calculator.Events;
using StEn.FinCalcR.Common.Extensions;
using StEn.FinCalcR.Common.LanguageResources;
using StEn.FinCalcR.Common.Services.Localization;
using StEn.FinCalcR.WinUi.Commanding;
using StEn.FinCalcR.WinUi.Events;
using StEn.FinCalcR.WinUi.Events.EventArgs;
using StEn.FinCalcR.WinUi.Types;

namespace StEn.FinCalcR.WinUi.ViewModels
{
	public class FinCalcViewModel2 : Screen, IHandle<KeyboardKeyDownEvent>
	{
		private const int LongTouchDelay = 2;
#pragma warning disable S1450 // Private fields only used as local variables in methods should become local variables
		private readonly ILocalizationService localizationService;
#pragma warning restore S1450 // Private fields only used as local variables in methods should become local variables
		private readonly IEventAggregator eventAggregator;
		private readonly ICommandInvoker calculatorRemote;
		private readonly ICalculationCommandReceiver calculator;
		private string displayText;
		private double displayNumber;
		private bool isDisplayTextNumeric = true;
		private bool isDecimalSeparatorActive = false;
		private string leftSide;
		private string rightSide;

		private string advanceStatusBarText; // Remains in VM
		private string yearsStatusBarText; // Remains in VM
		private string interestStatusBarText; // Remains in VM
		private string startStatusBarText; // Remains in VM
		private string rateStatusBarText; // Remains in VM
		private string endStatusBarText; // Remains in VM

		public FinCalcViewModel2(
			ILocalizationService localizationService,
			IEventAggregator eventAggregator,
			ICommandInvoker calculatorRemote,
			ICalculationCommandReceiver calculator)
		{
			this.localizationService = localizationService;
			this.eventAggregator = eventAggregator;
			this.calculatorRemote = calculatorRemote;
			this.calculator = calculator;

			this.eventAggregator?.Subscribe(this);
			this.calculator.OutputText.TextChanged += this.OnOutputTextChanged;

			this.OnClearPressed();
		}

		public double YearsNumber => this.calculator.MemoryFields.Get<double>(MemoryFieldNames.YearsNumber).Value;

		public double InterestNumber => this.calculator.MemoryFields.Get<double>(MemoryFieldNames.InterestNumber).Value;

		public double NominalInterestRateNumber => this.calculator.MemoryFields.Get<double>(MemoryFieldNames.NominalInterestRateNumber).Value;

		public double RepaymentRateNumber => this.calculator.MemoryFields.Get<double>(MemoryFieldNames.RepaymentRateNumber).Value;

		public double StartNumber => this.calculator.MemoryFields.Get<double>(MemoryFieldNames.StartNumber).Value;

		public double RateNumber => this.calculator.MemoryFields.Get<double>(MemoryFieldNames.RateNumber).Value;

		public double EndNumber => this.calculator.MemoryFields.Get<double>(MemoryFieldNames.EndNumber).Value;

		public LastPressedOperation LastPressedOperation { get; set; } = LastPressedOperation.None;

		public PressedSpecialFunctions PressedSpecialFunctions { get; private set; }

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

		public MathOperator ActiveMathOperator
		{
			get => this.calculator.ActiveMathOperator;
			set
			{
				this.calculator.ActiveMathOperator = value;
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

		public IAsyncCommand<IGestureHandler> InterestPressedCommandAsync => new AsyncCommand<IGestureHandler>(this.OnInterestPressedAsync);

		public ICommand InterestPressedCommand => new SyncCommand<bool>(this.OnInterestPressed);

		public ICommand StartPressedCommand => new SyncCommand<bool>(this.OnStartPressed);

		public ICommand RatePressedCommand => new SyncCommand<bool>(this.OnRatePressed);

		public ICommand EndPressedCommand => new SyncCommand<bool>(this.OnEndPressed);

		public ICommand KeyboardKeyPressedCommand => new SyncCommand<MappedKeyEventArgs>(this.OnKeyboardKeyPressed);

		public void Handle(KeyboardKeyDownEvent message) => this.KeyboardKeyPressedCommand.Execute(message.KeyEventArgs);

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
			var isLongTouch = await gestureHandler.IsLongTouchAsync(TimeSpan.FromSeconds(LongTouchDelay));
			this.InterestPressedCommand.Execute(isLongTouch);
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

		public bool IsAdvance()
		{
			return this.calculator.MemoryFields.Get<bool>(MemoryFieldNames.IsAdvance).Value;
		}

		public void OnKeyboardKeyPressed(object sender, KeyEventArgs e) // For Caliburn Micro
		{
			this.KeyboardKeyPressedCommand.Execute(new MappedKeyEventArgs(e.Key.ToString()));
		}

		private void OnKeyboardKeyPressed(MappedKeyEventArgs e)
		{
			switch (e.Key)
			{
				case "D0":
				case "NumPad0":
					this.DigitPressedCommand.Execute(0);
					break;
				case "D1":
				case "NumPad1":
					this.DigitPressedCommand.Execute(1);
					break;
				case "D2":
				case "NumPad2":
					this.DigitPressedCommand.Execute(2);
					break;
				case "D3":
				case "NumPad3":
					this.DigitPressedCommand.Execute(3);
					break;
				case "D4":
				case "NumPad4":
					this.DigitPressedCommand.Execute(4);
					break;
				case "D5":
				case "NumPad5":
					this.DigitPressedCommand.Execute(5);
					break;
				case "D6":
				case "NumPad6":
					this.DigitPressedCommand.Execute(6);
					break;
				case "D7":
					if (e.IsShiftPressed)
					{
						this.OperatorPressedCommand.Execute("/");
					}
					else
					{
						this.DigitPressedCommand.Execute(7);
					}

					break;
				case "NumPad7":
					this.DigitPressedCommand.Execute(7);
					break;
				case "D8":
				case "NumPad8":
					this.DigitPressedCommand.Execute(8);
					break;
				case "D9":
				case "NumPad9":
					this.DigitPressedCommand.Execute(9);
					break;
				case "Add":
					this.OperatorPressedCommand.Execute("+");
					break;
				case "Subtract":
					this.OperatorPressedCommand.Execute("-");
					break;
				case "Divide":
					this.OperatorPressedCommand.Execute("/");
					break;
				case "Multiply":
					this.OperatorPressedCommand.Execute("*");
					break;
				case "OemPlus":
					if (e.IsShiftPressed)
					{
						this.OperatorPressedCommand.Execute("*");
					}

					break;
				case "Return":
					this.CalculatePressedCommand.Execute(null);
					break;
				case "Decimal":
				case "OemComma":
				case "OemPeriod":
					this.DecimalSeparatorPressedCommand.Execute(null);
					break;
				case "Delete":
					if (e.IsShiftPressed)
					{
						this.ClearPressedCommand.Execute(true);
					}
					else
					{
						this.ClearPressedCommand.Execute(false);
					}

					break;
				case "OemQuestion":
					this.AlgebSignCommand.Execute(null);
					break;
				case "F1":
					this.YearsPressedCommand.Execute(e.IsShiftPressed);
					break;
				case "F2":
					this.InterestPressedCommand.Execute(e.IsShiftPressed);
					break;
				case "F3":
					this.StartPressedCommand.Execute(e.IsShiftPressed);
					break;
				case "F4":
					this.RatePressedCommand.Execute(e.IsShiftPressed);
					break;
				case "F5":
					this.EndPressedCommand.Execute(e.IsShiftPressed);
					break;
				case "F6":
					this.OperatorPressedCommand.Execute("*");
					this.YearsPressedCommand.Execute(e.IsShiftPressed);
					break;
				case "F7":
					this.OperatorPressedCommand.Execute("*");
					this.InterestPressedCommand.Execute(e.IsShiftPressed);
					break;
				case "F8":
					this.OperatorPressedCommand.Execute("*");
					this.StartPressedCommand.Execute(e.IsShiftPressed);
					break;
				case "F9":
					this.OperatorPressedCommand.Execute("*");
					this.RatePressedCommand.Execute(e.IsShiftPressed);
					break;
				default:
					Debug.WriteLine(e.Key);
					break;
			}
		}

		private void OnClearPressed(bool isLongTouch = false)
		{
			if (isLongTouch)
			{
				this.eventAggregator.PublishOnUIThread(new HintEvent(Resources.HINT_SPECIAL_FUNCTION_MEMORY_RESET));
				this.ResetSpecialFunctionLabels(true);
				this.ResetNumbers(true);
				this.PressedSpecialFunctions = this.PressedSpecialFunctions.SetAllFlags(false);
			}
			else
			{
				this.ResetSpecialFunctionLabels();
				this.ResetNumbers();
			}

			this.ResetSides();
			this.ActiveMathOperator = MathOperator.None;
			this.calculator.IsCalcCommandLock = false;

			this.SetDisplayText();

			this.LastPressedOperation = LastPressedOperation.Clear;
		}

		private void OnYearsPressed(bool isLongTouch = false)
		{
			this.ResetSpecialFunctionLabels();
			this.YearsStatusBarText = Resources.FinCalcFunctionYears;

			// Special - if the last pressed operation was a special function this current special function should not work with old values.
			if (!isLongTouch && this.IsLastPressedOperationSpecialFunction())
			{
				this.ResetSides();
				this.ResetNumbers();
			}

			// Check if it is a second function call
			if (this.LastPressedOperation == LastPressedOperation.Operator && this.ActiveMathOperator == MathOperator.Multiply)
			{
				this.OnYearsSecondFunctionPressed(isLongTouch);
				return;
			}

			if (isLongTouch)
			{
				// Display the value in the memory
				this.CommonSpecialFunctionReadFromMemoryOperations(this.calculator.MemoryFields.Get<double>(MemoryFieldNames.YearsNumber).Value, 2);
			}
			else
			{
				// Write the value to the memory
				if ((this.PressedSpecialFunctions.IsOnlyFlagNotSet(PressedSpecialFunctions.Years) && this.IsLastPressedOperationSpecialFunction())
					|| this.LastPressedOperation == LastPressedOperation.Years)
				{
					var tmpYearsNumber = this.CalculateAndCheckResult(
						true,
						new Func<double, double, double, double, double, bool, double>(FinancialCalculator.N),
						this.calculator.MemoryFields.Get<double>(MemoryFieldNames.EndNumber).Value,
						this.calculator.MemoryFields.Get<double>(MemoryFieldNames.StartNumber).Value,
						this.calculator.MemoryFields.Get<double>(MemoryFieldNames.RateNumber).Value,
						this.calculator.MemoryFields.Get<double>(MemoryFieldNames.NominalInterestRateNumber).Value,
						this.calculator.MemoryFields.Get<int>(MemoryFieldNames.RatesPerAnnumNumber).Value,
						this.calculator.MemoryFields.Get<bool>(MemoryFieldNames.IsAdvance).Value);

					if (this.IsNumber(tmpYearsNumber))
					{
						this.BuildSidesFromNumber(tmpYearsNumber);
						this.CommonSpecialFunctionWriteToMemoryOperations(out var tmpVar, 2);
						this.calculator.MemoryFields.Get<double>(MemoryFieldNames.YearsNumber).Value = tmpVar;
					}
					else
					{
						// Don't display NaN or other non numeric values that might be the result of the calculation.
						this.CommonSpecialFunctionReadFromMemoryOperations(0, 2);
					}
				}
				else
				{
					var tmpYearsNumber = this.calculator.MemoryFields.Get<double>(MemoryFieldNames.YearsNumber).Value;
					this.CommonSpecialFunctionWriteToMemoryOperations(out var tmpVar, 2);
					this.calculator.MemoryFields.Get<double>(MemoryFieldNames.YearsNumber).Value = tmpVar;
					if (this.calculator.MemoryFields.Get<double>(MemoryFieldNames.YearsNumber).Value < 0)
					{
						this.ResetSides();
						this.ResetNumbers();
						this.SetDisplayText();
						this.calculator.MemoryFields.Get<double>(MemoryFieldNames.YearsNumber).Value = tmpYearsNumber;
						this.eventAggregator.PublishOnUIThread(new ErrorEvent(Resources.EXC_INTEREST_EXCEEDED_LIMIT));
					}
				}
			}

			this.PressedSpecialFunctions = this.PressedSpecialFunctions.SetFlag(PressedSpecialFunctions.Years, true);
			this.LastPressedOperation = LastPressedOperation.Years;
		}

		private void OnYearsSecondFunctionPressed(bool isLongTouch)
		{
			if (isLongTouch)
			{
				// Output saved rates
				this.ResetNumbers();
				this.calculator.MemoryFields.Get<double>(MemoryFieldNames.PreOperatorNumber).Value = this.calculator.MemoryFields.Get<int>(MemoryFieldNames.RatesPerAnnumNumber).Value;
				this.BuildSidesFromNumber(this.calculator.MemoryFields.Get<int>(MemoryFieldNames.RatesPerAnnumNumber).Value);
				this.ActiveMathOperator = MathOperator.None;
				this.SetDisplayText(this.calculator.MemoryFields.Get<int>(MemoryFieldNames.RatesPerAnnumNumber).Value + " " + Resources.FinCalcRatesPerAnnumPostfix);
			}
			else
			{
				// Write the value to the memory
				this.CommonSpecialFunctionWriteToMemoryOperations(out var tmpRpaNumber, 0, false);
				if (tmpRpaNumber < 1
					|| tmpRpaNumber > 365
					|| tmpRpaNumber != Math.Truncate(tmpRpaNumber))
				{
					this.ResetSides();
					this.ResetNumbers();
					this.SetDisplayText();
					this.eventAggregator.PublishOnUIThread(new ErrorEvent(Resources.EXC_INTEREST_EXCEEDED_LIMIT));
				}
				else
				{
					this.calculator.MemoryFields.Get<int>(MemoryFieldNames.RatesPerAnnumNumber).Value = (int)tmpRpaNumber;
					this.SetDisplayText(this.calculator.MemoryFields.Get<int>(MemoryFieldNames.RatesPerAnnumNumber).Value + " " + Resources.FinCalcRatesPerAnnumPostfix);
				}
			}

			this.PressedSpecialFunctions = this.PressedSpecialFunctions.SetFlag(PressedSpecialFunctions.Years, true);
			this.LastPressedOperation = LastPressedOperation.RatesPerAnnum;
		}

		private async Task OnInterestPressedAsync(IGestureHandler handler)
		{
			var isLongTouch = await handler.IsLongTouchAsync(TimeSpan.FromSeconds(LongTouchDelay));
			this.OnInterestPressed(isLongTouch);
		}

		private void OnInterestPressed(bool isLongTouch)
		{
			// Prepare
			this.ResetSpecialFunctionLabels();
			this.InterestStatusBarText = Resources.FinCalcFunctionInterest;

			// Special - if the last pressed operation was a special function this current special function should not work with old values.
			if (!isLongTouch && this.IsLastPressedOperationSpecialFunction())
			{
				this.ResetSides();
				this.ResetNumbers();
			}

			// Check if it is a second function call
			if (this.LastPressedOperation == LastPressedOperation.Operator && this.ActiveMathOperator == MathOperator.Multiply)
			{
				this.OnInterestSecondFunctionPressed(isLongTouch);
				return;
			}

			// Proceed as standard function
			if (isLongTouch)
			{
				// Display the value in the memory
				this.CommonSpecialFunctionReadFromMemoryOperations(this.calculator.MemoryFields.Get<double>(MemoryFieldNames.InterestNumber).Value, 3);
			}
			else
			{
				// Write the value to the memory
				if ((this.PressedSpecialFunctions.IsOnlyFlagNotSet(PressedSpecialFunctions.Interest) && this.IsLastPressedOperationSpecialFunction())
					|| this.LastPressedOperation == LastPressedOperation.Interest)
				{
					var tmpInterestNumber = this.CalculateAndCheckResult(true, new Func<double, double, double, double, double, bool, int, double>(FinancialCalculator.P), (-1) * this.calculator.MemoryFields.Get<double>(MemoryFieldNames.EndNumber).Value, this.calculator.MemoryFields.Get<double>(MemoryFieldNames.StartNumber).Value, this.calculator.MemoryFields.Get<double>(MemoryFieldNames.RateNumber).Value, this.calculator.MemoryFields.Get<double>(MemoryFieldNames.YearsNumber).Value, this.calculator.MemoryFields.Get<int>(MemoryFieldNames.RatesPerAnnumNumber).Value, this.calculator.MemoryFields.Get<bool>(MemoryFieldNames.IsAdvance).Value, 50);

					if (this.IsNumber(tmpInterestNumber))
					{
						this.BuildSidesFromNumber(tmpInterestNumber);
						this.CommonSpecialFunctionWriteToMemoryOperations(out var tmpVar, 3);
						this.calculator.MemoryFields.Get<double>(MemoryFieldNames.InterestNumber).Value = tmpVar;
						this.calculator.MemoryFields.Get<double>(MemoryFieldNames.NominalInterestRateNumber).Value = this.CalculateAndCheckResult(false, new Func<double, double, double>(FinancialCalculator.GetYearlyNominalInterestRate), this.calculator.MemoryFields.Get<int>(MemoryFieldNames.RatesPerAnnumNumber).Value, this.calculator.MemoryFields.Get<double>(MemoryFieldNames.InterestNumber).Value);
					}
					else
					{
						// Don't display NaN or other non numeric values that might be the result of the calculation.
						this.CommonSpecialFunctionReadFromMemoryOperations(0, 3);
					}
				}
				else
				{
					var tmpInterestNumber = this.calculator.MemoryFields.Get<double>(MemoryFieldNames.InterestNumber).Value;
					this.CommonSpecialFunctionWriteToMemoryOperations(out var tmpVar, 3);
					this.calculator.MemoryFields.Get<double>(MemoryFieldNames.InterestNumber).Value = tmpVar;
					if (this.calculator.MemoryFields.Get<double>(MemoryFieldNames.InterestNumber).Value < -100)
					{
						this.ResetSides();
						this.ResetNumbers();
						this.SetDisplayText();
						this.calculator.MemoryFields.Get<double>(MemoryFieldNames.InterestNumber).Value = tmpInterestNumber;
						this.eventAggregator.PublishOnUIThread(new ErrorEvent(Resources.EXC_INTEREST_EXCEEDED_LIMIT));
					}
					else
					{
						this.calculator.MemoryFields.Get<double>(MemoryFieldNames.NominalInterestRateNumber).Value = this.CalculateAndCheckResult(false, new Func<double, double, double>(FinancialCalculator.GetYearlyNominalInterestRate), this.calculator.MemoryFields.Get<int>(MemoryFieldNames.RatesPerAnnumNumber).Value, this.calculator.MemoryFields.Get<double>(MemoryFieldNames.InterestNumber).Value);
					}
				}
			}

			this.PressedSpecialFunctions = this.PressedSpecialFunctions.SetFlag(PressedSpecialFunctions.Interest, true);
			this.LastPressedOperation = LastPressedOperation.Interest;
		}

		private void OnInterestSecondFunctionPressed(bool isLongTouch = false)
		{
			if (isLongTouch)
			{
				// Output saved nominal interest
				this.calculator.MemoryFields.Get<double>(MemoryFieldNames.NominalInterestRateNumber).Value = this.CalculateAndCheckResult(true, new Func<double, double, double>(FinancialCalculator.GetYearlyNominalInterestRate), this.calculator.MemoryFields.Get<int>(MemoryFieldNames.RatesPerAnnumNumber).Value, this.calculator.MemoryFields.Get<double>(MemoryFieldNames.InterestNumber).Value);
				this.CommonSpecialFunctionReadFromMemoryOperations(this.calculator.MemoryFields.Get<double>(MemoryFieldNames.NominalInterestRateNumber).Value, 3);
			}
			else
			{
				// Calculate/save effective interest, save nominal interest (as interest) and display the effective interest.
				var tmpNominalInterestNumber = this.calculator.MemoryFields.Get<double>(MemoryFieldNames.NominalInterestRateNumber).Value;
				this.CommonSpecialFunctionWriteToMemoryOperations(out var tmpVar, 3, false);
				this.calculator.MemoryFields.Get<double>(MemoryFieldNames.NominalInterestRateNumber).Value = tmpVar;
				if (this.calculator.MemoryFields.Get<double>(MemoryFieldNames.NominalInterestRateNumber).Value < -100)
				{
					this.ResetSides();
					this.ResetNumbers();
					this.SetDisplayText();
					this.calculator.MemoryFields.Get<double>(MemoryFieldNames.NominalInterestRateNumber).Value = tmpNominalInterestNumber;
					this.eventAggregator.PublishOnUIThread(new ErrorEvent(Resources.EXC_INTEREST_EXCEEDED_LIMIT));
				}
				else
				{
					this.calculator.MemoryFields.Get<double>(MemoryFieldNames.InterestNumber).Value = this.CalculateAndCheckResult(false, new Func<double, double, double>((m, p) => FinancialCalculator.GetEffectiveInterestRate(p, m)), this.calculator.MemoryFields.Get<int>(MemoryFieldNames.RatesPerAnnumNumber).Value, this.calculator.MemoryFields.Get<double>(MemoryFieldNames.NominalInterestRateNumber).Value);
					this.calculator.MemoryFields.Get<double>(MemoryFieldNames.PreOperatorNumber).Value = this.calculator.MemoryFields.Get<double>(MemoryFieldNames.InterestNumber).Value;
					this.BuildSidesFromNumber(this.calculator.MemoryFields.Get<double>(MemoryFieldNames.InterestNumber).Value);
					this.SetDisplayText(true, 3);
				}
			}

			this.PressedSpecialFunctions = this.PressedSpecialFunctions.SetFlag(PressedSpecialFunctions.Interest, true);
			this.LastPressedOperation = LastPressedOperation.Interest;
		}

		private void OnStartPressed(bool isLongTouch = false)
		{
			this.ResetSpecialFunctionLabels();

			// Special - if the last pressed operation was a special function this current special function should not work with old values.
			if (!isLongTouch && this.IsLastPressedOperationSpecialFunction())
			{
				this.ResetSides();
				this.ResetNumbers();
			}

			// Check if it is a second function call
			if (this.LastPressedOperation == LastPressedOperation.Operator && this.ActiveMathOperator == MathOperator.Multiply)
			{
				this.OnStartSecondFunctionPressed();
				return;
			}

			this.StartStatusBarText = Resources.FinCalcFunctionStart;

			if (isLongTouch)
			{
				// Display the value in the memory
				this.CommonSpecialFunctionReadFromMemoryOperations(this.calculator.MemoryFields.Get<double>(MemoryFieldNames.StartNumber).Value, 2);
			}
			else
			{
				// Write the value to the memory
				if ((this.PressedSpecialFunctions.IsOnlyFlagNotSet(PressedSpecialFunctions.Start) && this.IsLastPressedOperationSpecialFunction())
					|| this.LastPressedOperation == LastPressedOperation.Start)
				{
					var tmpStartNumber = this.CalculateAndCheckResult(true, new Func<double, double, double, double, double, bool, double>(FinancialCalculator.K0), this.calculator.MemoryFields.Get<double>(MemoryFieldNames.EndNumber).Value, this.calculator.MemoryFields.Get<double>(MemoryFieldNames.RateNumber).Value, this.calculator.MemoryFields.Get<double>(MemoryFieldNames.NominalInterestRateNumber).Value, this.calculator.MemoryFields.Get<double>(MemoryFieldNames.YearsNumber).Value, this.calculator.MemoryFields.Get<int>(MemoryFieldNames.RatesPerAnnumNumber).Value, this.calculator.MemoryFields.Get<bool>(MemoryFieldNames.IsAdvance).Value);

					if (this.IsNumber(tmpStartNumber))
					{
						this.BuildSidesFromNumber(tmpStartNumber);
						this.CommonSpecialFunctionWriteToMemoryOperations(out var tmpVar, 2);
						this.calculator.MemoryFields.Get<double>(MemoryFieldNames.StartNumber).Value = tmpVar;
					}
					else
					{
						// Don't display NaN or other non numeric values that might be the result of the calculation.
						this.CommonSpecialFunctionReadFromMemoryOperations(0, 2);
					}
				}
				else
				{
					this.CommonSpecialFunctionWriteToMemoryOperations(out var tmpVar, 2);
					this.calculator.MemoryFields.Get<double>(MemoryFieldNames.StartNumber).Value = tmpVar;
				}
			}

			this.PressedSpecialFunctions = this.PressedSpecialFunctions.SetFlag(PressedSpecialFunctions.Start, true);
			this.LastPressedOperation = LastPressedOperation.Start;
		}

		private void OnStartSecondFunctionPressed()
		{
			if (this.calculator.MemoryFields.Get<bool>(MemoryFieldNames.IsAdvance).Value)
			{
				this.calculator.MemoryFields.Get<bool>(MemoryFieldNames.IsAdvance).Value = false;
				this.AdvanceStatusBarText = string.Empty;
			}
			else
			{
				this.calculator.MemoryFields.Get<bool>(MemoryFieldNames.IsAdvance).Value = true;
				this.AdvanceStatusBarText = Resources.FinCalcFunctionAdvance;
			}

			this.PressedSpecialFunctions = this.PressedSpecialFunctions.SetFlag(PressedSpecialFunctions.Start, true);
			this.LastPressedOperation = LastPressedOperation.Start;
		}

		private void OnRatePressed(bool isLongTouch = false)
		{
			this.ResetSpecialFunctionLabels();
			this.RateStatusBarText = Resources.FinCalcFunctionRate;

			// Special - if the last pressed operation was a special function this current special function should not work with old values.
			if (!isLongTouch && this.IsLastPressedOperationSpecialFunction())
			{
				this.ResetSides();
				this.ResetNumbers();
			}

			// Check if it is a second function call
			if (this.LastPressedOperation == LastPressedOperation.Operator && this.ActiveMathOperator == MathOperator.Multiply)
			{
				this.OnRateSecondFunctionPressed(isLongTouch);
				return;
			}

			if (isLongTouch)
			{
				// Display the value in the memory
				this.CommonSpecialFunctionReadFromMemoryOperations(this.calculator.MemoryFields.Get<double>(MemoryFieldNames.RateNumber).Value, 2);
			}
			else
			{
				// Write the value to the memory
				if ((this.PressedSpecialFunctions.IsOnlyFlagNotSet(PressedSpecialFunctions.Rate) && this.IsLastPressedOperationSpecialFunction())
					|| this.LastPressedOperation == LastPressedOperation.Rate)
				{
					var tmpRateNumber = (-1) * this.CalculateAndCheckResult(true, new Func<double, double, double, double, double, bool, double>(FinancialCalculator.E), this.calculator.MemoryFields.Get<double>(MemoryFieldNames.EndNumber).Value, this.calculator.MemoryFields.Get<double>(MemoryFieldNames.StartNumber).Value, this.calculator.MemoryFields.Get<double>(MemoryFieldNames.NominalInterestRateNumber).Value, this.calculator.MemoryFields.Get<double>(MemoryFieldNames.YearsNumber).Value, this.calculator.MemoryFields.Get<int>(MemoryFieldNames.RatesPerAnnumNumber).Value, this.calculator.MemoryFields.Get<bool>(MemoryFieldNames.IsAdvance).Value);

					if (this.IsNumber(tmpRateNumber))
					{
						this.BuildSidesFromNumber(tmpRateNumber);
						this.CommonSpecialFunctionWriteToMemoryOperations(out var tmpVar, 2);
						this.calculator.MemoryFields.Get<double>(MemoryFieldNames.RateNumber).Value = tmpVar;
					}
					else
					{
						// Don't display NaN or other non numeric values that might be the result of the calculation.
						this.CommonSpecialFunctionReadFromMemoryOperations(0, 2);
					}
				}
				else
				{
					// Write the values to the memory
					this.CommonSpecialFunctionWriteToMemoryOperations(out var tmpVar, 2);
					this.calculator.MemoryFields.Get<double>(MemoryFieldNames.RateNumber).Value = tmpVar;
					this.calculator.MemoryFields.Get<double>(MemoryFieldNames.RepaymentRateNumber).Value = this.CalculateAndCheckResult(false, new Func<double, double, double, double, double>((m, k0, p, annuity) => FinancialCalculator.GetRepaymentRate(k0, p, m, annuity)), this.calculator.MemoryFields.Get<int>(MemoryFieldNames.RatesPerAnnumNumber).Value, this.calculator.MemoryFields.Get<double>(MemoryFieldNames.StartNumber).Value, this.calculator.MemoryFields.Get<double>(MemoryFieldNames.NominalInterestRateNumber).Value, (-1) * this.calculator.MemoryFields.Get<double>(MemoryFieldNames.RateNumber).Value);
				}
			}

			this.PressedSpecialFunctions = this.PressedSpecialFunctions.SetFlag(PressedSpecialFunctions.Rate, true);
			this.LastPressedOperation = LastPressedOperation.Rate;
		}

		private void OnRateSecondFunctionPressed(bool isLongTouch)
		{
			if (isLongTouch)
			{
				// Output saved repayment rate
				this.calculator.MemoryFields.Get<double>(MemoryFieldNames.RepaymentRateNumber).Value = this.CalculateAndCheckResult(true, new Func<double, double, double, double, double>((m, k0, p, annuity) => FinancialCalculator.GetRepaymentRate(k0, p, m, annuity)), this.calculator.MemoryFields.Get<int>(MemoryFieldNames.RatesPerAnnumNumber).Value, this.calculator.MemoryFields.Get<double>(MemoryFieldNames.StartNumber).Value, this.calculator.MemoryFields.Get<double>(MemoryFieldNames.NominalInterestRateNumber).Value, (-1) * this.calculator.MemoryFields.Get<double>(MemoryFieldNames.RateNumber).Value);
				if (this.IsNumber(this.calculator.MemoryFields.Get<double>(MemoryFieldNames.RepaymentRateNumber).Value))
				{
					this.CommonSpecialFunctionReadFromMemoryOperations(this.calculator.MemoryFields.Get<double>(MemoryFieldNames.RepaymentRateNumber).Value, 2);
				}
				else
				{
					// Don't display NaN or other non numeric values that might be the result of the calculation.
					this.CommonSpecialFunctionReadFromMemoryOperations(0, 2);
				}
			}
			else
			{
				// Calculate/save repayment, save repayment (as rate) and display the repayment.
				this.CommonSpecialFunctionWriteToMemoryOperations(out var tmpVar, 2, false);
				this.calculator.MemoryFields.Get<double>(MemoryFieldNames.RepaymentRateNumber).Value = tmpVar;

				this.calculator.MemoryFields.Get<double>(MemoryFieldNames.RateNumber).Value = (-1) * this.CalculateAndCheckResult(false, new Func<double, double, double, double, double>((m, k0, p, e) => FinancialCalculator.GetAnnuity(k0, e, p, m)), this.calculator.MemoryFields.Get<int>(MemoryFieldNames.RatesPerAnnumNumber).Value, this.calculator.MemoryFields.Get<double>(MemoryFieldNames.StartNumber).Value, this.calculator.MemoryFields.Get<double>(MemoryFieldNames.NominalInterestRateNumber).Value, this.calculator.MemoryFields.Get<double>(MemoryFieldNames.RepaymentRateNumber).Value);
				this.calculator.MemoryFields.Get<double>(MemoryFieldNames.PreOperatorNumber).Value = this.calculator.MemoryFields.Get<double>(MemoryFieldNames.RateNumber).Value;
				this.BuildSidesFromNumber(this.calculator.MemoryFields.Get<double>(MemoryFieldNames.RateNumber).Value);
				this.SetDisplayText(true, 2);
			}

			this.PressedSpecialFunctions = this.PressedSpecialFunctions.SetFlag(PressedSpecialFunctions.Rate, true);
			this.LastPressedOperation = LastPressedOperation.Rate;
		}

		private void OnEndPressed(bool isLongTouch = false)
		{
			this.ResetSpecialFunctionLabels();
			this.EndStatusBarText = Resources.FinCalcFunctionEnd;

			// Special - if the last pressed operation was a special function this current special function should not work with old values.
			if (!isLongTouch && this.IsLastPressedOperationSpecialFunction())
			{
				this.ResetSides();
				this.ResetNumbers();
			}

			if (isLongTouch)
			{
				// Display the value in the memory
				this.CommonSpecialFunctionReadFromMemoryOperations(this.calculator.MemoryFields.Get<double>(MemoryFieldNames.EndNumber).Value, 2);
			}
			else
			{
				// Percentage calculation <-- On the physical calculator it is marked a second function but is triggered as standard function
				if ((this.LastPressedOperation == LastPressedOperation.Digit
					 || this.LastPressedOperation == LastPressedOperation.AlgebSign
					 || this.LastPressedOperation == LastPressedOperation.Decimal)
					&& this.ActiveMathOperator != MathOperator.None)
				{
					this.SetNumber(out var tmpVar);
					this.calculator.MemoryFields.Get<double>(MemoryFieldNames.PostOperatorNumber).Value = tmpVar;
					var tmpResult = double.NaN;
					switch (this.ActiveMathOperator)
					{
						case MathOperator.Multiply:
							tmpResult = this.CalculateAndCheckResult(true, new Func<double, double, double>(SimpleCalculator.GetPartValue), this.calculator.MemoryFields.Get<double>(MemoryFieldNames.PreOperatorNumber).Value, this.calculator.MemoryFields.Get<double>(MemoryFieldNames.PostOperatorNumber).Value);
							break;
						case MathOperator.Add:
							tmpResult = this.CalculateAndCheckResult(true, new Func<double, double, double>(SimpleCalculator.AddPartValueToBaseValue), this.calculator.MemoryFields.Get<double>(MemoryFieldNames.PreOperatorNumber).Value, this.calculator.MemoryFields.Get<double>(MemoryFieldNames.PostOperatorNumber).Value);
							break;
						case MathOperator.Subtract:
							tmpResult = this.CalculateAndCheckResult(true, new Func<double, double, double>(SimpleCalculator.SubPartValueFromBaseValue), this.calculator.MemoryFields.Get<double>(MemoryFieldNames.PreOperatorNumber).Value, this.calculator.MemoryFields.Get<double>(MemoryFieldNames.PostOperatorNumber).Value);
							break;
						case MathOperator.Divide: // function is not documented and calculates like below - but makes not much sense...
							tmpResult = this.CalculateAndCheckResult(
								true,
								new Func<double, double, double>(
									(baseValue, rate) => baseValue / rate * 100),
								this.calculator.MemoryFields.Get<double>(MemoryFieldNames.PreOperatorNumber).Value,
								this.calculator.MemoryFields.Get<double>(MemoryFieldNames.PostOperatorNumber).Value);
							break;
					}

					if (this.IsNumber(tmpResult))
					{
						this.ResetNumbers();
						this.calculator.MemoryFields.Get<double>(MemoryFieldNames.PreOperatorNumber).Value = tmpResult;
						this.BuildSidesFromNumber(tmpResult);
						this.ActiveMathOperator = MathOperator.None;
						this.SetDisplayText(true, 2);
						this.calculator.IsCalcCommandLock = true;
					}
					else
					{
						// Don't display NaN or other non numeric values that might be the result of the calculation.
						this.CommonSpecialFunctionReadFromMemoryOperations(0, 2);
					}

					this.LastPressedOperation = LastPressedOperation.PercentCalculation;
					return;
				}

				// Write the value to the memory
				else if ((this.PressedSpecialFunctions.IsOnlyFlagNotSet(PressedSpecialFunctions.End) && this.IsLastPressedOperationSpecialFunction())
					|| this.LastPressedOperation == LastPressedOperation.End)
				{
					var tmpEndNumber = (-1) * this.CalculateAndCheckResult(true, new Func<double, double, double, double, double, bool, double>(FinancialCalculator.Kn), this.calculator.MemoryFields.Get<double>(MemoryFieldNames.StartNumber).Value, this.calculator.MemoryFields.Get<double>(MemoryFieldNames.RateNumber).Value, this.calculator.MemoryFields.Get<double>(MemoryFieldNames.NominalInterestRateNumber).Value, this.calculator.MemoryFields.Get<double>(MemoryFieldNames.YearsNumber).Value, this.calculator.MemoryFields.Get<int>(MemoryFieldNames.RatesPerAnnumNumber).Value, this.calculator.MemoryFields.Get<bool>(MemoryFieldNames.IsAdvance).Value);

					if (this.IsNumber(tmpEndNumber))
					{
						this.BuildSidesFromNumber(tmpEndNumber);
						this.CommonSpecialFunctionWriteToMemoryOperations(out var tmpVar, 2);
						this.calculator.MemoryFields.Get<double>(MemoryFieldNames.EndNumber).Value = tmpVar;
					}
					else
					{
						// Don't display NaN or other non numeric values that might be the result of the calculation.
						this.CommonSpecialFunctionReadFromMemoryOperations(0, 2);
					}
				}
				else
				{
					this.CommonSpecialFunctionWriteToMemoryOperations(out var tmpVar, 2);
					this.calculator.MemoryFields.Get<double>(MemoryFieldNames.EndNumber).Value = tmpVar;
				}
			}

			this.PressedSpecialFunctions = this.PressedSpecialFunctions.SetFlag(PressedSpecialFunctions.End, true);
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

			switch (this.LastPressedOperation)
			{
				case LastPressedOperation.Interest:
					this.SetDisplayText(true, 3);
					break;
				case LastPressedOperation.Years:
				case LastPressedOperation.Start:
				case LastPressedOperation.Rate:
				case LastPressedOperation.End:
				case LastPressedOperation.PercentCalculation:
					this.SetDisplayText(true);
					break;
				case LastPressedOperation.RatesPerAnnum:
					this.SetDisplayText();
					break;
				default:
					this.SetDisplayText();
					break;
			}

			this.LastPressedOperation = LastPressedOperation.AlgebSign;
		}

		private void OnDigitPressed(object digitObj)
		{
			this.ResetSpecialFunctionLabels();

			// Special - if the last pressed operation was a special function this operation should not work with old values.
			if (this.IsLastPressedOperationSpecialFunction())
			{
				this.ResetNumbers();
				this.ResetSides();
				this.ActiveMathOperator = MathOperator.None;
			}

			if (this.calculator.IsCalcCommandLock)
			{
				this.ResetSides();
				this.calculator.IsCalcCommandLock = false;
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
						if (this.leftSide == "-0")
						{
							this.leftSide = "-" + digit;
						}
						else
						{
							this.leftSide = digit;
						}
					}
				}
			}

			this.SetDisplayText();

			this.LastPressedOperation = LastPressedOperation.Digit;
		}

		private void OnOperatorPressed(object mathOperatorObj)
		{
			this.ResetSpecialFunctionLabels();

			if (!this.isDisplayTextNumeric)
			{
				this.SetDisplayText();
			}

			if (this.LastPressedOperation == LastPressedOperation.PercentCalculation)
			{
				this.SetDisplayText();
			}

			var tmpOperator = MathOperator.None;
			var strOperator = (string)mathOperatorObj;
			switch (strOperator)
			{
				case "+":
					tmpOperator = MathOperator.Add;
					break;
				case "-":
					tmpOperator = MathOperator.Subtract;
					break;
				case "*":
					tmpOperator = MathOperator.Multiply;
					break;
				case "/":
					tmpOperator = MathOperator.Divide;
					break;
				default:
					break;
			}

			if (this.ActiveMathOperator != MathOperator.None)
			{
				this.OnCalculatePressed();
				this.ActiveMathOperator = tmpOperator;
			}
			else
			{
				this.ActiveMathOperator = tmpOperator;
				this.SetNumber(out var tmpVar);
				this.calculator.MemoryFields.Get<double>(MemoryFieldNames.PreOperatorNumber).Value = tmpVar;
			}

			this.LastPressedOperation = LastPressedOperation.Operator;
		}

		private void OnDecimalSeparatorPressed()
		{
			this.ResetSpecialFunctionLabels();

			if (this.calculator.IsCalcCommandLock)
			{
				this.calculator.IsCalcCommandLock = false;
			}

			if (!this.isDisplayTextNumeric)
			{
				this.SetDisplayText();
			}

			// Special - if the last pressed operation was a special function this operation should not work with old values.
			if (this.IsLastPressedOperationSpecialFunction()

				// Percent calculation -> is not considered a special function yet.
				|| this.LastPressedOperation == LastPressedOperation.PercentCalculation)
			{
				this.ResetNumbers();
				this.ResetSides();
				this.ActiveMathOperator = MathOperator.None;
			}

			this.isDecimalSeparatorActive = true;

			this.LastPressedOperation = LastPressedOperation.Decimal;
		}

		private void OnCalculatePressed()
		{
			this.ResetSpecialFunctionLabels();

			if (this.calculator.IsCalcCommandLock)
			{
				return;
			}

			this.SetNumber(out var tmpVar);
			this.calculator.MemoryFields.Get<double>(MemoryFieldNames.PostOperatorNumber).Value = tmpVar;

			double calculatedResult = 0;

			if (this.ActiveMathOperator != MathOperator.None)
			{
				calculatedResult = this.CalculateAndCheckResult(
					true,
					new Func<double, double, string, double>(SimpleCalculator.Calculate),
					this.calculator.MemoryFields.Get<double>(MemoryFieldNames.PreOperatorNumber).Value,
					this.calculator.MemoryFields.Get<double>(MemoryFieldNames.PostOperatorNumber).Value,
					this.TranslateMathOperator(this.ActiveMathOperator));
			}

			if (this.IsNumber(calculatedResult))
			{
				this.ResetNumbers();
				this.calculator.MemoryFields.Get<double>(MemoryFieldNames.PreOperatorNumber).Value = calculatedResult;
				this.BuildSidesFromNumber(calculatedResult);
				this.ActiveMathOperator = MathOperator.None;
				this.SetDisplayText();
				this.calculator.IsCalcCommandLock = true;
			}

			this.LastPressedOperation = LastPressedOperation.Calculate;
		}

		private string TranslateMathOperator(MathOperator activeMathOperator)
		{
			// TODO: Remove whole function as soon as VMv2 is finished so the old VM does not rely on SimpleCalculator anymore.
			switch (activeMathOperator)
			{
				case MathOperator.None:
					return string.Empty;
				case MathOperator.Add:
					return "+";
				case MathOperator.Subtract:
					return "-";
				case MathOperator.Divide:
					return "/";
				case MathOperator.Multiply:
					return "*";
				default:
					throw new ArgumentOutOfRangeException(nameof(activeMathOperator), activeMathOperator, null);
			}
		}

		private void SetNumber(out double number)
		{
			var realRightSide = string.IsNullOrEmpty(this.rightSide) ? "0" : this.rightSide;
			var numberString = this.leftSide + Resources.CALC_DECIMAL_SEPARATOR + realRightSide;
			if (!double.TryParse(numberString, out number))
			{
				this.eventAggregator.PublishOnUIThread(new ErrorEvent(new ArgumentException(Resources.EXC_PARSE_DOUBLE_IMPOSSIBLE), Resources.EXC_ARGUMENT_INVALID + " " + numberString));
			}

			this.ResetSides();
		}

		private void SetDisplayNumber()
		{
			var realRightSide = string.IsNullOrEmpty(this.rightSide) ? "0" : this.rightSide;
			var concatenatedSides = this.leftSide + Resources.CALC_DECIMAL_SEPARATOR + realRightSide;
			if (!double.TryParse(concatenatedSides, out var parsedNumber))
			{
				// This number is only for background checks and should not throw
				this.DisplayNumber = double.NaN;
			}
			else
			{
				this.DisplayNumber = parsedNumber;
			}
		}

		private void SetDisplayText(string text)
		{
			this.DisplayText = text;
			this.SetDisplayNumber();
			this.isDisplayTextNumeric = false;
		}

		private void SetDisplayText(bool isSpecialFunctionNumber = false, int specialNumberDecimalCount = 2)
		{
			var displayLeftSide = this.InsertThousandSeparator(this.leftSide);
			var displayRightSide = this.rightSide;
			if (isSpecialFunctionNumber)
			{
				if (string.IsNullOrWhiteSpace(displayRightSide))
				{
					displayRightSide = "0";
				}

				if (displayRightSide.Length > specialNumberDecimalCount)
				{
					var concatenatedSides = displayLeftSide + Resources.CALC_DECIMAL_SEPARATOR + displayRightSide;
					if (double.TryParse(concatenatedSides, out var parsedNumber))
					{
						var numberToRound = Math.Round(parsedNumber, specialNumberDecimalCount, MidpointRounding.AwayFromZero);
						var sidesArray = numberToRound.ToString(CultureInfo.CurrentCulture).Split(Resources.CALC_DECIMAL_SEPARATOR.ToCharArray());
						displayLeftSide = this.InsertThousandSeparator(sidesArray[0]);
						displayRightSide = sidesArray.Length > 1 ? sidesArray[1] : "0";
					}
				}

				for (var i = displayRightSide.Length; i < specialNumberDecimalCount; i++)
				{
#pragma warning disable S1643 // Strings should not be concatenated using '+' in a loop
					displayRightSide += "0";
#pragma warning restore S1643 // Strings should not be concatenated using '+' in a loop
				}
			}

			this.DisplayText = displayLeftSide + Resources.CALC_DECIMAL_SEPARATOR + displayRightSide;
			this.SetDisplayNumber();
			this.isDisplayTextNumeric = true;
		}

		private void BuildSidesFromNumber(double number)
		{
			var roundedResult = Math.Round(number, 9);
			var s = roundedResult.ToString(CultureInfo.InvariantCulture);
			var parts = s.Split('.');
			this.leftSide = parts[0];
			this.rightSide = parts.Length < 2 ? string.Empty : parts[1];
		}

		private string InsertThousandSeparator(string inputWithoutSeparator)
		{
			bool hasAlgebSign = false;
			if (inputWithoutSeparator.StartsWith("-"))
			{
				hasAlgebSign = true;
				inputWithoutSeparator = inputWithoutSeparator.Substring(1, inputWithoutSeparator.Length - 1);
			}

			var len = inputWithoutSeparator.Length;
			if (len < 4)
			{
				return hasAlgebSign ? "-" + inputWithoutSeparator : inputWithoutSeparator;
			}

			var result = string.Empty;
			int lastIndex = 1;
			for (var i = inputWithoutSeparator.Length - 3; i > 0; i -= 3)
			{
				lastIndex = i;
#pragma warning disable S1643 // Strings should not be concatenated using '+' in a loop
				result = $"{Resources.CALC_THOUSANDS_SEPARATOR}{inputWithoutSeparator.Substring(i, 3)}" + result;
#pragma warning restore S1643 // Strings should not be concatenated using '+' in a loop
			}

			result = inputWithoutSeparator.Substring(0, lastIndex) + result;

			return hasAlgebSign ? "-" + result : result;
		}

		private void CommonSpecialFunctionWriteToMemoryOperations(out double numberToSet, int specialNumberDecimalCount, bool setDisplayText = true)
		{
			// If last input was an operator restore the firstNumber for upcoming operations
			if (this.LastPressedOperation == LastPressedOperation.Operator)
			{
				this.BuildSidesFromNumber(this.calculator.MemoryFields.Get<double>(MemoryFieldNames.PreOperatorNumber).Value);
			}

			this.SetNumber(out numberToSet);
			this.ResetNumbers();
			this.calculator.MemoryFields.Get<double>(MemoryFieldNames.PreOperatorNumber).Value = numberToSet;
			this.BuildSidesFromNumber(numberToSet); // So that the display text can be set.
			this.ActiveMathOperator = MathOperator.None;
			if (setDisplayText)
			{
				this.SetDisplayText(true, specialNumberDecimalCount);
			}
		}

		private void CommonSpecialFunctionReadFromMemoryOperations(double fistNumberSubstitution, int specialNumberDecimalCount)
		{
			this.ResetNumbers();
			this.calculator.MemoryFields.Get<double>(MemoryFieldNames.PreOperatorNumber).Value = fistNumberSubstitution;
			this.BuildSidesFromNumber(fistNumberSubstitution);
			this.ActiveMathOperator = MathOperator.None;
			this.SetDisplayText(true, specialNumberDecimalCount);
		}

		private bool IsLastPressedOperationSpecialFunction()
		{
			return this.LastPressedOperation == LastPressedOperation.Years
					|| this.LastPressedOperation == LastPressedOperation.Interest
					|| this.LastPressedOperation == LastPressedOperation.Start
					|| this.LastPressedOperation == LastPressedOperation.Rate
					|| this.LastPressedOperation == LastPressedOperation.End
					|| this.LastPressedOperation == LastPressedOperation.RatesPerAnnum;
		}

		private bool IsNumber(double number)
		{
			return !double.IsNaN(number) && !double.IsInfinity(number);
		}

		private double CalculateAndCheckResult(bool notifyIfResultIsNotValid, Delegate method, params object[] args)
		{
			double calculatedResult = 0;
			try
			{
				calculatedResult = (double)method.DynamicInvoke(args);
				if (!this.IsNumber(calculatedResult))
				{
					throw new NotFiniteNumberException();
				}
			}
			catch (CalculationException ex)
			{
				if (notifyIfResultIsNotValid)
				{
					this.eventAggregator.PublishOnUIThread(new ErrorEvent(ex, Resources.EXC_CALC_NOT_POSSIBLE));
					this.OnClearPressed();
				}
			}
			catch (NotFiniteNumberException ex)
			{
				if (notifyIfResultIsNotValid)
				{
					this.eventAggregator.PublishOnUIThread(new ErrorEvent(ex, Resources.EXC_NOT_FINITE_NUMBER));
					this.OnClearPressed();
				}
			}
			catch (DivideByZeroException ex)
			{
				if (notifyIfResultIsNotValid)
				{
					this.eventAggregator.PublishOnUIThread(new ErrorEvent(ex, string.Format(CultureInfo.InvariantCulture, Resources.EXC_DIVISION_BY_ZERO)));
					this.OnClearPressed();
				}
			}
			catch (OverflowException ex)
			{
				if (notifyIfResultIsNotValid)
				{
					this.eventAggregator.PublishOnUIThread(new ErrorEvent(ex, Resources.EXC_OVERFLOW_EXCEPTION));
					this.OnClearPressed();
				}
			}
			catch (NotSupportedException)
			{
				if (notifyIfResultIsNotValid)
				{
					this.eventAggregator.PublishOnUIThread(new ErrorEvent(Resources.EXC_OPERATION_NOT_SUPPORTED));
					this.OnClearPressed();
				}
			}

			return calculatedResult;
		}

		private void ResetSides()
		{
			this.rightSide = string.Empty;
			this.leftSide = "0";
			this.isDecimalSeparatorActive = false;
		}

		private void ResetNumbers(bool resetSpecialFunctionNumbers = false)
		{
			this.calculator.MemoryFields.Get<double>(MemoryFieldNames.PreOperatorNumber).Value = 0;
			this.calculator.MemoryFields.Get<double>(MemoryFieldNames.PostOperatorNumber).Value = 0;

			if (resetSpecialFunctionNumbers)
			{
				this.calculator.MemoryFields.Get<double>(MemoryFieldNames.YearsNumber).Value = 0;
				this.calculator.MemoryFields.Get<double>(MemoryFieldNames.InterestNumber).Value = 0;
				this.calculator.MemoryFields.Get<double>(MemoryFieldNames.StartNumber).Value = 0;
				this.calculator.MemoryFields.Get<double>(MemoryFieldNames.RateNumber).Value = 0;
				this.calculator.MemoryFields.Get<double>(MemoryFieldNames.EndNumber).Value = 0;
				this.calculator.MemoryFields.Get<int>(MemoryFieldNames.RatesPerAnnumNumber).Value = 12;
				this.calculator.MemoryFields.Get<double>(MemoryFieldNames.NominalInterestRateNumber).Value = 0;
				this.calculator.MemoryFields.Get<double>(MemoryFieldNames.RepaymentRateNumber).Value = 0;
				this.calculator.MemoryFields.Get<bool>(MemoryFieldNames.IsAdvance).Value = false;
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

		private void OnOutputTextChanged(object sender, OutputTextChangedEventArgs e)
		{
			this.DisplayText = e.NewText;
			this.DisplayNumber = double.TryParse(this.DisplayText, out var value) ? value : double.NaN;
		}
	}
}
