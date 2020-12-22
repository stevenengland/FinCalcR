using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Caliburn.Micro;
using StEn.FinCalcR.Calculations;
using StEn.FinCalcR.Calculations.Calculator;
using StEn.FinCalcR.Calculations.Calculator.Commands;
using StEn.FinCalcR.Calculations.Calculator.Events;
using StEn.FinCalcR.Calculations.Exceptions;
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
		private double displayNumber; // Remains in VM

		private string advanceStatusBarText; // Remains in VM
		private string yearsStatusBarText; // Remains in VM
		private string interestStatusBarText; // Remains in VM
		private string startStatusBarText; // Remains in VM
		private string rateStatusBarText; // Remains in VM
		private string endStatusBarText; // Remains in VM
		private CommandWord lastPressedOperation = CommandWord.None;
		private bool secondFunctionTrigger;

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
			this.calculatorRemote.CommandExecuted += this.OnCommandExecuted;
			this.calculatorRemote.CommandFailed += this.OnCommandFailed;

			this.OnClearPressed();
		}

		public double YearsNumber => this.calculator.MemoryFields.Get<double>(MemoryFieldNames.YearsNumber).Value;

		public double InterestNumber => this.calculator.MemoryFields.Get<double>(MemoryFieldNames.InterestNumber).Value;

		public double NominalInterestRateNumber => this.calculator.MemoryFields.Get<double>(MemoryFieldNames.NominalInterestRateNumber).Value;

		public double RepaymentRateNumber => this.calculator.MemoryFields.Get<double>(MemoryFieldNames.RepaymentRateNumber).Value;

		public double StartNumber => this.calculator.MemoryFields.Get<double>(MemoryFieldNames.StartNumber).Value;

		public double RateNumber => this.calculator.MemoryFields.Get<double>(MemoryFieldNames.RateNumber).Value;

		public double EndNumber => this.calculator.MemoryFields.Get<double>(MemoryFieldNames.EndNumber).Value;

		// TODO: RMOVE PROPERTY
		public CommandWord LastPressedOperation
		{
			get => this.lastPressedOperation;
			set
			{
				this.lastPressedOperation = value;
				this.calculator.LastCommand = value;
			}
		}

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

		public bool SecondFunctionTrigger
		{
			get => this.secondFunctionTrigger;
			set
			{
				this.secondFunctionTrigger = value;
				this.NotifyOfPropertyChange(() => this.SecondFunctionTrigger);
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

		private void OnKeyboardKeyPressed(MappedKeyEventArgs e)
		{
			if (e.ActiveWindowContent is FinCalcViewModel2 == false)
			{
				return;
			}

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
				this.PressedSpecialFunctions = this.PressedSpecialFunctions.SetAllFlags(false);
				this.calculatorRemote.InvokeCommand(CommandWord.Clear);
			}
			else
			{
				this.ResetSpecialFunctionLabels();
				this.calculatorRemote.InvokeCommand(CommandWord.Clear, new List<string>() { MemoryFieldNames.Categories.Standard });
			}
		}

		private void OnYearsPressed(bool isLongTouch = false)
		{
			this.ResetSpecialFunctionLabels();
			this.YearsStatusBarText = Resources.FinCalcFunctionYears;

			if (this.SecondFunctionTrigger)
			{
				this.OnYearsSecondFunctionPressed(isLongTouch);
				return;
			}

			if (isLongTouch)
			{
				this.calculatorRemote.InvokeCommand(CommandWord.GetYears);
			}
			else
			{
				if ((this.PressedSpecialFunctions.IsOnlyFlagNotSet(PressedSpecialFunctions.Years) && this.LastPressedOperation.IsSpecialCommandWord())
					|| (this.LastPressedOperation == CommandWord.SetYears
					    || this.LastPressedOperation == CommandWord.GetYears
					    || this.LastPressedOperation == CommandWord.CalculateYears))
				{
					this.calculatorRemote.InvokeCommand(CommandWord.CalculateYears);
				}
				else
				{
					this.calculatorRemote.InvokeCommand(CommandWord.SetYears);
				}
			}

			this.PressedSpecialFunctions = this.PressedSpecialFunctions.SetFlag(PressedSpecialFunctions.Years, true);
			this.SecondFunctionTrigger = false;
		}

		private void OnYearsSecondFunctionPressed(bool isLongTouch)
		{
			if (isLongTouch)
			{
				this.calculatorRemote.InvokeCommand(CommandWord.GetRatesPerAnnum);
			}
			else
			{
				this.calculatorRemote.InvokeCommand(CommandWord.SetRatesPerAnnum);
			}

			this.SecondFunctionTrigger = false;
		}

		private void OnInterestPressed(bool isLongTouch)
		{
			// Prepare
			this.ResetSpecialFunctionLabels();
			this.InterestStatusBarText = Resources.FinCalcFunctionInterest;

			// Special - if the last pressed operation was a special function this current special function should not work with old values.
			if (!isLongTouch && this.IsCommandWordSpecialFunction())
			{
				this.ResetSides();
				this.calculator.MemoryFields.Reset(new List<string>() { MemoryFieldNames.Categories.Standard });
			}

			// Check if it is a second function call
			if (this.SecondFunctionTrigger)
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
				if ((this.PressedSpecialFunctions.IsOnlyFlagNotSet(PressedSpecialFunctions.Interest) && this.IsCommandWordSpecialFunction())
					|| this.LastPressedOperation == CommandWord.Interest)
				{
					var tmpNominalInterestNumber = this.CalculateAndCheckResult(true, new Func<double, double, double, double, double, bool, int, double>(FinancialCalculator.P), (-1) * this.calculator.MemoryFields.Get<double>(MemoryFieldNames.EndNumber).Value, this.calculator.MemoryFields.Get<double>(MemoryFieldNames.StartNumber).Value, this.calculator.MemoryFields.Get<double>(MemoryFieldNames.RateNumber).Value, this.calculator.MemoryFields.Get<double>(MemoryFieldNames.YearsNumber).Value, this.calculator.MemoryFields.Get<int>(MemoryFieldNames.RatesPerAnnumNumber).Value, this.calculator.MemoryFields.Get<bool>(MemoryFieldNames.IsAdvance).Value, 50);

					if (this.IsNumber(tmpNominalInterestNumber))
					{
						this.calculator.MemoryFields.Get<double>(MemoryFieldNames.NominalInterestRateNumber).Value = tmpNominalInterestNumber;
						var tmpInterestNumber = this.CalculateAndCheckResult(false, new Func<double, double, double>(FinancialCalculator.GetEffectiveInterestRate), tmpNominalInterestNumber, this.calculator.MemoryFields.Get<int>(MemoryFieldNames.RatesPerAnnumNumber).Value);
						this.BuildSidesFromNumber(tmpInterestNumber);
						this.CommonSpecialFunctionWriteToMemoryOperations(out var tmpVar, 3);
						this.calculator.MemoryFields.Get<double>(MemoryFieldNames.InterestNumber).Value = tmpVar;
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
						this.calculator.MemoryFields.Reset(new List<string>() { MemoryFieldNames.Categories.Standard });
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
			this.LastPressedOperation = CommandWord.Interest;
			this.calculatorRemote.AddCommandToJournal(CommandWord.Interest);
			this.SecondFunctionTrigger = false;
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
					this.calculator.MemoryFields.Reset(new List<string>() { MemoryFieldNames.Categories.Standard });
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
			this.LastPressedOperation = CommandWord.Interest;
			this.calculatorRemote.AddCommandToJournal(CommandWord.Interest);
			this.SecondFunctionTrigger = false;
		}

		private void OnStartPressed(bool isLongTouch = false)
		{
			this.ResetSpecialFunctionLabels();

			// Check if it is a second function call
			if (this.SecondFunctionTrigger)
			{
				this.OnStartSecondFunctionPressed();
				return;
			}

			this.StartStatusBarText = Resources.FinCalcFunctionStart;

			// GetStart
			if (isLongTouch)
			{
				this.calculatorRemote.InvokeCommand(CommandWord.GetStart);
			}
			else
			{
				// CalculateStart
				if ((this.PressedSpecialFunctions.IsOnlyFlagNotSet(PressedSpecialFunctions.Start) && this.IsCommandWordSpecialFunction())
					|| (this.LastPressedOperation == CommandWord.GetStart
						|| this.LastPressedOperation == CommandWord.SetStart
						|| this.lastPressedOperation == CommandWord.CalculateStart
						|| this.LastPressedOperation == CommandWord.SetAdvance))
				{
					this.calculatorRemote.InvokeCommand(CommandWord.CalculateStart);
				}

				// SetStart
				else
				{
					// Special - if the last pressed operation was a special function this current special function should not work with old values.
					if (!isLongTouch && this.IsCommandWordSpecialFunction())
					{
						this.ResetSides();
						this.calculator.MemoryFields.Reset(new List<string>() { MemoryFieldNames.Categories.Standard });
					}

					this.CommonSpecialFunctionWriteToMemoryOperations(out var tmpVar, 2);
					this.calculator.MemoryFields.Get<double>(MemoryFieldNames.StartNumber).Value = tmpVar;

					this.LastPressedOperation = CommandWord.SetStart;
					this.calculatorRemote.AddCommandToJournal(CommandWord.SetStart);
				}
			}

			this.PressedSpecialFunctions = this.PressedSpecialFunctions.SetFlag(PressedSpecialFunctions.Start, true);
			this.SecondFunctionTrigger = false;
		}

		private void OnStartSecondFunctionPressed()
		{
			// SetAdvance
			// Special - if the last pressed operation was a special function this current special function should not work with old values.
			if (this.IsCommandWordSpecialFunction())
			{
				this.ResetSides();
				this.calculator.MemoryFields.Reset(new List<string>() { MemoryFieldNames.Categories.Standard });
			}

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
			this.LastPressedOperation = CommandWord.SetAdvance;
			this.calculatorRemote.AddCommandToJournal(CommandWord.SetAdvance);
			this.SecondFunctionTrigger = false;
		}

		private void OnRatePressed(bool isLongTouch = false)
		{
			this.ResetSpecialFunctionLabels();
			this.RateStatusBarText = Resources.FinCalcFunctionRate;

			// Check if it is a second function call
			if (this.SecondFunctionTrigger)
			{
				this.OnRateSecondFunctionPressed(isLongTouch);
				return;
			}

			// GetRate
			if (isLongTouch)
			{
				this.calculatorRemote.InvokeCommand(CommandWord.GetRate);
			}
			else
			{
				// CalculateRate
				if ((this.PressedSpecialFunctions.IsOnlyFlagNotSet(PressedSpecialFunctions.Rate) && this.IsCommandWordSpecialFunction())
					|| (this.LastPressedOperation == CommandWord.GetRate
					    || this.LastPressedOperation == CommandWord.SetRate
					    || this.LastPressedOperation == CommandWord.CalculateRate
					    || this.LastPressedOperation == CommandWord.GetRepaymentRate
					    || this.lastPressedOperation == CommandWord.SetRepaymentRate))
				{
					this.calculatorRemote.InvokeCommand(CommandWord.CalculateRate);
				}

				// SetRate [+ repaymentRateNumber]
				else
				{
					this.calculatorRemote.InvokeCommand(CommandWord.SetRate);
				}
			}

			this.PressedSpecialFunctions = this.PressedSpecialFunctions.SetFlag(PressedSpecialFunctions.Rate, true);
			this.SecondFunctionTrigger = false;
		}

		private void OnRateSecondFunctionPressed(bool isLongTouch)
		{
			// GetRepaymentRate
			if (isLongTouch)
			{
				this.calculatorRemote.InvokeCommand(CommandWord.GetRepaymentRate);
			}

			// SetRepaymentRate
			else
			{
				this.calculatorRemote.InvokeCommand(CommandWord.SetRepaymentRate);
			}

			this.PressedSpecialFunctions = this.PressedSpecialFunctions.SetFlag(PressedSpecialFunctions.Rate, true);
			this.SecondFunctionTrigger = false;
		}

		private void OnEndPressed(bool isLongTouch = false)
		{
			this.ResetSpecialFunctionLabels();
			this.EndStatusBarText = Resources.FinCalcFunctionEnd;

			if (isLongTouch)
			{
				this.calculatorRemote.InvokeCommand(CommandWord.GetEnd);
			}
			else
			{
				// Percentage calculation <-- On the physical calculator it is marked a second function but is triggered as standard function
				if ((this.LastPressedOperation == CommandWord.Digit
					 || this.LastPressedOperation == CommandWord.AlgebSign
					 || this.LastPressedOperation == CommandWord.DecimalSeparator)
					&& this.ActiveMathOperator != MathOperator.None)
				{
					this.calculatorRemote.InvokeCommand(CommandWord.PercentCalculation);
					return;
				}

				// Write the value to the memory
				else if ((this.PressedSpecialFunctions.IsOnlyFlagNotSet(PressedSpecialFunctions.End) && this.IsCommandWordSpecialFunction())
					|| (this.LastPressedOperation == CommandWord.GetEnd
					    || this.lastPressedOperation == CommandWord.SetEnd
					    || this.lastPressedOperation == CommandWord.CalculateEnd))
				{
					this.calculatorRemote.InvokeCommand(CommandWord.CalculateEnd);
				}
				else
				{
					this.calculatorRemote.InvokeCommand(CommandWord.SetEnd);
				}
			}

			this.PressedSpecialFunctions = this.PressedSpecialFunctions.SetFlag(PressedSpecialFunctions.End, true);
			this.SecondFunctionTrigger = false;
		}

		private void OnAlgebSignPressed()
		{
			this.ResetSpecialFunctionLabels();
			this.calculatorRemote.InvokeCommand(CommandWord.AlgebSign);
		}

		private void OnDigitPressed(object digitObj)
		{
			this.ResetSpecialFunctionLabels();
			this.calculatorRemote.InvokeCommand(CommandWord.Digit, digitObj);
		}

		private void OnOperatorPressed(object mathOperatorObj)
		{
			this.ResetSpecialFunctionLabels();
			this.calculatorRemote.InvokeCommand(CommandWord.Operator, mathOperatorObj);

			if ((string)mathOperatorObj == "*")
			{
				this.SecondFunctionTrigger = true;
			}
        }

		private void OnDecimalSeparatorPressed()
		{
			this.ResetSpecialFunctionLabels();
			this.calculatorRemote.InvokeCommand(CommandWord.DecimalSeparator);
		}

		private void OnCalculatePressed()
		{
			this.ResetSpecialFunctionLabels();
			this.calculatorRemote.InvokeCommand(CommandWord.Calculate);
		}

		private void SetNumber(out double number)
		{
			var realRightSide = string.IsNullOrEmpty(this.calculator.InputText.FractionalNumberPart) ? "0" : this.calculator.InputText.FractionalNumberPart;
			var numberString = this.calculator.InputText.WholeNumberPart + Resources.CALC_DECIMAL_SEPARATOR + realRightSide;
			if (!double.TryParse(numberString, out number))
			{
				this.eventAggregator.PublishOnUIThread(new ErrorEvent(new ArgumentException(Resources.EXC_PARSE_DOUBLE_IMPOSSIBLE), Resources.EXC_ARGUMENT_INVALID + " " + numberString));
			}

			this.ResetSides();
		}

		private void SetDisplayNumber()
		{
			var realRightSide = string.IsNullOrEmpty(this.calculator.InputText.FractionalNumberPart) ? "0" : this.calculator.InputText.FractionalNumberPart;
			var concatenatedSides = this.calculator.InputText.WholeNumberPart + Resources.CALC_DECIMAL_SEPARATOR + realRightSide;
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

		private void SetDisplayText(bool isSpecialFunctionNumber = false, int specialNumberDecimalCount = 2)
		{
			var displayLeftSide = this.InsertThousandSeparator(this.calculator.InputText.WholeNumberPart);
			var displayRightSide = this.calculator.InputText.FractionalNumberPart;
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
		}

		private void BuildSidesFromNumber(double number)
		{
			var roundedResult = Math.Round(number, 9);
			var s = roundedResult.ToString(CultureInfo.InvariantCulture);
			var parts = s.Split('.');
			this.calculator.InputText.WholeNumberPart = parts[0];
			this.calculator.InputText.FractionalNumberPart = parts.Length < 2 ? string.Empty : parts[1];
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
			if (this.LastPressedOperation == CommandWord.Operator)
			{
				this.BuildSidesFromNumber(this.calculator.MemoryFields.Get<double>(MemoryFieldNames.PreOperatorNumber).Value);
			}

			this.SetNumber(out numberToSet);
			this.calculator.MemoryFields.Reset(new List<string>() { MemoryFieldNames.Categories.Standard });
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
			this.calculator.MemoryFields.Reset(new List<string>() { MemoryFieldNames.Categories.Standard });
			this.calculator.MemoryFields.Get<double>(MemoryFieldNames.PreOperatorNumber).Value = fistNumberSubstitution;
			this.BuildSidesFromNumber(fistNumberSubstitution);
			this.ActiveMathOperator = MathOperator.None;
			this.SetDisplayText(true, specialNumberDecimalCount);
		}

		private bool IsCommandWordSpecialFunction()
		{
			return this.LastPressedOperation.IsSpecialCommandWord();
		}

		// TODO: REMOVE OR REPLACE
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
				this.SecondFunctionTrigger = false;
				if (notifyIfResultIsNotValid)
				{
					this.eventAggregator.PublishOnUIThread(new ErrorEvent(ex, Resources.EXC_CALC_NOT_POSSIBLE));
					this.OnClearPressed();
				}
			}
			catch (NotFiniteNumberException ex)
			{
				this.SecondFunctionTrigger = false;
				if (notifyIfResultIsNotValid)
				{
					this.eventAggregator.PublishOnUIThread(new ErrorEvent(ex, Resources.EXC_NOT_FINITE_NUMBER));
					this.OnClearPressed();
				}
			}
			catch (DivideByZeroException ex)
			{
				this.SecondFunctionTrigger = false;
				if (notifyIfResultIsNotValid)
				{
					this.eventAggregator.PublishOnUIThread(new ErrorEvent(ex, string.Format(CultureInfo.InvariantCulture, Resources.EXC_DIVISION_BY_ZERO)));
					this.OnClearPressed();
				}
			}
			catch (OverflowException ex)
			{
				this.SecondFunctionTrigger = false;
				if (notifyIfResultIsNotValid)
				{
					this.eventAggregator.PublishOnUIThread(new ErrorEvent(ex, Resources.EXC_OVERFLOW_EXCEPTION));
					this.OnClearPressed();
				}
			}
			catch (NotSupportedException)
			{
				this.SecondFunctionTrigger = false;
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
			this.calculator.InputText.FractionalNumberPart = string.Empty;
			this.calculator.InputText.WholeNumberPart = "0";
			this.calculator.InputText.IsDecimalSeparatorActive = false;
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

		// TODO: REMOVE?
		private void OnOutputTextChanged(object sender, OutputTextChangedEventArgs e)
		{
			this.DisplayText = e.NewText;
			this.DisplayNumber = double.TryParse(this.calculator.InputText.GetEvaluatedResult(), out var value) ? value : double.NaN;
		}

		private void OnCommandFailed(object sender, Exception ex)
		{
			switch (ex)
			{
				case CalculationException _:
					this.eventAggregator.PublishOnUIThread(new ErrorEvent(ex, ex.Message));
					break;
				case NotFiniteNumberException _:
					this.eventAggregator.PublishOnUIThread(new ErrorEvent(ex, Resources.EXC_NOT_FINITE_NUMBER));
					break;
				case DivideByZeroException _:
					this.eventAggregator.PublishOnUIThread(new ErrorEvent(ex, string.Format(CultureInfo.InvariantCulture, Resources.EXC_DIVISION_BY_ZERO)));
					break;
				case OverflowException _:
					this.eventAggregator.PublishOnUIThread(new ErrorEvent(ex, Resources.EXC_OVERFLOW_EXCEPTION));
					break;
				case NotSupportedException _:
					this.eventAggregator.PublishOnUIThread(new ErrorEvent(Resources.EXC_OPERATION_NOT_SUPPORTED));
					break;
				case ValidationException _:
					this.eventAggregator.PublishOnUIThread(new ErrorEvent(ex, ex.Message));
					break;
				default:
					this.eventAggregator.PublishOnUIThread(new ErrorEvent(ex, ex.Message));
					break;
			}

			this.calculatorRemote.InvokeCommand(CommandWord.Clear, new List<string>() { MemoryFieldNames.Categories.Standard });
		}

		private void OnCommandExecuted(object sender, CommandWord e)
		{
			// TODO REMOVE
			this.LastPressedOperation = e;

			this.SecondFunctionTrigger = false;
		}
	}
}
