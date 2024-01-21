using MinimarketApp.Repository;
using System.Windows.Controls;
using System.Windows;

namespace MinimarketApp.View
{
    public partial class Login : UserControl
    {
        private readonly UserRepository _userRepository = new();
        
        public Login()
        {
            InitializeComponent();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(Input_Username.InputText) || string.IsNullOrEmpty(Input_Password.InputText)) return;

            switch (_userRepository.Login(Input_Username.InputText, Input_Password.InputText))
            {
                case "1":
                    Admin admin = new();
                    admin.Show();
                    break;
                case "2":
                    MessageBox.Show("Kasir");
                    // Kasir kasir = new();
                    // kasir.Show();
                    break;
                default:
                    MessageBox.Show("Username atau password salah!");
                    break;
            }
        }
    }
}
