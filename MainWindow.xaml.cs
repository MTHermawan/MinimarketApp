using MinimarketApp.Data;
using MinimarketApp.Repository;
using MinimarketApp.Utilities;
using MinimarketApp.View;
using MySql.Data.MySqlClient;
using System.Windows;

namespace MinimarketApp
{
    public partial class MainWindow : Window
    {
        private readonly UserRepository _userRepository = new();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
           // _userRepository.Login(Input_Username.InputText, Input_Password.InputText);
        }
    }
}