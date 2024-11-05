using MinimarketApp.ViewModel;
using Org.BouncyCastle.Tls;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace MinimarketApp.View
{
    public partial class Dashboard : UserControl
    {
        public Dashboard()
        {
            DataContext = new DashboardVM();
            InitializeComponent();
        }

        private void cmbTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var viewModel = DataContext as DashboardVM;
            if (dgTable == null) return;
            if (cmbTable.SelectedIndex == 0)
            {
                dgTable.ItemsSource = viewModel.StokHampirHabis;
                dgTable.Columns.Clear();
                dgTable.Columns.Add(new DataGridTextColumn() { Header = "Barcode", Binding = new Binding("barcode") });
                dgTable.Columns.Add(new DataGridTextColumn() { Header = "Nama Produk", Binding = new Binding("nama_produk") });
                dgTable.Columns.Add(new DataGridTextColumn() { Header = "Stok", Binding = new Binding("stok") });
                txtTableKeterangan.Text = dgTable.Items.Count > 0 ? dgTable.Items.Count + " Produk Perlu Diisi Stok" : "Tidak Ada Produk Perlu Diisi Stok";
            }
            else if (cmbTable.SelectedIndex == 1)
            {
                dgTable.ItemsSource = viewModel.ProdukTerlarisSemingguTerakhir;
                dgTable.Columns.Clear();
                dgTable.Columns.Add(new DataGridTextColumn() { Header = "Barcode", Binding = new Binding("barcode") });
                dgTable.Columns.Add(new DataGridTextColumn() { Header = "Nama Produk", Binding = new Binding("nama_produk") });
                dgTable.Columns.Add(new DataGridTextColumn() { Header = "Jumlah Terjual", Binding = new Binding("jumlah_terjual") });
                txtTableKeterangan.Text = "Produk Terlaris Seminggu Terakhir (" + DateTime.Now.AddDays(-7).ToString("dd/MM/yyyy", new CultureInfo("id-ID")) + " - " + DateTime.Now.ToString("dd/MM/yyyy", new CultureInfo("id-ID")) + ")";
            }
        }

        private void dgTable_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            cmbTable_SelectionChanged(null, null);
        }
    }
}
