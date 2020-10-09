using System;
using System.Threading.Tasks;
using System.Windows;
using Caliburn.Micro;
using FinCalcR.WinUi.Tests.Mocks;
using MaterialDesignThemes.Wpf;
using Moq;
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
		public void VmIsSubscribedToEventAggregator()
		{
			var mockObjects = MockFactories.GetMockObjects();
			var eventAggregatorMock = Mock.Get((IEventAggregator)mockObjects[nameof(IEventAggregator)]);

			var vm = MockFactories.ShellViewModelFactory(mockObjects);

			// Test to make sure subscribe was called on the event aggregator at least once
			eventAggregatorMock.Verify(x => x.Subscribe(vm), Times.Once);
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
