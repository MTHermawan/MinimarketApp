using System.Windows;
using System.Windows.Controls;

namespace MinimarketApp.View.UserControls
{
    public partial class ToggleButtonAccountMenu : UserControl
    {
        public ToggleButtonAccountMenu()
        {
            DataContext = this;
            InitializeComponent();
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            ((App)Application.Current).Logout();
        }
    }
}
