using MinimarketApp.Model;
using MinimarketApp.Repository;
using MinimarketApp.Utilities;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
using System.Windows;
using System.Windows.Input;

namespace MinimarketApp.ViewModel
{
    public class RiwayatPembelianVM : ViewModelBase
    {
        private ProdukRepository _produkRepository = new();
        private TransaksiPembelianRepository _transaksiPembelianRepository = new();
        private UserRepository _userRepository = new();

        public RiwayatPembelianVM()
        {
            DetailRowCommand = new RelayCommand(DetailRow);
            EditTransaksiCommand = new RelayCommand(EditTransaksi);

            RefreshDatabase();
        }

        private void RefreshDatabase()
        {
            Items = _transaksiPembelianRepository.GetAllTransaksi().DefaultView;

        }

        private DataView items;
        public DataView Items
        {
            get { return items; }
            set
            {
                items = value;
                OnPropertyChanged();
            }
        }

        private DataRowView selectedItem;
        public DataRowView SelectedItem
        {
            get { return selectedItem; }
            set
            {
                selectedItem = value;
                OnPropertyChanged();
            }
        }

        public ICommand DetailRowCommand { get; set; }
        public ICommand EditTransaksiCommand { get; set; }

        public void EditTransaksi(object obj)
        {
            if (SelectedItem != null)
            {
                object mainViewModel = ((App)Application.Current).MainWindow.DataContext;
                ((MainViewModel)mainViewModel).NavigateToTransaksiPembelian(SelectedItem["id_transaksi_pembelian"].ToString());
            }
        }

        private void DetailRow(object obj)
        {
            if (SelectedItem != null)
            {
                string idTransaksiPembelian = SelectedItem["id_transaksi_pembelian"]?.ToString();
                ObservableCollection<TransaksiPembelianModel> DetailRow = _transaksiPembelianRepository.GetDetailById(idTransaksiPembelian);
                string message = "";
                foreach (TransaksiPembelianModel item in DetailRow)
                {
                    message += $"Nama Produk: {item.nama_produk}, Harga Beli: {item.harga_beli}, Kuantitas: {item.kuantitas}, Subtotal: {item.subtotal}\n";
                }

                MessageBox.Show(message, "Detail Transaksi Pembelian", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
