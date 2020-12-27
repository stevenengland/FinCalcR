using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Caliburn.Micro;
using FinCalcR.WinUi.Tests.Mocks;
using FluentAssertions;
using MaterialDesignThemes.Wpf;
using Moq;
using StEn.FinCalcR.Common.LanguageResources;
using StEn.FinCalcR.Common.Services.Localization;
using StEn.FinCalcR.WinUi.Events;
using StEn.FinCalcR.WinUi.LibraryMapper.DialogHost;
using StEn.FinCalcR.WinUi.Models;
using Xunit;

// ReSharper disable RedundantArgumentDefaultValue
namespace FinCalcR.WinUi.Tests.ViewModelTests
{
    public class ShellViewModelTests
    {
        [Fact]
        public void ActiveContentIsSetAtStartup()
        {
            // Arrange
            var mockObjects = MockFactories.GetMockObjects();
            var vm = MockFactories.ShellViewModelFactory(mockObjects);

            // Act

            // Assert
            vm.ActiveWindowContent.Should().Be(vm.MenuItems.First().Content);
        }

        [Fact]
        public void TitleTextIsSetAtStartup()
        {
            var mockObjects = MockFactories.GetMockObjects();
            var vm = MockFactories.ShellViewModelFactory(mockObjects);
            Assert.True(vm.TitleBarText == Resources.AppTitleTxt_Text + " - " + Resources.FinCalcItem_Name);
        }

        [Fact]
        public void VmIsSubscribedToEventAggregator()
        {
            var mockObjects = MockFactories.GetMockObjects();
            var eventAggregatorMock = Mock.Get((IEventAggregator)mockObjects[nameof(IEventAggregator)]);

            var vm = MockFactories.ShellViewModelFactory(mockObjects);

            // Test to make sure subscribe was called on the event aggregator at least once
            eventAggregatorMock.Verify(x => x.Subscribe(vm), Times.Once);
        }

        [Fact]
        public void ActiveContentChangesWhenSelectedMenuItemChanges()
        {
            // Arrange
            var mockObjects = MockFactories.GetMockObjects();
            var vm = MockFactories.ShellViewModelFactory(mockObjects);

            // Act
            vm.MenuItemsSelectionChangedCommand.Execute(vm.MenuItems.LastOrDefault());

            // Assert
            vm.ActiveWindowContent.Should().NotBe(vm.MenuItems.First().Content);
        }

        [Fact]
        public async Task ErrorEventsAreHandledWithSnackbarAsync()
        {
            var mockObjects = MockFactories.GetMockObjects();

            var snackbarMock = Mock.Get((ISnackbarMessageQueue)mockObjects[nameof(ISnackbarMessageQueue)]);
            var vm = MockFactories.ShellViewModelFactory(mockObjects);

            await vm.Handle(new ErrorEvent(new Exception(), "test", false, true)).ConfigureAwait(true);

            snackbarMock.Verify(x => x.Enqueue(It.IsAny<object>(), It.IsAny<object>(), It.IsAny<Action<ErrorEvent>>(), It.IsAny<ErrorEvent>()), Times.Once);
        }

        [Fact]
        public void SwitchingTheLanguageSucceeds()
        {
            var mockObjects = MockFactories.GetMockObjects();
            var localizationServiceMock = Mock.Get((ILocalizationService)mockObjects[nameof(ILocalizationService)]);
            var windowManagerMock = Mock.Get((IWindowManager)mockObjects[nameof(IWindowManager)]);
            var vm = MockFactories.ShellViewModelFactory(mockObjects);

            vm.LanguageSelectionChangedCommand.Execute(new KeyValuePair<string, string>("de", "de"));

            localizationServiceMock.Verify(x => x.ChangeCurrentCulture(It.IsAny<CultureInfo>()), Times.Once);
            windowManagerMock.Verify(x => x.ShowWindow(It.IsAny<object>(), It.IsAny<object>(), It.IsAny<Dictionary<string, object>>()), Times.Once);
        }

        [Fact]
        public async Task ErrorEventsAreHandledAsync()
        {
            var mockObjects = MockFactories.GetMockObjects();

            var dialogHostMapperMock = Mock.Get((IDialogHostMapper)mockObjects[nameof(IDialogHostMapper)]);
            var vm = MockFactories.ShellViewModelFactory(mockObjects);

            await vm.Handle(new ErrorEvent(new Exception(), "test", false, false)).ConfigureAwait(true);

            dialogHostMapperMock.Verify(x => x.ShowAsync(It.IsAny<object>(), It.IsAny<object>()));
        }

        [Fact]
        public void MenuItemChangesAreHandled()
        {
            var mockObjects = MockFactories.GetMockObjects();

            var vm = MockFactories.ShellViewModelFactory(mockObjects);

            vm.MenuItemsSelectionChangedCommand.Execute(new NavigationMenuItem() { Name = string.Empty });

            Assert.False(vm.IsMenuBarVisible);
        }

        [Fact]
        public void WindowGetsMinimized()
        {
            var mockObjects = MockFactories.GetMockObjects();

            var vm = MockFactories.ShellViewModelFactory(mockObjects);

            vm.MinimizeAppCommand.Execute(null);

            Assert.Equal(WindowState.Minimized, vm.CurWindowState);
        }
    }
}
