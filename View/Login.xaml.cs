using MinimarketApp.Repository;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;
using MinimarketApp.ViewModel;

namespace MinimarketApp.View
{
    public partial class Login : UserControl
    {
        public Login()
        {
            DataContext = new LoginVM();

            InitializeComponent();
        }
    }
}
