using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Caliburn.Micro;
using MaterialDesignThemes.Wpf;
using Moq;
using Moq.AutoMock;
using StEn.FinCalcR.Calculations.Calculator;
using StEn.FinCalcR.Calculations.Calculator.Commands;
using StEn.FinCalcR.Calculations.Calculator.Display;
using StEn.FinCalcR.Common.Services.Localization;
using StEn.FinCalcR.WinUi.LibraryMapper.DialogHost;
using StEn.FinCalcR.WinUi.ViewModels;

namespace FinCalcR.WinUi.Tests.Mocks
{
    public static class MockFactories
    {
        public static ShellViewModel ShellViewModelMock(out AutoMocker mocker)
        {
            mocker = new AutoMocker();
            mocker.Use<ICalculationCommandReceiver>(GetCalculationCommandReceiver());
            return mocker.CreateInstance<ShellViewModel>();
        }

        public static AboutViewModel AboutViewModelFactory(out AutoMocker mocker)
        {
            mocker = new AutoMocker();
            return mocker.CreateInstance<AboutViewModel>();
        }

        public static FinCalcViewModel FinCalcViewModelFactory(Dictionary<string, object> mockObjects) => new FinCalcViewModel(
                (ILocalizationService)mockObjects[nameof(ILocalizationService)],
                (IEventAggregator)mockObjects[nameof(IEventAggregator)],
                (ICommandInvoker)mockObjects[nameof(ICommandInvoker)],
                (ICalculationCommandReceiver)mockObjects[nameof(ICalculationCommandReceiver)]);

        public static FinCalcViewModel FinCalcViewModelWithCalculatorImplementationFactory(Dictionary<string, object> mockObjects)
        {
            SetImplementationOfCalculatorCommandObjects(out var receiver, out var invoker);
            return new FinCalcViewModel(
                (ILocalizationService)mockObjects[nameof(ILocalizationService)],
                (IEventAggregator)mockObjects[nameof(IEventAggregator)],
                invoker,
                receiver);
        }

        public static Dictionary<string, object> GetMockObjects() => new Dictionary<string, object>()
            {
                { nameof(IEventAggregator), GetEventAggregator() },
                { nameof(ISnackbarMessageQueue), GetISnackbarMessageQueue() },
                { nameof(IDialogHostMapper), GetIDialogHostMapper() },
                { nameof(ILocalizationService), GetLocalizationService() },
                { nameof(IWindowManager), GetWindowManager() },
                { nameof(ICalculationCommandReceiver), GetCalculationCommandReceiver() },
                { nameof(ICommandInvoker), GetCommandInvoker() },
            };

        private static ILocalizationService GetLocalizationService() => new Mock<ILocalizationService>().Object;

        private static IEventAggregator GetEventAggregator() => new Mock<IEventAggregator>().Object;

        private static ISnackbarMessageQueue GetISnackbarMessageQueue() => new Mock<ISnackbarMessageQueue>().Object;

        private static IDialogHostMapper GetIDialogHostMapper() => new Mock<IDialogHostMapper>().Object;

        private static IWindowManager GetWindowManager() => new Mock<IWindowManager>().Object;

        private static ICommandInvoker GetCommandInvoker() => new Mock<ICommandInvoker>().Object;

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
