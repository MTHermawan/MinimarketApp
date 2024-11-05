using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace MinimarketApp.Utilities
{
    public class ComboBoxUtils
    {
        public static bool GetProtectText(DependencyObject obj)
        {
            return (bool)obj.GetValue(ProtectTextProperty);
        }

        public static void SetProtectText(DependencyObject obj, bool value)
        {
            obj.SetValue(ProtectTextProperty, value);
        }

        // Using a DependencyProperty as the backing store for ProtectText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ProtectTextProperty =
            DependencyProperty.RegisterAttached("ProtectText", typeof(bool), typeof(ComboBoxUtils), new FrameworkPropertyMetadata(OnProtectTextChanged));

        private static void OnProtectTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var box = d as ComboBox;
            if (box == null) return;
            if (true.Equals(e.NewValue))
            {
                box.SelectionChanged += OnSelectionChanged;
            }
            else
            {
                box.SelectionChanged -= OnSelectionChanged;
            }
        }

        private static void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var box = sender as ComboBox;
            if (box == null) return;
            if (box.SelectedItem == null)
            {
                var text = box.Text;
                Action setText = () => { box.Text = text; };
                Dispatcher.CurrentDispatcher.BeginInvoke(setText);
            }
        }
    }
}