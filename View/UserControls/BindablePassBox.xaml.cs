using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace MinimarketApp.View.UserControls
{
    public partial class BindablePassBox : UserControl
    {
        public string Password
        {
            get { return (string)GetValue(PasswordProperty); }
            set
            {
                SetValue(PasswordProperty, value);
            }
        }

        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.Register("Password", typeof(string), typeof(BindablePassBox),
                new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, null, null, false, UpdateSourceTrigger.PropertyChanged));

        public BindablePassBox()
        {
            InitializeComponent();

            SetControlVisibility();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            Password = HiddenPasswordBox.Password;
        }

        private bool _passwordRevealMode;
        public bool PasswordRevealMode
        {
            get { return _passwordRevealMode; }
            set
            {
                _passwordRevealMode = value;
                SetControlVisibility();
                if (value)
                {
                    PasswordBox.Focus();
                    PasswordBox.Text = Password;
                    PasswordBox.CaretIndex = PasswordBox.Text.Length;
                }
                else
                {
                    HiddenPasswordBox.Focus();
                    HiddenPasswordBox.Password = Password;
                    SetPasswordCaretIndex();
                }
            }
        }

        private void SetControlVisibility()
        {
            if (PasswordRevealMode)
            {
                HiddenPasswordBox.Visibility = Visibility.Hidden;
                PasswordBox.Visibility = Visibility.Visible;
                EyeOn.Visibility = Visibility.Hidden;
                EyeOff.Visibility = Visibility.Visible;
            }
            else
            {
                HiddenPasswordBox.Visibility = Visibility.Visible;
                PasswordBox.Visibility = Visibility.Hidden;
                EyeOn.Visibility = Visibility.Visible;
                EyeOff.Visibility = Visibility.Hidden;
            }
        }

        private void PasswordBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Password = PasswordBox.Text;
        }

        private void SetPasswordCaretIndex()
        {
            HiddenPasswordBox.GetType().GetMethod("Select", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(HiddenPasswordBox, new object[] { HiddenPasswordBox.Password.Length, 0 });
        }

        private void BtnSetPassVisibility_Click(object sender, RoutedEventArgs e)
        {
            PasswordRevealMode = !PasswordRevealMode;
        }

        private bool _isFocus;
        public bool IsFocus
        {
            get { return _isFocus; }
            set
            {
                _isFocus = value;
                InputPasswordIcon.Foreground = (value) ? (Brush)new BrushConverter().ConvertFrom("#3F51B5") : (Brush)new BrushConverter().ConvertFrom("#000000");
            }
        }

        private void FocusChanged(object sender, RoutedEventArgs e)
        {
            IsFocus = (HiddenPasswordBox.IsFocused) || (PasswordBox.IsFocused) ? true : false;
        }
    }
}
