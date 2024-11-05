using MinimarketApp.ViewModel;
using System.Windows;
using System.Windows.Controls;

namespace MinimarketApp.View
{
    public partial class NavigationMenu : UserControl
    {
        public NavigationMenu()
        {
            DataContext = new NavigationMenuVM();

            InitializeComponent();
        }
    }
}
