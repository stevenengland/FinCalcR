using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Caliburn.Micro;
using MaterialDesignThemes.Wpf;
using MediatR;
using Moq;
using Moq.AutoMock;
using StEn.FinCalcR.Calculations.Calculator;
using StEn.FinCalcR.Calculations.Calculator.Commands;
using StEn.FinCalcR.Calculations.Calculator.Display;
using StEn.FinCalcR.Common.Services.Localization;
using StEn.FinCalcR.WinUi.LibraryMapper.DialogHost;
using StEn.FinCalcR.WinUi.Services;
using StEn.FinCalcR.WinUi.ViewModels;

namespace FinCalcR.WinUi.Tests.Mocks
{
    public static class MockFactories
    {
        public static ShellViewModel ShellViewModelMock(out AutoMocker mocker)
        {
            mocker = new AutoMocker();
            mocker.Use(GetCalculationCommandReceiver());
            return mocker.CreateInstance<ShellViewModel>();
        }

        public static AboutViewModel AboutViewModelFactory(out AutoMocker mocker)
        {
            mocker = new AutoMocker();
            return mocker.CreateInstance<AboutViewModel>();
        }

        public static FinCalcViewModel FinCalcViewModelWithCalculatorImplementationFactory(out AutoMocker mocker)
        {
            mocker = new AutoMocker();
            SetImplementationOfCalculatorCommandObjects(out var receiver, out var invoker);
            mocker.Use(receiver);
            mocker.Use(invoker);
            return mocker.CreateInstance<FinCalcViewModel>();
        }

        public static FinCalcViewModel FinCalcViewModelWithCalculatorImplementationFactory(Dictionary<string, object> mockObjects)
        {
            SetImplementationOfCalculatorCommandObjects(out var receiver, out var invoker);
            return new FinCalcViewModel(
                (ILocalizationService)mockObjects[nameof(ILocalizationService)],
                (IMediator)mockObjects[nameof(IMediator)],
                invoker,
                receiver,
                (ISubscriptionService)mockObjects[nameof(ISubscriptionService)]);
        }

        public static Dictionary<string, object> GetMockObjects() => new Dictionary<string, object>()
            {
                { nameof(IMediator), GetMediator() },
                { nameof(ISnackbarMessageQueue), GetISnackbarMessageQueue() },
                { nameof(IDialogHostMapper), GetIDialogHostMapper() },
                { nameof(ILocalizationService), GetLocalizationService() },
                { nameof(IWindowManager), GetWindowManager() },
                { nameof(ICalculationCommandReceiver), GetCalculationCommandReceiver() },
                { nameof(ICommandInvoker), GetCommandInvoker() },
                { nameof(ISubscriptionService), GetSubscriptionService() },
            };

        private static ISubscriptionService GetSubscriptionService() => new Mock<ISubscriptionService>().Object;

        private static ILocalizationService GetLocalizationService() => new Mock<ILocalizationService>().Object;

        private static IMediator GetMediator() => new Mock<IMediator>().Object;

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
