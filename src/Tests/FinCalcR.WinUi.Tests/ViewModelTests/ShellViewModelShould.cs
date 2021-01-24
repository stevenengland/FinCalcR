using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows;
using Caliburn.Micro;
using FinCalcR.WinUi.Tests.Mocks;
using FluentAssertions;
using MediatR;
using Moq;
using StEn.FinCalcR.Common.LanguageResources;
using StEn.FinCalcR.Common.Services.Localization;
using StEn.FinCalcR.WinUi.Events;
using StEn.FinCalcR.WinUi.Models;
using StEn.FinCalcR.WinUi.Services;
using StEn.FinCalcR.WinUi.ViewModels;
using Xunit;

namespace FinCalcR.WinUi.Tests.ViewModelTests
{
    public class ShellViewModelShould
    {
        [Fact]
        public void Subscribe_WhenVmIsCreated()
        {
            // Arrange
            MockFactories.ShellViewModelMock(out var mocker);
            var subscriptionServiceMock = mocker.GetMock<ISubscriptionAggregator>();

            // Act
            // Assert
            subscriptionServiceMock.Verify(m => m.Subscribe(It.Is<object>(o => o.GetType() == typeof(ShellViewModel))), Times.Once);
        }

        [Fact]
        public void SetActiveWindowContentAndNavigationMenuItem_WhenAppIsStarted()
        {
            // Arrange
            var vm = MockFactories.ShellViewModelMock(out _);

            // Act
            // Assert
            vm.ActiveWindowContent.Should().Be(vm.MenuItems.First().Content);
            vm.ActiveNavigationMenuItem.Should().Be(vm.MenuItems.First());
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
        public void ChangeActiveContent_WhenSelectedMenuItemChanges()
        {
            // Arrange
            var vm = MockFactories.ShellViewModelMock(out _);

            // Act
            vm.NavigateCommand.Execute(vm.MenuItems.LastOrDefault());

            // Assert
            vm.ActiveWindowContent.Should().NotBe(vm.MenuItems.First().Content);
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
        public void HandleMenuItemChanges_WhenTheSelectedViewModelChanges()
        {
            // Arrange
            var vm = MockFactories.ShellViewModelMock(out _);

            // Act
            vm.NavigateCommand.Execute(new NavigationMenuItem() { Name = string.Empty });

            // Assert
            vm.IsMenuBarVisible.Should().BeFalse();
        }

        [Fact]
        public void MinimizeWindow_WhenMinimizeButtonIsPressed()
        {
            // Arrange
            var vm = MockFactories.ShellViewModelMock(out _);

            // Act
            vm.MinimizeAppCommand.Execute(null);

            // Assert
            vm.CurWindowState.Should().Be(WindowState.Minimized);
        }

        [Fact]
        public void CloseWindow_WhenClosingButtonIsPressed()
        {
            // Arrange
            var vm = MockFactories.ShellViewModelMock(out var mocker);
            var mediator = mocker.GetMock<IMediator>();

            // Act
            vm.ExitAppCommand.Execute(null);

            // Assert
            mediator.Verify(m => m.Publish(It.Is<INotification>(o => o.GetType() == typeof(ApplicationShutdownEvent)), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
