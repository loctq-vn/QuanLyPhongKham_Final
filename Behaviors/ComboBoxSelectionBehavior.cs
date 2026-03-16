using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace QuanLyPhongKham_Final.Behaviors
{
    /// <summary>
    /// Attached Behavior để xử lý ComboBox SelectionChanged event
    /// </summary>
    public static class ComboBoxSelectionBehavior
    {
        public static ICommand GetSelectionChangedCommand(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(SelectionChangedCommandProperty);
        }

        public static void SetSelectionChangedCommand(DependencyObject obj, ICommand value)
        {
            obj.SetValue(SelectionChangedCommandProperty, value);
        }

        public static readonly DependencyProperty SelectionChangedCommandProperty =
            DependencyProperty.RegisterAttached(
                "SelectionChangedCommand",
                typeof(ICommand),
                typeof(ComboBoxSelectionBehavior),
                new PropertyMetadata(null, OnSelectionChangedCommandChanged));

        private static void OnSelectionChangedCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ComboBox comboBox)
            {
                if (e.NewValue != null)
                {
                    comboBox.SelectionChanged += ComboBox_SelectionChanged;
                }
                else
                {
                    comboBox.SelectionChanged -= ComboBox_SelectionChanged;
                }
            }
        }

        private static void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox comboBox)
            {
                var command = GetSelectionChangedCommand(comboBox);
                if (command != null && command.CanExecute(comboBox.SelectedValue))
                {
                    command.Execute(comboBox.SelectedValue);
                }
            }
        }
    }
}