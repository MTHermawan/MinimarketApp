using MinimarketApp.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace MinimarketApp.View
{
    public partial class DetailTransaksiPenjualan : UserControl, INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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

        public TransaksiModel TransaksiData
        {
            get { return (TransaksiModel)GetValue(TransaksiDataProperty); }
            set { SetValue(TransaksiDataProperty, value); }
        }

        public static readonly DependencyProperty TransaksiDataProperty =
            DependencyProperty.Register("TransaksiData", typeof(TransaksiModel), typeof(DetailTransaksiPenjualan),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, TransaksiDataChanged, null, false, UpdateSourceTrigger.PropertyChanged));

        private static void TransaksiDataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is DetailTransaksiPenjualan detailTransaksiPenjualan)
            {
                TransaksiModel newValue = (TransaksiModel)e.NewValue;

                detailTransaksiPenjualan.IdTransaksi = newValue.id_transaksi;
                detailTransaksiPenjualan.TanggalTransaksi = newValue.formatted_tanggal_transaksi;
                detailTransaksiPenjualan.Kasir = newValue.username;
                detailTransaksiPenjualan.Pelanggan = newValue.nama_pelanggan;
                detailTransaksiPenjualan.TotalHarga = newValue.formatted_total_harga;
            }
        }

        public ObservableCollection<TransaksiModel> DetailTransaksiItems
        {
            get { return (ObservableCollection<TransaksiModel>)GetValue(DetailItemsProperty); }
            set { SetValue(DetailItemsProperty, value); }
        }

        public static readonly DependencyProperty DetailItemsProperty =
            DependencyProperty.Register("DetailTransaksiItems", typeof(ObservableCollection<TransaksiModel>), typeof(DetailTransaksiPenjualan),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, null, null, false, UpdateSourceTrigger.PropertyChanged));


        public DetailTransaksiPenjualan()
        {
            InitializeComponent();
        }
    }
}
