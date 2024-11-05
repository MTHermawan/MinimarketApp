using MinimarketApp.View;
using MinimarketApp.View.UserControls;
using MinimarketApp.ViewModel;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;

namespace MinimarketApp
{
    public partial class MainWindow : Window
    {
        public Point justClicked;
        public bool dragRight, dragDown;
        public Popup _popup;

        public MainWindow()
        {
            InitializeComponent();

            this.Title = "MinimarketApp";
        }

        private void btnCloseApp_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        
        /*private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                Application.Current.MainWindow.WindowStyle = WindowStyle.None;
            }
            else
            {
                Application.Current.MainWindow.WindowStyle = WindowStyle.SingleBorderWindow;
                *//*WindowBorder.Padding = new Thickness(0);
                RectangleWindowBorder.Visibility = Visibility.Visible;*//*
            }
        }*/

        protected override void OnClosing(CancelEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
            e.Cancel = true;
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

        private void btnMenu_Click(object sender, RoutedEventArgs e)
        {
            SideBarContainer.Visibility = (SideBarContainer.Visibility) != Visibility.Visible ? Visibility.Visible : Visibility.Collapsed;
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        private void WindowResizeNorth(object sender, MouseButtonEventArgs e)
        {
            var hwndSource = PresentationSource.FromVisual((Visual)sender) as HwndSource;
            SendMessage(hwndSource.Handle, 0x112, (IntPtr)ResizeDirection.Top, IntPtr.Zero);
        }

        private void WindowResizeSouth(object sender, MouseButtonEventArgs e)
        {
            var hwndSource = PresentationSource.FromVisual((Visual)sender) as HwndSource;
            SendMessage(hwndSource.Handle, 0x112, (IntPtr)ResizeDirection.Bottom, IntPtr.Zero);
        }

        private void WindowResizeWest(object sender, MouseButtonEventArgs e)
        {
            var hwndSource = PresentationSource.FromVisual((Visual)sender) as HwndSource;
            SendMessage(hwndSource.Handle, 0x112, (IntPtr)ResizeDirection.Left, IntPtr.Zero);
        }

        private void WindowResizeEast(object sender, MouseButtonEventArgs e)
        {
            var hwndSource = PresentationSource.FromVisual((Visual)sender) as HwndSource;
            SendMessage(hwndSource.Handle, 0x112, (IntPtr)ResizeDirection.Right, IntPtr.Zero);
        }

        private enum ResizeDirection { Left = 61441, Right = 61442, Top = 61443, Bottom = 61446, BottomRight = 61448, }

        private void BorderVertical_OnMouseEnter(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.SizeWE;
        }

        private void BorderHorizontal_OnMouseEnter(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.SizeNS;
        }

        private void BorderAll_OnMouseLeave(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void BorderSouthEast_OnMouseEnter(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.SizeNWSE;
        }

        private void ToggleAccount_Click(object sender, RoutedEventArgs e)
        {
            /*if (_popup == null)
            {
                _popup = new Popup();
                _popup.PlacementTarget = (UIElement)sender;
                _popup.Placement = PlacementMode.Bottom;
                _popup.AllowsTransparency = true;
                _popup.Child = new ToggleButtonAccountMenu();
                _popup.IsOpen = true;
            }
            else
            {
                _popup.IsOpen = !_popup.IsOpen;
            }*/
        }

        /*private void Window_PreviewKeyUp(object sender, KeyEventArgs e)
        {

        }*/

        private void UIElement_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var hwndSource = PresentationSource.FromVisual((Visual)sender) as HwndSource;
            SendMessage(hwndSource.Handle, 0x112, (IntPtr)ResizeDirection.BottomRight, IntPtr.Zero);
        }
    }
}