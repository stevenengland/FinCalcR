using System;
using System.Collections.Generic;
using System.Text;
using Caliburn.Micro;
using Moq;
using StEn.FinCalcR.Common.Services.Localization;
using MaterialDesignThemes.Wpf;
using StEn.FinCalcR.WinUi.

namespace FinCalcR.Ui.Tests.Mocks
{
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "Tests do not care")]
	public class MockFactories
	{

		public static Dictionary<string, object> GetMockObjects()
		{
			return new Dictionary<string, object>()
			{
				{ nameof(IConfigurationCollection), MockFactories.GetConfigurationCollection() },
				{ nameof(IPslLerfConfiguration), MockFactories.GetPslLerfConfiguration() },
				{ nameof(IDxfLerfConfiguration), MockFactories.GetDxfLerfConfiguration() },
				{ nameof(IEventAggregator), MockFactories.GetEventAggregator() },
				{ nameof(ISnackbarMessageQueue), MockFactories.GetISnackbarMessageQueue() },
				{ nameof(IDialogHostMapper), MockFactories.GetIDialogHostMapper() },
				{ nameof(ILoggerManager), MockFactories.GetLoggerManager() },
				{ nameof(ILocalizationService), MockFactories.GetLocalizationService() },
				{ nameof(IFileHandlingService), MockFactories.GetFileHandlingService() },
				{ nameof(IPslLerfHandler), MockFactories.GetPslLerfHandler() },
				{ nameof(IDxfLerfHandler), MockFactories.GetDxfLerfHandler() },
				{ nameof(IWindowManager), MockFactories.GetWindowManager() },
				{ nameof(IGeoDataHandler), MockFactories.GetGeoDataHandler() },
				{ nameof(IMapsUiServiceMapper), MockFactories.GetMapsUiServiceMapper() },
				{ nameof(IRemoteHomeService), MockFactories.GetRemoteHomeService() },
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
	}
}
