using System.Collections.Generic;
using Caliburn.Micro;
using MaterialDesignThemes.Wpf;
using Moq;
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
					MockFactories.FinCalcViewModelFactory(GetMockObjects())
				);
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

		public static Dictionary<string, object> GetMockObjects()
		{
			return new Dictionary<string, object>()
			{
				{ nameof(IEventAggregator), MockFactories.GetEventAggregator() },
				{ nameof(ISnackbarMessageQueue), MockFactories.GetISnackbarMessageQueue() },
				{ nameof(IDialogHostMapper), MockFactories.GetIDialogHostMapper() },
				{ nameof(ILocalizationService), MockFactories.GetLocalizationService() },
				{ nameof(IWindowManager), MockFactories.GetWindowManager() },
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
	}
}
