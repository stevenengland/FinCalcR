﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Windows;
using Caliburn.Micro;
using MaterialDesignThemes.Wpf;
using StEn.FinCalcR.Common.LanguageResources;
using StEn.FinCalcR.Common.Services.Localization;
using StEn.FinCalcR.WinUi.Events;
using StEn.FinCalcR.WinUi.LibraryMapper.DialogHost;
using StEn.FinCalcR.WinUi.LibraryMapper.WpfLocalizeExtension;
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
				this.SetErrorEvent(new ErrorEvent(e, $"Could not set Language {nameof(this.LocalizationService)}:" + e.Message, true));
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

		protected override void OnStartup(object sender, StartupEventArgs e)
		{
			var proc = Process.GetCurrentProcess();
			var count = Process.GetProcesses().Count(p => p.ProcessName == proc.ProcessName);

			if (count > 1)
			{
				Environment.Exit(1);
			}

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
