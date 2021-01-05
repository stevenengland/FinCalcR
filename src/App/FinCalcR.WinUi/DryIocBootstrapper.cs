using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Windows;
using Caliburn.Micro;
using DryIoc;
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
    public class DryIocBootstrapper : BootstrapperBase
    {
        private readonly IContainer unityContainer = new Container();
        private ErrorEvent firstErrorEvent;

        public DryIocBootstrapper()
        {
            this.Initialize();
        }

        protected override void Configure()
        {
            // --- Register container itself ---
            this.unityContainer.RegisterInstance(this.unityContainer);
            this.RegisterLocalizationService();
            this.RegisterSnackbar();
            this.RegisterCalculator();
            this.unityContainer.Register<IWindowManager, WindowManager>(Reuse.Singleton);
            this.unityContainer.Register<IEventAggregator, EventAggregator>(Reuse.Singleton);
            this.unityContainer.Register<IDialogHostMapper, DialogHostMapper>(Reuse.Singleton);
            this.RegisterViewModels();
        }

        protected override void BuildUp(object instance)
        {
            // Resolve service first then inject properties into it.
            this.unityContainer.InjectPropertiesAndFields(instance);
            base.BuildUp(instance);
        }

        protected override void OnExit(object sender, EventArgs e)
        {
            this.unityContainer.Dispose();
            base.OnExit(sender, e);
        }

        protected override object GetInstance(Type service, string key) => string.IsNullOrEmpty(key) ? this.unityContainer.Resolve(service, key) : this.unityContainer.Resolve(service);

        protected override IEnumerable<object> GetAllInstances(Type service) => this.unityContainer.ResolveMany(service);

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            this.DisplayRootViewFor<ShellViewModel>();
            if (this.firstErrorEvent == null)
            {
                return;
            }

            var eventAggregator = this.unityContainer.Resolve<IEventAggregator>();
            eventAggregator.PublishOnUIThread(
                new ErrorEvent(
                    this.firstErrorEvent.Exception,
                    Resources.EXC_GUI_UNHANDLED_EXCEPTION_OCCURED + " " + this.firstErrorEvent.ErrorMessage,
                    this.firstErrorEvent.ApplicationMustShutdown));
        }

        protected override void OnUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            var eventAggregator = this.unityContainer.Resolve<IEventAggregator>();
            if (e == null)
            {
                eventAggregator.PublishOnUIThread(new ErrorEvent(new Exception(), $"{Resources.EXC_GUI_UNHANDLED_EXCEPTION_OCCURED} null", true));
            }
            else
            {
                eventAggregator.PublishOnUIThread(new ErrorEvent(e.Exception, Resources.EXC_GUI_UNHANDLED_EXCEPTION_OCCURED + " " + e.Exception.Message, true));
                e.Handled = true;
            }
        }

        private void RegisterLocalizationService()
        {
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

                this.unityContainer.RegisterInstance(localizationService);
            }
            catch (Exception e)
            {
                this.SetErrorEvent(new ErrorEvent(e, $"Could not set Language with the help of {nameof(ILocalizationService)}:" + e.Message, true));
            }
        }

        private void RegisterSnackbar()
        {
            try
            {
                ISnackbarMessageQueue sbMessageQueue = new SnackbarMessageQueue(TimeSpan.FromSeconds(5));
                this.unityContainer.RegisterInstance(sbMessageQueue);
            }
            catch (Exception e)
            {
                this.SetErrorEvent(
                    new ErrorEvent(e, $"Could not load {nameof(ISnackbarMessageQueue)}:" + e.Message, true));
            }
        }

        private void RegisterCalculator()
        {
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
            this.unityContainer.RegisterInstance(calculator);

            ICommandInvoker calculatorRemote = new CalculatorRemote(new CommandList(commands));
            this.unityContainer.RegisterInstance(calculatorRemote);
        }

        private void RegisterViewModels()
        {
            try
            {
                this.GetType().Assembly.GetTypes()
                    .Where(type => type.IsClass && type.Name.EndsWith("ViewModel"))
                    .ToList()
                    .ForEach(viewModelType => this.unityContainer.Register(viewModelType, viewModelType));
            }
            catch (Exception e)
            {
                this.SetErrorEvent(new ErrorEvent(e, "One or more ViewModels could not be loaded:" + e.Message, true));
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
