using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Caliburn.Micro;
using JetBrains.Annotations;
using MaterialDesignThemes.Wpf;
using MediatR;
using StEn.FinCalcR.Common.LanguageResources;
using StEn.FinCalcR.Common.Services.Localization;
using StEn.FinCalcR.WinUi.Commanding;
using StEn.FinCalcR.WinUi.Events;
using StEn.FinCalcR.WinUi.Events.EventArgs;
using StEn.FinCalcR.WinUi.Extensions;
using StEn.FinCalcR.WinUi.LibraryMapper.DialogHost;
using StEn.FinCalcR.WinUi.Models;
using StEn.FinCalcR.WinUi.Services;
using SyncCommand = StEn.FinCalcR.WinUi.Commanding.SyncCommand;

namespace StEn.FinCalcR.WinUi.ViewModels
{
    public class ShellViewModel : Screen
    {
        private readonly IMediator mediator;
        private readonly IDialogHostMapper dialogHostMapper;
        private readonly ILocalizationService localizationService;
        private readonly IWindowManager windowManager;
        private readonly ISubscriptionService subscriptionService;
        private readonly AboutViewModel aboutViewModel;
        private readonly FinCalcViewModel finCalcViewModel;
        private WindowState curWindowState;
        private bool isMenuBarVisible;
        private string titleBarText;
        private ObservableCollection<KeyValuePair<string, string>> languages = new ObservableCollection<KeyValuePair<string, string>>();
        private KeyValuePair<string, string> selectedLanguage;
        private ObservableCollection<NavigationMenuItem> menuItems;

        public ShellViewModel(
            ISnackbarMessageQueue snackbarMessageQueue,
            IMediator mediator,
            IDialogHostMapper dialogHostMapper,
            ILocalizationService localizationService,
            IWindowManager windowManager,
            ISubscriptionService subscriptionService,
            AboutViewModel aboutViewModel,
            FinCalcViewModel finCalcViewModel)
        {
            this.SbMessageQueue = snackbarMessageQueue;
            this.mediator = mediator;
            this.dialogHostMapper = dialogHostMapper;
            this.localizationService = localizationService;
            this.windowManager = windowManager;
            this.subscriptionService = subscriptionService;

            this.aboutViewModel = aboutViewModel;
            this.finCalcViewModel = finCalcViewModel;

            this.subscriptionService.Subscribe(this);

            this.LoadMenuItems();
            this.LoadLanguages();
            this.TitleBarText = Resources.AppTitleTxt_Text + " - " + Resources.FinCalcItem_Name;
        }

        public ICommand MenuItemsSelectionChangedCommand => new SyncCommand<object>(this.OnMenuItemsSelectionChanged);

        public ICommand LanguageSelectionChangedCommand => new SyncCommand<object>(this.OnLanguageSelectionChanged);

        public ICommand ExitAppCommand => new SyncCommand(this.OnExitApp);

        public ICommand MinimizeAppCommand => new SyncCommand(this.OnMinimizeApp);

        public ICommand KeyboardKeyPressedCommand => new SyncCommand<MappedKeyEventArgs>(this.OnKeyboardKeyPressed);

        public ISnackbarMessageQueue SbMessageQueue { get; }

        public ObservableCollection<NavigationMenuItem> MenuItems
        {
            get => this.menuItems;
            set
            {
                this.menuItems = value;
                this.NotifyOfPropertyChange(() => this.MenuItems);
            }
        }

        public ObservableCollection<KeyValuePair<string, string>> Languages
        {
            get => this.languages;
            set
            {
                this.languages = value;
                this.NotifyOfPropertyChange(() => this.Languages);
            }
        }

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

        public KeyValuePair<string, string> SelectedLanguage
        {
            get => this.selectedLanguage;
            set
            {
                this.selectedLanguage = value;
                this.NotifyOfPropertyChange(() => this.SelectedLanguage);
            }
        }

        public object ActiveWindowContent { get; set; }

        [UsedImplicitly]
        public void OnKeyboardKeyPressed(object sender, KeyEventArgs e) // For Xaml behavior event
        {
            var eventArgs = new MappedKeyEventArgs(e.Key.ToString(), this.ActiveWindowContent);
            if ((Keyboard.Modifiers & ModifierKeys.Shift) != 0)
            {
                eventArgs.IsShiftPressed = true;
            }

            var mappedEventArgs = eventArgs;
            this.KeyboardKeyPressedCommand.Execute(mappedEventArgs);
        }

        private void OnKeyboardKeyPressed(MappedKeyEventArgs e) => this.mediator.PublishOnUiThread(new KeyboardKeyDownEvent(e));

        private void LoadLanguages()
        {
            this.Languages.Clear();
            this.Languages.Add(new KeyValuePair<string, string>("de", Resources.LANG_GERMAN));
            this.Languages.Add(new KeyValuePair<string, string>("en", Resources.LANG_ENGLISH));
        }

        private void LoadMenuItems()
        {
            this.MenuItems = new ObservableCollection<NavigationMenuItem>()
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
            this.ActiveWindowContent = this.MenuItems.First().Content;
        }

        private void OnMenuItemsSelectionChanged(object item)
        {
            this.IsMenuBarVisible = false;
            var navItem = (NavigationMenuItem)item;
            this.TitleBarText = Resources.AppTitleTxt_Text + " - " + navItem.Name;
            this.ActiveWindowContent = navItem.Content;
        }

        private void OnLanguageSelectionChanged(object item)
        {
            var langItem = (KeyValuePair<string, string>)item;
            var lang = langItem.Key;
            switch (lang)
            {
                case "de":
                    this.localizationService.ChangeCurrentCulture(new CultureInfo("de"));
                    break;
                case "en":
                    this.localizationService.ChangeCurrentCulture(new CultureInfo("en"));
                    break;
                default:
                    break;
            }

            // Open an new instance of vm (avoid using IoC.Get to be testable). The new vm will not take over the old values.
            this.windowManager.ShowWindow(new ShellViewModel(this.SbMessageQueue, this.mediator, this.dialogHostMapper, this.localizationService, this.windowManager, this.subscriptionService, this.aboutViewModel, this.finCalcViewModel), null, null);

            // Close this instance
            // One could check the success in a static bootstrapper void that takes a look at the Application.Current.Windows ("How much windows of type xyz do exist?").
            this.TryClose();
        }

        private void OnMinimizeApp() => this.CurWindowState = WindowState.Minimized;

        private void OnExitApp() => this.GracefulShutdown();

        private void GracefulShutdown()
        {
            this.TryClose();
            this.mediator.PublishOnUiThread(new ApplicationShutdownEvent());
        }
    }
}
