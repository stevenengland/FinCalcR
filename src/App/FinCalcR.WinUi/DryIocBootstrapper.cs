using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Windows;
using Caliburn.Micro;
using DryIoc;
using MaterialDesignThemes.Wpf;
using MediatR;
using MediatR.Pipeline;
using StEn.FinCalcR.Calculations.Calculator;
using StEn.FinCalcR.Calculations.Calculator.Commands;
using StEn.FinCalcR.Calculations.Calculator.Display;
using StEn.FinCalcR.Calculations.Messages;
using StEn.FinCalcR.Common.LanguageResources;
using StEn.FinCalcR.Common.Services.Localization;
using StEn.FinCalcR.WinUi.Events;
using StEn.FinCalcR.WinUi.Extensions;
using StEn.FinCalcR.WinUi.LibraryMapper.DialogHost;
using StEn.FinCalcR.WinUi.LibraryMapper.WpfLocalizeExtension;
using StEn.FinCalcR.WinUi.Messages;
using StEn.FinCalcR.WinUi.Services;
using StEn.FinCalcR.WinUi.ViewModels;
using PlatformProvider = StEn.FinCalcR.WinUi.Platform.PlatformProvider;

namespace StEn.FinCalcR.WinUi
{
    public class DryIocBootstrapper : BootstrapperBase
    {
        private readonly IContainer iocContainer = new Container();
        private ErrorEvent firstErrorEvent;

        public DryIocBootstrapper()
        {
            this.Initialize();
        }

        protected override void Configure()
        {
            // --- Register container itself ---
            this.iocContainer.RegisterInstance(this.iocContainer);
            this.RegisterLocalizationService();
            this.RegisterSnackbar();
            this.RegisterCalculator();
            this.iocContainer.Register<IWindowManager, WindowManager>(Reuse.Singleton);
            this.iocContainer.Register<IDialogHostMapper, DialogHostMapper>(Reuse.Singleton);
            this.RegisterMediator();
            this.RegisterKeyboardEventDistributionService();
            this.RegisterViewModels();

            PlatformProvider.Current = new Platform.XamlPlatformProvider();

#pragma warning disable S1481 // Unused local variables should be removed
            var registrations = this.iocContainer.GetServiceRegistrations();
#pragma warning restore S1481 // Unused local variables should be removed

            // Finally check the registrations
            var registrationErrors = this.iocContainer.Validate();
            if (registrationErrors.Length > 0)
            {
                throw new InvalidOperationException();
            }
        }

        protected override void BuildUp(object instance)
        {
            // Resolve service first then inject properties into it.
            this.iocContainer.InjectPropertiesAndFields(instance);
            base.BuildUp(instance);
        }

        protected override void OnExit(object sender, EventArgs e)
        {
            this.iocContainer.Dispose();
            base.OnExit(sender, e);
        }

        protected override object GetInstance(Type service, string key) => string.IsNullOrEmpty(key) ? this.iocContainer.Resolve(service, key) : this.iocContainer.Resolve(service);

        protected override IEnumerable<object> GetAllInstances(Type service) => this.iocContainer.ResolveMany(service);

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            this.DisplayRootViewFor<ShellViewModel>();
            if (this.firstErrorEvent == null)
            {
                return;
            }

            var mediator = this.iocContainer.Resolve<IMediator>();
            mediator.PublishOnUiThread(
                new ErrorEvent(
                    this.firstErrorEvent.Exception,
                    Resources.EXC_GUI_UNHANDLED_EXCEPTION_OCCURED + " " + this.firstErrorEvent.ErrorMessage,
                    this.firstErrorEvent.ApplicationMustShutdown));
        }

        protected override void OnUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            var mediator = this.iocContainer.Resolve<IMediator>();
            if (e == null)
            {
                mediator.PublishOnUiThread(new ErrorEvent(new Exception(), $"{Resources.EXC_GUI_UNHANDLED_EXCEPTION_OCCURED} null", true));
            }
            else
            {
                mediator.PublishOnUiThread(new ErrorEvent(e.Exception, Resources.EXC_GUI_UNHANDLED_EXCEPTION_OCCURED + " " + e.Exception.Message, true));
                e.Handled = true;
            }
        }

        private void RegisterMediator()
        {
            try
            {
                var handler = typeof(ShellViewModel).GetAssembly().GetTypes().Where(t => t
                    .GetInterfaces()
                    .Any(i => i.Name.StartsWith("IRequestHandler") || i.Name.StartsWith("INotificationHandler"))); // Not that nice, but works so far

                this.iocContainer.RegisterMany(
                    handler,
                    ifAlreadyRegistered: IfAlreadyRegistered.Keep); // Could affect viewModels that are already registered.

                // Pipeline
                this.iocContainer.Register(typeof(IPipelineBehavior<,>), typeof(RequestPreProcessorBehavior<,>), serviceKey: "RequestPreProcessorBehavior");
                this.iocContainer.Register(typeof(IPipelineBehavior<,>), typeof(RequestPostProcessorBehavior<,>), serviceKey: "RequestPostProcessorBehavior");

                this.iocContainer.Register<IMediator, Mediator>();
                this.iocContainer.RegisterDelegate<ServiceFactory>(r => r.Resolve);
            }
            catch (Exception e)
            {
                this.SetErrorEvent(new ErrorEvent(e, $"Could not register {nameof(IMediator)}:" + e.Message, true));
            }
        }

        private void RegisterKeyboardEventDistributionService()
        {
            try
            {
                this.iocContainer.Register<ISubscriptionAggregator, SubscriptionAggregator>(reuse: Reuse.Singleton);
                this.iocContainer.Register<IKeyboardEventNotifier, KeyboardEventNotifier>(ifAlreadyRegistered: IfAlreadyRegistered.Keep);
            }
            catch (Exception e)
            {
                this.SetErrorEvent(new ErrorEvent(e, $"Could not register {nameof(IKeyboardEventNotifier)}:" + e.Message, true));
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

                this.iocContainer.RegisterInstance(localizationService);
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
                this.iocContainer.RegisterInstance(sbMessageQueue);
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
            this.iocContainer.RegisterInstance(calculator);

            ICommandInvoker calculatorRemote = new CalculatorRemote(new CommandList(commands));
            this.iocContainer.RegisterInstance(calculatorRemote);
        }

        private void RegisterViewModels()
        {
            try
            {
                this.GetType().Assembly.GetTypes()
                    .Where(type => type.IsClass && type.Name.EndsWith("ViewModel"))
                    .ToList()
                    .ForEach(viewModelType => this.iocContainer.Register(viewModelType, viewModelType, ifAlreadyRegistered: IfAlreadyRegistered.Keep));
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
