using MinimarketApp.ViewModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace MinimarketApp.View
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            DataContext = new LoginVM();

            InitializeComponent();
            this.Title = "MyCashier";
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
            e.Cancel = true;
        }

        private void btnCloseApp_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void TopBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }

        private void btnMaximizeApp_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Normal)
            {
                WindowState = WindowState.Maximized;
            }
            else
            {
                WindowState = WindowState.Normal;
            }
        }

        private void btnMinimizeApp_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void TopBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                if (WindowState == WindowState.Normal)
                {
                    WindowState = WindowState.Maximized;
                }
                else
                {
                    WindowState = WindowState.Normal;
                }
            }
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            /*if (WindowState == WindowState.Maximized)
            {
                WindowBorder.Padding = new Thickness(5);
                RectangleWindowBorder.Visibility = Visibility.Collapsed;
            }
            else
            {
                WindowBorder.Padding = new Thickness(0);
                RectangleWindowBorder.Visibility = Visibility.Visible;
            }*/
        }

        private void txtBoxUsername_GotFocus(object sender, RoutedEventArgs e)
        {
            InputUsernameIcon.Foreground = (Brush)new BrushConverter().ConvertFrom("#3F51B5");
        }

        private void txtBoxUsername_LostFocus(object sender, RoutedEventArgs e)
        {
            InputUsernameIcon.Foreground = (Brush)new BrushConverter().ConvertFrom("#000000");
        }
    }
}
