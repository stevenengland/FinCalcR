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
    public class ShellViewModelShould
    {
        [Fact]
        public void SetActiveWindowContent_WhenAppIsStarted()
        {
            // Arrange
            var vm = MockFactories.ShellViewModelMock(out _);

            // Act
            // Assert
            vm.ActiveWindowContent.Should().Be(vm.MenuItems.First().Content);
        }

        [Fact]
        public void SetTitleText_WhenAppIsStarted()
        {
            // Arrange
            var vm = MockFactories.ShellViewModelMock(out _);

            // Act
            // Assert
            vm.TitleBarText.Should().Be(Resources.AppTitleTxt_Text + " - " + Resources.FinCalcItem_Name);
        }

        [Fact]
        public void SubscribeToEventAggregator_WhenAppIsStarted()
        {
            // Arrange
            var vm = MockFactories.ShellViewModelMock(out var mocker);
            var eventAggregatorMock = mocker.GetMock<IEventAggregator>();

            // Act
            // Assert
            // Test to make sure subscribe was called on the event aggregator at least once
            eventAggregatorMock.Verify(x => x.Subscribe(vm), Times.Once);
        }

        [Fact]
        public void ChangeActiveContent_WhenSelectedMenuItemChanges()
        {
            // Arrange
            var vm = MockFactories.ShellViewModelMock(out _);

            // Act
            vm.MenuItemsSelectionChangedCommand.Execute(vm.MenuItems.LastOrDefault());

            // Assert
            vm.ActiveWindowContent.Should().NotBe(vm.MenuItems.First().Content);
        }

        [Fact]
        public async Task HandleErrorEventsWithSnackbarAsync()
        {
            // Arrange
            var vm = MockFactories.ShellViewModelMock(out var mocker);
            var snackbarMock = mocker.GetMock<ISnackbarMessageQueue>();

            // Act
            await vm.Handle(new ErrorEvent(new Exception(), "test", false, true)).ConfigureAwait(true);

            // Assert
            snackbarMock.Verify(x => x.Enqueue(It.IsAny<object>(), It.IsAny<object>(), It.IsAny<Action<ErrorEvent>>(), It.IsAny<ErrorEvent>()), Times.Once);
        }

        [Fact]
        public void SwitchTheLanguage_WhenLanguageSelectionChanges()
        {
            // Arrange
            var vm = MockFactories.ShellViewModelMock(out var mocker);
            var localizationServiceMock = mocker.GetMock<ILocalizationService>();
            var windowManagerMock = mocker.GetMock<IWindowManager>();

            // Act
            vm.LanguageSelectionChangedCommand.Execute(new KeyValuePair<string, string>("de", "de"));

            // Assert
            localizationServiceMock.Verify(x => x.ChangeCurrentCulture(It.IsAny<CultureInfo>()), Times.Once);
            windowManagerMock.Verify(x => x.ShowWindow(It.IsAny<object>(), It.IsAny<object>(), It.IsAny<Dictionary<string, object>>()), Times.Once);
        }

        [Fact]
        public async Task HandleErrorEventsAsync()
        {
            // Arrange
            var vm = MockFactories.ShellViewModelMock(out var mocker);
            var dialogHostMapperMock = mocker.GetMock<IDialogHostMapper>();

            // Act
            await vm.Handle(new ErrorEvent(new Exception(), "test", false, false)).ConfigureAwait(true);

            // Assert
            dialogHostMapperMock.Verify(x => x.ShowAsync(It.IsAny<object>(), It.IsAny<object>()));
        }

        [Fact]
        public void HandleMenuItemChanges()
        {
            // Arrange
            var vm = MockFactories.ShellViewModelMock(out _);

            // Act
            vm.MenuItemsSelectionChangedCommand.Execute(new NavigationMenuItem() { Name = string.Empty });

            // Assert
            vm.IsMenuBarVisible.Should().BeFalse();
        }

        [Fact]
        public void MinimizeWindow()
        {
            // Arrange
            var vm = MockFactories.ShellViewModelMock(out _);

            // Act
            vm.MinimizeAppCommand.Execute(null);

            // Assert
            vm.CurWindowState.Should().Be(WindowState.Minimized);
        }
    }
}
