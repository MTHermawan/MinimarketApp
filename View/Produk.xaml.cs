using MinimarketApp.ViewModel;
using System.Windows.Controls;

namespace MinimarketApp.View
{
    public partial class Produk : UserControl
    {
        public Produk()
        {
            DataContext = new ProdukVM();
            
            InitializeComponent();
        }

        private void BtnReset_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            TxtBarcode.Focus();
        }
    }
}
