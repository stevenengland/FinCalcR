using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Windows;
using Caliburn.Micro;
using MaterialDesignThemes.Wpf;
using StEn.FinCalcR.Calculations.Calculator;
using StEn.FinCalcR.Calculations.Calculator.Commands;
using StEn.FinCalcR.Calculations.Calculator.Display;
using StEn.FinCalcR.Calculations.Messages;
using StEn.FinCalcR.Common.LanguageResources;
using StEn.FinCalcR.Common.Services.Localization;
using StEn.FinCalcR.WinUi.Events;
using StEn.FinCalcR.WinUi.LibraryMapper.DialogHost;
using StEn.FinCalcR.WinUi.LibraryMapper.WpfLocalizeExtension;
using StEn.FinCalcR.WinUi.Messages;
using StEn.FinCalcR.WinUi.ViewModels;

namespace StEn.FinCalcR.WinUi
{
	public class Bootstrapper : BootstrapperBase
	{
		private readonly SimpleContainer simpleContainer = new SimpleContainer();
		private ErrorEvent firstErrorEvent;

		public Bootstrapper()
		{
			this.Initialize();
		}

		protected override void Configure()
		{
			// --- Register Localization ---
			try
			{
				// Configure Message override
				ErrorMessages.Instance = new LocalizedErrorMessages();

				ILocalizationService localizationService = new WpfLocalizeExtensionMapper("Resources");
				if (CultureInfo.CurrentCulture.ToString().StartsWith("en") || CultureInfo.CurrentCulture.ToString().StartsWith("en-"))
				{
					localizationService.ChangeCurrentCulture(new CultureInfo("en"));
				}
				else if (CultureInfo.CurrentCulture.ToString().StartsWith("de") || CultureInfo.CurrentCulture.ToString().StartsWith("de-"))
				{
					localizationService.ChangeCurrentCulture(new CultureInfo("de"));
				}
				else
				{
					localizationService.ChangeCurrentCulture(new CultureInfo("en"));
				}

				this.simpleContainer.Instance(localizationService);
			}
			catch (Exception e)
			{
				this.SetErrorEvent(new ErrorEvent(e, $"Could not set Language with the help of {nameof(ILocalizationService)}:" + e.Message, true));
			}

			// --- Register Snackbar ---
			try
			{
				ISnackbarMessageQueue sbMessageQueue = new SnackbarMessageQueue(TimeSpan.FromSeconds(5));
				this.simpleContainer.Instance(sbMessageQueue);
			}
			catch (Exception e)
			{
				this.SetErrorEvent(
					new ErrorEvent(e, $"Could not load {nameof(ISnackbarMessageQueue)}:" + e.Message, true));
			}

			// --- Register Calculator ---
			var commands = new List<ICalculatorCommand>();
			ICalculationCommandReceiver calculator = new TwoOperandsCalculator(new SingleNumberOutput(), new SingleNumberInput(9));
			var assemblies = new[]
			{
				Assembly.GetAssembly(typeof(ICalculatorCommand)),
			};

			foreach (var assembly in assemblies)
			{
				assembly.GetTypes()
					.Where(type => type.BaseType == typeof(BaseCommand))
					.ToList().ForEach(commandType => commands.Add((ICalculatorCommand)Activator.CreateInstance(commandType, new object[] { calculator })));
			}

			// Register an instance of the calculator helps to preserve internal states when VM is recreated like when the language changes.
			this.simpleContainer.Instance(calculator);

			ICommandInvoker calculatorRemote = new CalculatorRemote(new CommandList(commands));
			this.simpleContainer.Instance(calculatorRemote);

			// --- Register container itself ---
			this.simpleContainer.Instance(this.simpleContainer);

			// --- Register diverse ---
			this.simpleContainer
				.Singleton<IWindowManager, WindowManager>()
				.Singleton<IEventAggregator, EventAggregator>()
				.PerRequest<IDialogHostMapper, DialogHostMapper>();

			// --- View Model Registration ---
			this.GetType().Assembly.GetTypes()
				.Where(type => type.IsClass)
				.Where(type => type.Name.EndsWith("ViewModel"))
				.ToList()
				.ForEach(viewModelType => this.simpleContainer.RegisterPerRequest(
					viewModelType, viewModelType.ToString(), viewModelType));
		}

		protected override void OnStartup(object sender, StartupEventArgs e)
		{
			this.DisplayRootViewFor<ShellViewModel>();
			if (this.firstErrorEvent != null)
			{
				var eventAggregator = (IEventAggregator)this.simpleContainer.GetInstance(typeof(IEventAggregator), null);
				eventAggregator.PublishOnUIThread(
					new ErrorEvent(
						this.firstErrorEvent.Exception,
						Resources.EXC_GUI_UNHANDLED_EXCEPTION_OCCURED + " " + this.firstErrorEvent.ErrorMessage,
						this.firstErrorEvent.ApplicationMustShutdown));
			}
		}

		protected override object GetInstance(Type service, string key)
		{
			return this.simpleContainer.GetInstance(service, key);
		}

		protected override IEnumerable<object> GetAllInstances(Type service)
		{
			return this.simpleContainer.GetAllInstances(service);
		}

		protected override void BuildUp(object instance)
		{
			this.simpleContainer.BuildUp(instance);
        }

		protected override void OnUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
		{
			var eventAggregator = (IEventAggregator)this.simpleContainer.GetInstance(typeof(IEventAggregator), null);
			if (e == null)
			{
				eventAggregator.PublishOnUIThread(new ErrorEvent(new Exception(), Resources.EXC_GUI_UNHANDLED_EXCEPTION_OCCURED + " " + "null", true));
			}
			else
			{
				eventAggregator.PublishOnUIThread(new ErrorEvent(e.Exception, Resources.EXC_GUI_UNHANDLED_EXCEPTION_OCCURED + " " + e.Exception.Message, true));
				e.Handled = true;
			}
		}

		private void SetErrorEvent(ErrorEvent errorEvent)
		{
			// only the youngest should be kept
			if (this.firstErrorEvent == null)
			{
				this.firstErrorEvent = errorEvent;
			}
		}
	}
}
