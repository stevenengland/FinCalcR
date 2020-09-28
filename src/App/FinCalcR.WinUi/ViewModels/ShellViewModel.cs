using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Caliburn.Micro;
using MaterialDesignThemes.Wpf;
using StEn.FinCalcR.Common.LanguageResources;
using StEn.FinCalcR.Common.Services.Localization;
using StEn.FinCalcR.WinUi.Events;
using StEn.FinCalcR.WinUi.LibraryMapper.DialogHost;
using StEn.FinCalcR.WinUi.Models;
using SyncCommand = StEn.FinCalcR.WinUi.Commanding.SyncCommand;

namespace StEn.FinCalcR.WinUi.ViewModels
{
	public class ShellViewModel : Screen, IHandleWithTask<ErrorEvent>, IHandleWithTask<HintEvent>
	{
		private readonly IEventAggregator eventAggregator;
		private readonly IDialogHostMapper dialogHostMapper;
		private readonly AboutViewModel aboutViewModel;
		private readonly FinCalcViewModel finCalcViewModel;
		private ErrorEvent lastErrorEvent;
		private WindowState curWindowState;
		private bool isMenuBarVisible;
		private string titleBarText;

		public ShellViewModel(
			ISnackbarMessageQueue snackbarMessageQueue,
			IEventAggregator eventAggregator,
			IDialogHostMapper dialogHostMapper,
			ILocalizationService localizationService,
			IWindowManager windowManager,
			AboutViewModel aboutViewModel,
			FinCalcViewModel finCalcViewModel)
		{
			this.SbMessageQueue = snackbarMessageQueue;
			this.eventAggregator = eventAggregator;
			this.dialogHostMapper = dialogHostMapper;

			this.aboutViewModel = aboutViewModel;
			this.finCalcViewModel = finCalcViewModel;

			this.eventAggregator?.Subscribe(this);

			this.LoadMenuItems();
		}

		public ICommand MenuItemsSelectionChangedCommand => new Commanding.SyncCommand<object>((arg) => this.OnMenuItemsSelectionChanged(arg));

		public ICommand ExitAppCommand => new SyncCommand(() => this.OnExitApp());

		public ICommand MinimizeAppCommand => new SyncCommand(() => this.OnMinimizeApp());

		public ISnackbarMessageQueue SbMessageQueue { get; private set; }

		public NavigationMenuItem[] MenuItems { get; private set; }

		public WindowState CurWindowState
		{
			get => this.curWindowState;
			set
			{
				this.curWindowState = value;
				this.NotifyOfPropertyChange(() => this.CurWindowState);
			}
		}

		public bool IsMenuBarVisible
		{
			get => this.isMenuBarVisible;
			set
			{
				this.isMenuBarVisible = value;
				this.NotifyOfPropertyChange(() => this.IsMenuBarVisible);
			}
		}

		public string TitleBarText
		{
			get => this.titleBarText;
			set
			{
				this.titleBarText = value;
				this.NotifyOfPropertyChange(() => this.TitleBarText);
			}
		}

		public async Task Handle(HintEvent message)
		{
			if (message == null)
			{
				return;
			}

			await this.dialogHostMapper.Show(
				this.dialogHostMapper.GetHintView(message.Message),
				"RootDialog").ConfigureAwait(true);
		}

		public async Task Handle(ErrorEvent message)
		{
			if (message == null)
			{
				message = new ErrorEvent(new ArgumentException(Resources.EXC_ARGUMENT_NULL + $"{nameof(message)}"), Resources.EXC_ARGUMENT_NULL + $"{nameof(message)}", false);
			}

			this.lastErrorEvent = message;

			if (message.ApplicationMustShutdown)
			{
				await this.dialogHostMapper.Show(
					this.dialogHostMapper.GetErrorView(message.ErrorMessage, message.Exception?.Message, $"{Resources.EXC_ERROR_EVENT_GENERAL_APP_NEEDS_SHUTDOWN}"),
					"RootDialog").ConfigureAwait(true);
				this.GracefulShutdown();
			}
			else
			{
				await this.dialogHostMapper.Show(
					this.dialogHostMapper.GetErrorView(message.ErrorMessage, message.Exception?.Message),
					"RootDialog").ConfigureAwait(true);
			}
		}

#pragma warning disable VSTHRD100 // Avoid async void methods
		protected override async void OnViewLoaded(object view)
#pragma warning restore VSTHRD100 // Avoid async void methods
		{
			base.OnViewLoaded(view);

			// All stuff that uses error handling should come here to make use of the DialogHost, that is only available after the view loaded.
			if (this.lastErrorEvent != null)
			{
				this.SbMessageQueue.Enqueue("Application loaded with errors.");
				if (this.lastErrorEvent.ApplicationMustShutdown)
				{
					await this.dialogHostMapper.Show(
						this.dialogHostMapper.GetErrorView(this.lastErrorEvent.ErrorMessage, this.lastErrorEvent.Exception.Message, $"{Resources.EXC_ERROR_EVENT_GENERAL_APP_NEEDS_SHUTDOWN}"),
						"RootDialog").ConfigureAwait(true);
					this.GracefulShutdown();
				}
			}
		}

		private void LoadMenuItems()
		{
			this.MenuItems = new[]
			{
				new NavigationMenuItem()
				{
					Name = Resources.FinCalcItem_Name,
					Content = this.finCalcViewModel,
					HorizontalScrollBarVisibilityRequirement = ScrollBarVisibility.Auto,
					VerticalScrollBarVisibilityRequirement = ScrollBarVisibility.Auto,
				},
				new NavigationMenuItem()
				{
					Name = Resources.AboutNavigationItem_Name,
					Content = this.aboutViewModel,
					HorizontalScrollBarVisibilityRequirement = ScrollBarVisibility.Auto,
					VerticalScrollBarVisibilityRequirement = ScrollBarVisibility.Auto,
				},
			};
		}

		private void OnMenuItemsSelectionChanged(object item)
		{
			this.IsMenuBarVisible = false;
			var navItem = (NavigationMenuItem)item;
			this.TitleBarText = Resources.AppTitleTxt_Text + " - " + navItem.Name;
		}

		private void OnMinimizeApp()
		{
			this.CurWindowState = WindowState.Minimized;
		}

		private void OnExitApp()
		{
			this.GracefulShutdown();
		}

		private async Task ShowErrorAsync(ErrorEvent errorEvent)
		{
			await this.dialogHostMapper.Show(
				this.dialogHostMapper.GetErrorView(
					errorEvent.ErrorMessage, errorEvent.Exception?.Message),
				"RootDialog").ConfigureAwait(true);
		}

		private void GracefulShutdown()
		{
			this.TryClose();
			System.Windows.Application.Current.Shutdown();
		}
	}
}
