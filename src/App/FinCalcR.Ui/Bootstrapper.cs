using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using Caliburn.Micro;
using MaterialDesignThemes.Wpf;
using StEn.FinCalcR.Common.Services.Localization;
using StEn.FinCalcR.Ui.Events;
using StEn.FinCalcR.Ui.LibraryMapper.WpfLocalizeExtension;

namespace StEn.FinCalcR.Ui
{
	public class Bootstrapper : BootstrapperBase
	{
		private readonly SimpleContainer simpleContainer = new SimpleContainer();
		private ErrorEvent firstErrorEvent;

		public Bootstrapper()
		{
			this.Initialize();
		}

		private ISnackbarMessageQueue SbMessageQueue { get; set; }

		private ILocalizationService LocalizationService { get; set; }

		protected override void Configure()
		{

			try

			{
				this.LocalizationService = new WpfLocalizeExtensionMapper("Resources");
				this.LocalizationService.ChangeCurrentCulture(new CultureInfo("de"));
				this.simpleContainer.Instance<ILocalizationService>(this.LocalizationService);
			}
			catch (Exception e)
			{
				this.SetErrorEvent(new ErrorEvent(e,
					$"Could not set Language {nameof(this.LocalizationService)}:" + e.Message, true));
			}

			try
			{
				this.SbMessageQueue = new SnackbarMessageQueue(TimeSpan.FromSeconds(5));
				this.simpleContainer.Instance<ISnackbarMessageQueue>(this.SbMessageQueue);
			}
			catch (Exception e)
			{
				this.SetErrorEvent(
					new ErrorEvent(e, $"Could not load {nameof(this.SbMessageQueue)}:" + e.Message, true));
			}

// Register itself
			this.simpleContainer.Instance(this.simpleContainer);

// .PerRequest
			this.simpleContainer
				.Singleton<IWindowManager, WindowManager>()
				.Singleton<IEventAggregator, EventAggregator>()
				.PerRequest<IDialogHostMapper, DialogHostMapper>();

// Registers every ViewModel to the container
			this.GetType().Assembly.GetTypes()
				.Where(type => type.IsClass)
				.Where(type => type.Name.EndsWith("ViewModel"))
				.ToList()
				.ForEach(viewModelType => this.simpleContainer.RegisterPerRequest(
					viewModelType, viewModelType.ToString(), viewModelType));
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