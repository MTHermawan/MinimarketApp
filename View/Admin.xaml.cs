using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Windows;

namespace MinimarketApp.View
{
    public partial class Admin : Window
    {
        public Admin()
        {
            DataContext = this;
            InitializeComponent();
            WelcomeText.Text = "Selamat Datang, " + Session.Username;
        }
    }
}
