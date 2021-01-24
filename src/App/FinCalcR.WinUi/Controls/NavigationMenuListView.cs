using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace StEn.FinCalcR.WinUi.Controls
{
    public class NavigationMenuListView : ListView, ICommandSource
    {
        // Make Command a dependency property so it can use data binding.
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register(
                "Command",
                typeof(ICommand),
                typeof(NavigationMenuListView),
                new PropertyMetadata(
                    null,
                    CommandChanged));

        // The DependencyProperty for the CommandParameter.
        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.Register(
                "CommandParameter",
                typeof(object),
                typeof(NavigationMenuListView));

        public static readonly DependencyProperty CommandTargetProperty =
            DependencyProperty.Register(
                "CommandTarget",
                typeof(IInputElement),
                typeof(NavigationMenuListView));

        private Device lastUsedDeviceBeforeSelectionChanged;

#pragma warning disable S1450 // Private fields only used as local variables in methods should become local variables
        private EventHandler canExecuteChangedHandler;
#pragma warning restore S1450 // Private fields only used as local variables in methods should become local variables

        public NavigationMenuListView()
        {
            this.SelectionChanged += this.OnSelectionChanged;
            this.PreviewKeyDown += this.OnPreviewKeyDown;
            this.PreviewMouseDown += this.OnPreviewMouseDown;
            this.PreviewStylusDown += this.OnPreviewStylusDown;
        }

        private enum Device
        {
            None,
            Stylus,
            Mouse,
            Keyboard,
        }

        public ICommand Command
        {
            get => (ICommand)this.GetValue(CommandProperty);
            set => this.SetValue(CommandProperty, value);
        }

        public object CommandParameter
        {
            get => this.GetValue(CommandParameterProperty);
            set => this.SetValue(CommandParameterProperty, value);
        }

        public IInputElement CommandTarget
        {
            get => (IInputElement)this.GetValue(CommandTargetProperty);
            set => this.SetValue(CommandTargetProperty, value);
        }

        // Command dependency property change callback.
        private static void CommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var lv = (NavigationMenuListView)d;
            lv.HookUpCommand((ICommand)e.OldValue, (ICommand)e.NewValue);
        }

        // Add a new command to the Command Property.
        private void HookUpCommand(ICommand oldCommand, ICommand newCommand)
        {
            // If oldCommand is not null, then we need to remove the handlers.
            if (oldCommand != null)
            {
                this.RemoveCommand(oldCommand);
            }

            this.AddCommand(newCommand);
        }

        // Remove an old command from the Command Property.
        private void RemoveCommand(ICommand oldCommand)
        {
            EventHandler handler = this.CanExecuteChanged;
            oldCommand.CanExecuteChanged -= handler;
        }

        // Add the command.
        private void AddCommand(ICommand newCommand)
        {
            var handler = new EventHandler(this.CanExecuteChanged);
            this.canExecuteChangedHandler = handler;
            if (newCommand != null)
            {
                newCommand.CanExecuteChanged += this.canExecuteChangedHandler;
            }
        }

        private void CanExecuteChanged(object sender, EventArgs e)
        {
            switch (this.Command)
            {
                case null:
                    return;
                case RoutedCommand command:
                    this.IsEnabled = command.CanExecute(this.CommandParameter, this.CommandTarget);
                    break;
                default:
                    this.IsEnabled = this.Command.CanExecute(this.CommandParameter);
                    break;
            }
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Debug.WriteLine("Selection Changed");

            if (this.lastUsedDeviceBeforeSelectionChanged == Device.Mouse)
            {
                // The events occur in this order: MOUSE -> SELECTION CHANGED
                // So Mouse downs need to be registered in Mouse down event and if the selection changes afterwards this previous mouse down on an item should lead to navigation.
                // We can assume it was a navigation item and nothing else because the selection changed.
                this.ExecuteCommand();
            }

            this.lastUsedDeviceBeforeSelectionChanged = Device.None;
        }

        private void OnPreviewStylusDown(object sender, StylusDownEventArgs e)
        {
            Debug.WriteLine("Stylus");

            this.lastUsedDeviceBeforeSelectionChanged = Device.Stylus;
        }

        private void OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Debug.WriteLine("Mouse");

            this.lastUsedDeviceBeforeSelectionChanged = Device.Mouse;
        }

        private void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            Debug.WriteLine($"Key from {e.OriginalSource.GetType()}");
            this.lastUsedDeviceBeforeSelectionChanged = Device.Keyboard;
            if (e.Key != Key.Enter)
            {
                return;
            }

            if (e.OriginalSource is ListViewItem)
            {
                this.ExecuteCommand();
            }
        }

        private void ExecuteCommand()
        {
            switch (this.Command)
            {
                case null:
                    return;
                case RoutedCommand command:
                    command.Execute(this.CommandParameter, this.CommandTarget);
                    break;
                default:
                    this.Command.Execute(this.CommandParameter);
                    break;
            }
        }
    }
}
