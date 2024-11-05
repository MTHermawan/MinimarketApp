using MinimarketApp.Model;
using MinimarketApp.Repository;
using MinimarketApp.Utilities;
using System.Collections.ObjectModel;
using System.Data;
using System.Windows;
using System.Windows.Input;

namespace MinimarketApp.ViewModel
{
    public class RiwayatPenjualanVM : ViewModelBase
    {
        private TransaksiRepository _transaksiRepository = new();
        
        public RiwayatPenjualanVM()
        {
            RefreshDatabase();
            ShowDetailCommand = new RelayCommand(DetailRow);
            EditTransaksiCommand = new RelayCommand(EditTransaksi);

        }

        private void RefreshDatabase()
        {
            Items = _transaksiRepository.GetAllTransaksi().DefaultView;
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

        private TransaksiModel _transaksiData;
        public TransaksiModel TransaksiData
        {
            get { return _transaksiData; }
            set
            {
                _transaksiData = value;
                OnPropertyChanged();
                if (_transaksiData != null)
                {
                    IdTransaksi = _transaksiData.id_transaksi;
                    TanggalTransaksi = _transaksiData.formatted_tanggal_transaksi;
                    Kasir = _transaksiData.username;
                    Pelanggan = _transaksiData.nama_pelanggan;
                }
            }
        }

        private ObservableCollection<TransaksiModel> _detailTransaksiItems;
        public ObservableCollection<TransaksiModel> DetailTransaksiItems
        {
            get { return _detailTransaksiItems; }
            set
            {
                _detailTransaksiItems = value;
                OnPropertyChanged();
            }
        }

        public ICommand ShowDetailCommand { get; set; }
        public ICommand EditTransaksiCommand { get; set; }

        private void DetailRow(object obj)
        {
            /*if (SelectedItem != null)
            {
                string idTransaksi = SelectedItem["id_transaksi"]?.ToString();
                ObservableCollection<TransaksiModel> DetailRow = _transaksiRepository.GetDetailById(idTransaksi);
                string msg = "";
                foreach (var item in DetailRow)
                {
                    msg += $"Nama Produk: {item.nama_produk}, Jumlah: {item.kuantitas}, Harga: {item.harga_jual}, Subtotal: {item.subtotal}\n";
                }
                MessageBox.Show(msg, "Detail Transaksi");
            }*/

            /*if (SelectedItem != null)
            {
                string idTransaksi = SelectedItem.id_transaksi;
                DetailTransaksiItems = _transaksiRepository.GetDetailById(idTransaksi);

                TransaksiData = new TransaksiModel
                {
                    id_transaksi = SelectedItem.id_transaksi,
                    formatted_tanggal_transaksi = SelectedItem.formatted_tanggal_transaksi,
                    id_user = SelectedItem.id_user,
                    username = SelectedItem.username,
                    telp_pelanggan = SelectedItem.telp_pelanggan,
                    nama_pelanggan = SelectedItem.nama_pelanggan,
                    total_harga = float.Parse(SelectedItem.total_harga.ToString()),
                };
            }*/
        }

        public void EditTransaksi(object obj)
        {
            if (SelectedItem != null)
            {
                object mainViewModel = ((App)Application.Current).MainWindow.DataContext;
                ((MainViewModel)mainViewModel).NavigateToTransaksi(SelectedItem["id_transaksi"].ToString());
            }
        }


        private string _idTransaksi;
        public string IdTransaksi
        {
            get { return _idTransaksi; }
            set
            {
                _idTransaksi = value;
                OnPropertyChanged();
            }
        }

        private string _tanggalTransaksi;
        public string TanggalTransaksi
        {
            get { return _tanggalTransaksi; }
            set
            {
                _tanggalTransaksi = value;
                OnPropertyChanged();
            }
        }

        private string _kasir;
        public string Kasir
        {
            get { return _kasir; }
            set
            {
                _kasir = value;
                OnPropertyChanged();
            }
        }

        private string _pelanggan;
        public string Pelanggan
        {
            get { return _pelanggan; }
            set
            {
                _pelanggan = value;
                OnPropertyChanged();
            }
        }

        private string _totalHarga;
        public string TotalHarga
        {
            get { return _totalHarga; }
            set
            {
                _totalHarga = value;
                OnPropertyChanged();
            }
        }
    }
}
