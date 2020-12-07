using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Caliburn.Micro;
using MaterialDesignThemes.Wpf;
using Moq;
using StEn.FinCalcR.Calculations.Calculator;
using StEn.FinCalcR.Calculations.Calculator.Display;
using StEn.FinCalcR.Calculations.Commands;
using StEn.FinCalcR.Common.LanguageResources;
using StEn.FinCalcR.Common.Services.Localization;
using StEn.FinCalcR.WinUi.LibraryMapper.DialogHost;
using StEn.FinCalcR.WinUi.ViewModels;

namespace FinCalcR.WinUi.Tests.Mocks
{
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "Tests do not care")]
	public static class MockFactories
	{
		public static ShellViewModel ShellViewModelFactory(Dictionary<string, object> mockObjects)
		{
			return new ShellViewModel(
					(ISnackbarMessageQueue)mockObjects[nameof(ISnackbarMessageQueue)],
					(IEventAggregator)mockObjects[nameof(IEventAggregator)],
					(IDialogHostMapper)mockObjects[nameof(IDialogHostMapper)],
					(ILocalizationService)mockObjects[nameof(ILocalizationService)],
					(IWindowManager)mockObjects[nameof(IWindowManager)],
					MockFactories.AboutViewModelFactory(GetMockObjects()),
					MockFactories.FinCalcViewModelFactory(GetMockObjects()));
		}

		public static AboutViewModel AboutViewModelFactory(Dictionary<string, object> mockObjects)
		{
			return new AboutViewModel();
		}

		public static FinCalcViewModel FinCalcViewModelFactory(Dictionary<string, object> mockObjects)
		{
			return new FinCalcViewModel(
				(ILocalizationService)mockObjects[nameof(ILocalizationService)],
				(IEventAggregator)mockObjects[nameof(IEventAggregator)]);
		}

		public static FinCalcViewModel2 FinCalcViewModel2Factory(Dictionary<string, object> mockObjects)
        {
            return new FinCalcViewModel2(
                (ILocalizationService)mockObjects[nameof(ILocalizationService)],
                (IEventAggregator)mockObjects[nameof(IEventAggregator)],
                (ICommandInvoker)mockObjects[nameof(ICommandInvoker)],
                (ICalculationCommandReceiver)mockObjects[nameof(ICalculationCommandReceiver)]);
        }

		public static FinCalcViewModel2 FinCalcViewModel2WithCalculatorImplementationFactory(Dictionary<string, object> mockObjects)
        {
			SetImplementationOfCalculatorCommandObjects(out var receiver, out var invoker);
			return new FinCalcViewModel2(
                (ILocalizationService)mockObjects[nameof(ILocalizationService)],
                (IEventAggregator)mockObjects[nameof(IEventAggregator)],
                invoker,
                receiver);
        }

		public static Dictionary<string, object> GetMockObjects()
		{
			return new Dictionary<string, object>()
			{
				{ nameof(IEventAggregator), MockFactories.GetEventAggregator() },
				{ nameof(ISnackbarMessageQueue), MockFactories.GetISnackbarMessageQueue() },
				{ nameof(IDialogHostMapper), MockFactories.GetIDialogHostMapper() },
				{ nameof(ILocalizationService), MockFactories.GetLocalizationService() },
				{ nameof(IWindowManager), MockFactories.GetWindowManager() },
				{ nameof(ICalculationCommandReceiver), MockFactories.GetCalculationCommandReceiver() },
				{ nameof(ICommandInvoker), MockFactories.GetCommandInvoker() },
			};
		}

		public static ILocalizationService GetLocalizationService()
		{
			return new Mock<ILocalizationService>().Object;
		}

		public static IEventAggregator GetEventAggregator()
		{
			return new Mock<IEventAggregator>().Object;
		}

		public static ISnackbarMessageQueue GetISnackbarMessageQueue()
		{
			return new Mock<ISnackbarMessageQueue>().Object;
		}

		public static IDialogHostMapper GetIDialogHostMapper()
		{
			return new Mock<IDialogHostMapper>().Object;
		}

		public static IWindowManager GetWindowManager()
		{
			return new Mock<IWindowManager>().Object;
		}

		private static ICommandInvoker GetCommandInvoker()
        {
			return new Mock<ICommandInvoker>().Object;
        }

		private static ICalculationCommandReceiver GetCalculationCommandReceiver()
        {
            return new Mock<ICalculationCommandReceiver>().Object;
        }

		private static void SetImplementationOfCalculatorCommandObjects(out ICalculationCommandReceiver receiver, out ICommandInvoker invoker)
        {
            var commands = new List<ICalculatorCommand>();

            var tmpReceiver = new Calculator(new SingleNumberOutputText(), new SingleNumberInput(Resources.CALC_THOUSANDS_SEPARATOR, Resources.CALC_DECIMAL_SEPARATOR, 9));

            var assemblies = new[]
            {
                Assembly.GetAssembly(typeof(ICalculatorCommand)),
            };

            foreach (var assembly in assemblies)
            {
                assembly.GetTypes()
                    .Where(type => type.BaseType == typeof(BaseCommand))
                    .ToList().ForEach(commandType => commands.Add((ICalculatorCommand)Activator.CreateInstance(commandType, new object[] { tmpReceiver })));
            }

            receiver = tmpReceiver;
            invoker = new CalculatorRemote(new CommandList(commands));
        }
	}
}
