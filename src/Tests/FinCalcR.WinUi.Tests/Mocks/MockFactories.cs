using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Caliburn.Micro;
using MaterialDesignThemes.Wpf;
using Moq;
using StEn.FinCalcR.Calculations.Calculator;
using StEn.FinCalcR.Calculations.Calculator.Commands;
using StEn.FinCalcR.Calculations.Calculator.Display;
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
					AboutViewModelFactory(GetMockObjects()),
					FinCalcViewModelFactory(GetMockObjects()));
		}

		public static AboutViewModel AboutViewModelFactory(Dictionary<string, object> mockObjects)
		{
			return new AboutViewModel();
		}

		public static FinCalcViewModel FinCalcViewModelFactory(Dictionary<string, object> mockObjects)
        {
            return new FinCalcViewModel(
                (ILocalizationService)mockObjects[nameof(ILocalizationService)],
                (IEventAggregator)mockObjects[nameof(IEventAggregator)],
                (ICommandInvoker)mockObjects[nameof(ICommandInvoker)],
                (ICalculationCommandReceiver)mockObjects[nameof(ICalculationCommandReceiver)]);
        }

		public static FinCalcViewModel FinCalcViewModelWithCalculatorImplementationFactory(Dictionary<string, object> mockObjects)
        {
			SetImplementationOfCalculatorCommandObjects(out var receiver, out var invoker);
			return new FinCalcViewModel(
                (ILocalizationService)mockObjects[nameof(ILocalizationService)],
                (IEventAggregator)mockObjects[nameof(IEventAggregator)],
                invoker,
                receiver);
        }

		public static Dictionary<string, object> GetMockObjects()
		{
			return new Dictionary<string, object>()
			{
				{ nameof(IEventAggregator), GetEventAggregator() },
				{ nameof(ISnackbarMessageQueue), GetISnackbarMessageQueue() },
				{ nameof(IDialogHostMapper), GetIDialogHostMapper() },
				{ nameof(ILocalizationService), GetLocalizationService() },
				{ nameof(IWindowManager), GetWindowManager() },
				{ nameof(ICalculationCommandReceiver), GetCalculationCommandReceiver() },
				{ nameof(ICommandInvoker), GetCommandInvoker() },
			};
		}

		private static ILocalizationService GetLocalizationService()
		{
			return new Mock<ILocalizationService>().Object;
		}

		private static IEventAggregator GetEventAggregator()
		{
			return new Mock<IEventAggregator>().Object;
		}

		private static ISnackbarMessageQueue GetISnackbarMessageQueue()
		{
			return new Mock<ISnackbarMessageQueue>().Object;
		}

		private static IDialogHostMapper GetIDialogHostMapper()
		{
			return new Mock<IDialogHostMapper>().Object;
		}

		private static IWindowManager GetWindowManager()
		{
			return new Mock<IWindowManager>().Object;
		}

		private static ICommandInvoker GetCommandInvoker()
        {
			return new Mock<ICommandInvoker>().Object;
        }

		private static ICalculationCommandReceiver GetCalculationCommandReceiver()
        {
			var calculatorMock = new Mock<ICalculationCommandReceiver>();
			calculatorMock.SetupGet(p => p.OutputText).Returns(new Mock<IOutputText>().Object);
			calculatorMock.SetupGet(p => p.InputText).Returns(new Mock<IInputText>().Object);
			return calculatorMock.Object;
        }

		private static void SetImplementationOfCalculatorCommandObjects(out ICalculationCommandReceiver receiver, out ICommandInvoker invoker)
        {
            var commands = new List<ICalculatorCommand>();

            var tmpReceiver = new TwoOperandsCalculator(new SingleNumberOutput(), new SingleNumberInput(9));

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
