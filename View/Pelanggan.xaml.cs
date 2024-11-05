using MinimarketApp.ViewModel;
using System.Windows.Controls;

namespace MinimarketApp.View
{
    public partial class Pelanggan : UserControl
    {
        public Pelanggan()
        {
            DataContext = new PelangganVM();

            InitializeComponent();
        }
    }
}
