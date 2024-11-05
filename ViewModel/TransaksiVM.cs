using Google.Protobuf.WellKnownTypes;
using MinimarketApp.Model;
using MinimarketApp.Repository;
using MinimarketApp.Utilities;
using MinimarketApp.View;
using Mysqlx.Crud;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Printing;
using System.Windows.Shapes;
using PdfSharp.Pdf;
using PdfSharp;
using TheArtOfDev.HtmlRenderer.PdfSharp;
using System.Reflection.PortableExecutable;

namespace MinimarketApp.ViewModel
{
    public class TransaksiVM : ViewModelBase
    {
        private ProdukRepository _produkRepository = new();
        private DiskonRepository _diskonRepository = new();
        private PelangganRepository _pelangganRepository = new();
        private TransaksiRepository _transaksiRepository = new();
        private UserRepository _userRepository = new();
        private SatuanRepository _satuanRepository = new();

        public TransaksiVM()
        {
            isTransaksiExist = false;
            ClearAll();
            ItemsSourceProduk?.Clear();

            AddRowCommand = new RelayCommand(AddRow, CanAddRow);
            DeleteRowCommand = new RelayCommand(DeleteRow, CanDeleteRow);
            BayarTransaksiCommand = new RelayCommand(BayarTransaksi, CanBayarTransaksi);
            NewTransaksiCommand = new RelayCommand(NewTransaksi);
            CancelTransaksiCommand = new RelayCommand(CancelTransaksi, CanCancelTransaksi);
            SubmitTransaksiCommand = new RelayCommand(SubmitTransaksi, CanSubmitTransaksi);
            GetFirstItemCommand = new RelayCommand(GetFirstItem);
            PrintCommand = new RelayCommand(Print);

            ItemsSourceProduk = new ObservableCollection<TransaksiModel>();
            ItemsSourceDetailProduk = new ObservableCollection<ProdukModel>();

            InputIdTransaksi = _transaksiRepository.freeId;
            CbPelanggan = _pelangganRepository.GetAll().DefaultView;
            CbPelangganText = "";
            SelectedCbPelanggan = null;
            InputPembayaran = "";

            RefreshAutoInput();
        }

        private void ClearAll()
        {
            // InputJumlah = "";
            // CbProdukStok = "";
            DataTable ProdukData = _produkRepository.GetAllActiveProdukLike("");
            CbProduk = ProdukData.DefaultView;
            SelectedCbProduk = null;
            System.Threading.Thread.Sleep(1000);
            CbProdukText = "";

            RefreshAutoInput();
        }



        private string? inputIdTransaksi;
        public string? InputIdTransaksi
        {
            get { return inputIdTransaksi; }
            set
            {
                inputIdTransaksi = value;
                OnPropertyChanged();

            }
        }

        private DataView _cbPelanggan;
        public DataView CbPelanggan
        {
            get { return _cbPelanggan; }
            set
            {
                _cbPelanggan = value;
                OnPropertyChanged();
            }
        }

        private DataRowView _selectedCbPelanggan;
        public DataRowView SelectedCbPelanggan
        {
            get { return _selectedCbPelanggan; }
            set
            {
                _selectedCbPelanggan = value;
                OnPropertyChanged();
            }
        }

        private string? _cbPelangganText;

        public string? CbPelangganText
        {
            get { return _cbPelangganText; }
            set
            {
                _cbPelangganText = value;
                OnPropertyChanged();

                DataTable PelangganData = new();
                PelangganData = _pelangganRepository.GetAll(value);

                CbPelanggan = PelangganData.DefaultView;

                if (PelangganData.Columns.Contains("telp_pelanggan"))
                {
                    CbPelanggan.Sort = "telp_pelanggan";
                    PelangganExist = CbPelanggan.FindRows(value).Length > 0;
                }
                else
                {
                    CbPelanggan = _pelangganRepository.GetAll().DefaultView;
                }
            }
        }

        private bool _pelanggankExist = false;
        public bool PelangganExist
        {
            get { return _pelanggankExist; }
            set
            {
                // Debug.WriteLine(value);
                if (_pelanggankExist != value)
                {
                    _pelanggankExist = value;
                    CbPelanggan.Sort = "telp_pelanggan";
                    if (value && CbPelanggan != null && !string.IsNullOrEmpty(CbPelangganText))
                    {
                        SelectedCbPelanggan = CbPelanggan?.FindRows(CbPelangganText)?[0];
                    }
                    else
                    {
                        SelectedCbPelanggan = null;
                        _pelanggankExist = false;
                    }
                    foreach (var item in ItemsSourceProduk)
                    {
                        item.diskonAktif = value;
                    }
                    RefreshAutoInput();
                }
            }
        }

        private DataView cbProduk;
        public DataView CbProduk
        {
            get { return cbProduk; }
            set
            {
                cbProduk = value;
                OnPropertyChanged();
            }
        }

        private DataRowView selectedCbProduk;
        public DataRowView SelectedCbProduk
        {
            get { return selectedCbProduk; }
            set
            {
                selectedCbProduk = value;
                OnPropertyChanged();
            }
        }

        private string inputJumlah;
        public string InputJumlah
        {
            get { return inputJumlah; }
            set
            {
                inputJumlah = value;
                OnPropertyChanged();

                if (!string.IsNullOrEmpty(value))
                    TidakMencukupiVisibility = ParseInt(value) > ParseInt(CbProdukStok) && ProdukExist ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private ObservableCollection<ProdukModel> _itemsSourceDetailProduk;

        public ObservableCollection<ProdukModel> ItemsSourceDetailProduk
        {
            get { return _itemsSourceDetailProduk; }
            set
            {
                _itemsSourceDetailProduk = value;
                OnPropertyChanged();
            }
        }


        private string grandTotal = "Rp0,00";
        public string GrandTotal
        {
            get { return grandTotal; }
            set
            {
                grandTotal = value;
                OnPropertyChanged();
                InputKembalian = (!IsTransaksiLocked) ? FloatToCurrency(ParseFloat(InputPembayaran) - CurrencyToFloat(value)) : FloatToCurrency(CurrencyToFloat(InputPembayaran) - CurrencyToFloat(value));
            }
        }

        private string _totalDiskon;
        public string TotalDiskon
        {
            get { return _totalDiskon; }
            set
            {
                _totalDiskon = value;
                OnPropertyChanged();
            }
        }

        private string inputPembayaran;
        public string InputPembayaran
        {
            get { return inputPembayaran; }
            set
            {
                inputPembayaran = value;
                OnPropertyChanged();
                RefreshAutoInput();
            }
        }

        private string inputKembalian;
        public string InputKembalian
        {
            get { return inputKembalian; }
            set
            {
                inputKembalian = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<TransaksiModel>? itemsSourceProduk;
        public ObservableCollection<TransaksiModel>? ItemsSourceProduk
        {
            get { return itemsSourceProduk; }
            set
            {
                itemsSourceProduk = value;
                OnPropertyChanged();
            }
        }

        private TransaksiModel selectedProduk;
        public TransaksiModel SelectedProduk
        {
            get { return selectedProduk; }
            set
            {
                selectedProduk = value;
                OnPropertyChanged();
            }
        }

        private bool _produkExist = false;
        public bool ProdukExist
        {
            get { return _produkExist; }
            set
            {
                if (_produkExist != value)
                {
                    _produkExist = value;
                    CbProduk.Sort = "ComboProdukView";
                    if (value && CbProduk != null && !string.IsNullOrEmpty(CbProdukText))
                    {
                        SelectedCbProduk = CbProduk?.FindRows(CbProdukText)?[0];
                        CbProdukStok = SelectedCbProduk["stok"].ToString();
                    }
                    else
                    {
                        SelectedCbProduk = null;
                        _produkExist = false;
                        CbProdukStok = "0";
                        InputJumlah = "0";
                    }
                    TidakTersediaVisibility = value ? Visibility.Collapsed : Visibility.Visible;
                }
            }
        }

        private Visibility _tidakTersediaVisibility = Visibility.Collapsed;
        public Visibility TidakTersediaVisibility
        {
            get { return _tidakTersediaVisibility; }
            set
            {
                if (_tidakTersediaVisibility != value)
                {
                    _tidakTersediaVisibility = value;
                    OnPropertyChanged();
                }
            }
        }
        
        private Visibility _tidakMencukupiVisibility = Visibility.Collapsed;
        public Visibility TidakMencukupiVisibility
        {
            get { return _tidakMencukupiVisibility; }
            set
            {
                if (_tidakMencukupiVisibility != value)
                {
                    _tidakMencukupiVisibility = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _cbProdukText;
        public string CbProdukText
        {
            get { return _cbProdukText; }
            set
            {
                _cbProdukText = value;
                OnPropertyChanged();
                DataTable ProdukData = new();
                ProdukData = _produkRepository.GetAllActiveProdukLike(value);
                
                CbProduk = ProdukData.DefaultView;

                if (ProdukData.Columns.Contains("ComboProdukView"))
                {
                    CbProduk.Sort = "ComboProdukView";
                    ProdukExist = CbProduk.FindRows(value).Length > 0;
                }
                else
                {
                    CbProduk = _produkRepository.GetAllActiveProdukLike().DefaultView;
                }
            }
        }

        private string _cbProdukStok;
        public string CbProdukStok
        {
            get { return _cbProdukStok; }
            set
            {
                _cbProdukStok = value;
                OnPropertyChanged();

                if (!string.IsNullOrEmpty(value))
                    TidakMencukupiVisibility = ParseInt(InputJumlah) > ParseInt(value) && ProdukExist ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public ICommand AddRowCommand { get; set; }
        public ICommand DeleteRowCommand { get; set; }
        public ICommand SubmitTransaksiCommand { get; set; }
        public ICommand BayarTransaksiCommand { get; set; }
        public ICommand NewTransaksiCommand { get; set; }
        public ICommand CancelTransaksiCommand { get; set; }
        public ICommand PrintCommand { get; set; }

        private bool CanAddRow(object obj)
        {
            return ProdukExist && !IsTransaksiLocked;
        }

        private void AddRow(object obj)
        {
            bool isExist = false;
            foreach (var item in ItemsSourceProduk)
            {
                if (item.barcode == SelectedCbProduk["barcode"].ToString())
                {
                    item.kuantitas++;
                    isExist = true;
                }
            }   

            if (!isExist)
            {
                ItemsSourceProduk.Add(new TransaksiModel
                {
                    id_transaksi = InputIdTransaksi,
                    barcode = SelectedCbProduk["barcode"].ToString(),
                    id_produk = SelectedCbProduk["id_produk"].ToString(),
                    diskonAktif = PelangganExist,
                    diskons = _diskonRepository.GetActiveDiskons(SelectedCbProduk["id_produk"].ToString()),
                    Satuans = _satuanRepository.GetProdukSatuanPenjualan(SelectedCbProduk["id_produk"].ToString()).DefaultView,
                    // id_detail_produk = int.Parse(SelectedCbProduk["id_detail_produk"].ToString()),
                    nama_produk = SelectedCbProduk["nama_produk"].ToString(),
                    // harga_jual = float.Parse(SelectedCbProduk["harga_jual"].ToString()),
                    // formatted_harga_jual = SelectedCbProduk["formatted_harga_jual"].ToString(),
                    kuantitas = 1,
                    // stok = int.Parse(SelectedCbProduk["stok"].ToString()),
                    nama_satuan = SelectedCbProduk["nama_satuan"].ToString(),
                });
            }

            RefreshAutoInput();
            ClearAll();
        }

        private bool CanDeleteRow(object obj)
        {
            return ItemsSourceProduk != null && ItemsSourceProduk.Count > 0 && !IsTransaksiLocked;
        }

        private void DeleteRow(object obj)
        {
            if (ItemsSourceProduk == null || ItemsSourceProduk.Count <= 0)
            {
                MessageBox.Show("Tidak ada data yang bisa dihapus");
            }
            else
            {
                ItemsSourceProduk.Remove(SelectedProduk);
            }
            RefreshAutoInput();
        }

        private void NewTransaksi(object obj)
        {
            InputIdTransaksi = _transaksiRepository.freeId;
            ClearAll();
            ItemsSourceProduk?.Clear();
            TransaksiExist(InputIdTransaksi);
            UnlockedTransaksi(InputIdTransaksi);
            CbPelangganText = "";
            GrandTotal = "Rp0,00";
            InputPembayaran = "";
        }

        private bool CanSubmitTransaksi(object obj)
        {
            return !string.IsNullOrEmpty(InputIdTransaksi) && ItemsSourceProduk.Count > 0 && !IsTransaksiLocked && isJumlahValid && (PelangganExist || string.IsNullOrEmpty(CbPelangganText)) && CheckValidProduk();
        }

        public enum StatusTransaksi
        {
            Selesai,
            BelumSelesai,
            Dibatalkan
        }

        public StatusTransaksi status { get; set; } = StatusTransaksi.BelumSelesai;

        private void SubmitTransaksi(object obj)
        {
            int result = 0;
            if (!_transaksiRepository.IsExist(InputIdTransaksi))
            {
                result = _transaksiRepository.Insert(
                new TransaksiModel
                {
                    id_transaksi = InputIdTransaksi,
                    id_user = _userRepository.GetCurrentUserData().id_user,
                    telp_pelanggan = SelectedCbPelanggan != null ? SelectedCbPelanggan["telp_pelanggan"].ToString() : null,
                    total_harga = CurrencyToFloat(GrandTotal),
                    pembayaran = ParseFloat(InputPembayaran),
                    // kembalian = CurrencyToFloat(InputKembalian),
                },
                    ItemsSourceProduk
                );
            }
            else
            {
                result = _transaksiRepository.RefreshTransaksi(
                new TransaksiModel
                {
                    id_transaksi = InputIdTransaksi,
                    id_user = _userRepository.GetCurrentUserData().id_user,
                    telp_pelanggan = SelectedCbPelanggan != null ? SelectedCbPelanggan["telp_pelanggan"].ToString() : null,
                    total_harga = CurrencyToFloat(GrandTotal),
                    pembayaran = ParseFloat(InputPembayaran),
                    // kembalian = CurrencyToFloat(InputKembalian),
                },
                    ItemsSourceProduk
                );
            }
            if (result > 0)
            {
                ClearAll();
                MessageBox.Show("Transaksi berhasil disimpan");
            }
            TransaksiExist(InputIdTransaksi);
            UnlockedTransaksi(InputIdTransaksi);
        }
        
        private bool CanBayarTransaksi(object obj)
        {
            return !string.IsNullOrEmpty(InputIdTransaksi) && ItemsSourceProduk.Count > 0 && (CurrencyToFloat(InputKembalian) >= 0 && CurrencyToFloat(GrandTotal) > 0) && !IsTransaksiLocked && isJumlahValid && (PelangganExist || string.IsNullOrEmpty(CbPelangganText)) && CheckValidProduk();
        }

        private void BayarTransaksi(object obj)
        {
            // if already exists in database
            SubmitTransaksi(obj);
            _transaksiRepository.UpdateStatusTransaksi(InputIdTransaksi, "Selesai");
            TransaksiExist(InputIdTransaksi);
            UnlockedTransaksi(InputIdTransaksi);
            MessageBox.Show("Kembalian" + InputKembalian);
        }

        private string _kasirTransaksi;
        public string KasirTransaksi
        {
            get { return _kasirTransaksi; }
            set
            {
                _kasirTransaksi = value;
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

        private bool _isTransaksiExist = false;
        private bool isTransaksiExist {
            get { return _isTransaksiExist; }
            set
            {
                _isTransaksiExist = value;
                OnPropertyChanged();
                if (value)
                {
                    KasirTransaksi = _transaksiRepository.GetKasirTransaksi(InputIdTransaksi);
                    TanggalTransaksi = _transaksiRepository.GetTanggalTransaksi(InputIdTransaksi);
                }
                else
                {
                    KasirTransaksi = _userRepository.GetCurrentUserData().username;
                    TanggalTransaksi = DateTime.UtcNow.ToString(@"dd MMMM yyyy", new CultureInfo("id-ID"));
                }
            }
        }

        private void TransaksiExist(string idTransaksi)
        {
            isTransaksiExist = _transaksiRepository.IsExist(idTransaksi);
        }

        private bool CanCancelTransaksi(object obj)
        {
            return !string.IsNullOrEmpty(InputIdTransaksi) && ItemsSourceProduk.Count > 0 && isTransaksiExist && !IsTransaksiLocked && (PelangganExist || string.IsNullOrEmpty(CbPelangganText));
        }

        private void CancelTransaksi(object obj)
        {
            _transaksiRepository.UpdateStatusTransaksi(InputIdTransaksi, "Dibatalkan");
            _transaksiRepository.CancelTransaksi(InputIdTransaksi);
            TransaksiExist(InputIdTransaksi);
            MessageBox.Show("Transaksi dibatalkan");
        }


        public void RefreshAutoInput()
        {
            if (ItemsSourceProduk?.Count > 0)
            {
                TotalDiskon = FloatToCurrency((float)ItemsSourceProduk.Sum(x => x.harga_diskon));
                if (!PelangganExist)
                { 
                    GrandTotal = FloatToCurrency((float)ItemsSourceProduk.Sum(x => x.subtotal));
                }
                else
                {
                    GrandTotal = FloatToCurrency((float)ItemsSourceProduk.Sum(x => x.subtotal) - CurrencyToFloat(TotalDiskon));
                }
            }
            else
            {
                TotalDiskon = "Rp0,00";
                GrandTotal = "Rp0,00";
                InputKembalian = "Rp0,00";
            }
            CheckStok();
        }

        private void Print(object obj)
        {
            
        }

        private bool isJumlahValid = false;

        public void CheckStok()
        {
            if (ItemsSourceProduk == null || ItemsSourceProduk.Count < 1)
            {
                isJumlahValid = false;
                return;
            }
            foreach (TransaksiModel transaksiModel in ItemsSourceProduk)
            {
                if (transaksiModel.kuantitas > transaksiModel.stok)
                {
                    isJumlahValid = false;
                    return;
                }
            }
            isJumlahValid = true;
        }

        private float ParseFloat(string value)
        {
            float result = 0;
            float.TryParse(value, out result);
            return result;
        }

        private int ParseInt(string value)
        {
            int result = 0;
            int.TryParse(value, out result);
            return result;
        }

        protected string? FloatToCurrency(float value)
        {
            return value.ToString("C", new CultureInfo("id-ID"));
        }

        protected float CurrencyToFloat(string value)
        {
            return float.Parse(value, NumberStyles.Currency, new CultureInfo("id-ID"));
        }

        public ICommand GetFirstItemCommand { get; set; }

        private void GetFirstItem(object obj)
        {
            // if the obj is combo box
            if (obj is ComboBox)
            {
                var comboBox = obj as ComboBox;
                comboBox.Text = comboBox.Items[0].ToString();
                comboBox.SelectedIndex = 0;
            }
        }

        public void ContinueTransaksi(string idTransaksi)
        {
            ObservableCollection<TransaksiModel> detailTransaksi = _transaksiRepository.GetDetailById(idTransaksi);
            CbPelangganText = "";

            if (detailTransaksi.Count > 0)
            {
                InputIdTransaksi = detailTransaksi[0].id_transaksi;
                System.Threading.Thread.Sleep(100);
                UnlockedTransaksi(idTransaksi);
                ItemsSourceProduk = detailTransaksi;
                InputIdTransaksi = idTransaksi;
                GrandTotal = FloatToCurrency((float)ItemsSourceProduk.Sum(x => x.subtotal));
                InputPembayaran = !IsTransaksiLocked ? detailTransaksi[0].pembayaran.ToString() : FloatToCurrency((float)detailTransaksi[0].pembayaran);
                CbPelangganText = detailTransaksi[0].telp_pelanggan;
                TransaksiExist(idTransaksi);
                RefreshAutoInput();
                
                foreach (var item in ItemsSourceProduk)
                {
                    item.diskons = _diskonRepository.GetActiveDiskons(item.id_produk);
                    item.Satuans = _satuanRepository.GetProdukSatuanPenjualan(item.id_produk).DefaultView;
                    item.id_satuan = _transaksiRepository.GetSatuanTransaksi(item.id_detail_transaksi.ToString());
                }
            }
        }

        private Visibility _collapseOnLocked;
        public Visibility CollapseOnLocked
        {
            get { return _collapseOnLocked; } set
            {
                _collapseOnLocked = value;
                OnPropertyChanged();
            }
        }

        private bool _isTransaksiLocked = false;
        public bool IsTransaksiLocked
        {
            get { return _isTransaksiLocked; }
            set
            {
                _isTransaksiLocked = value;
                OnPropertyChanged();
                if (value)
                {
                    InputPembayaran = FloatToCurrency(ParseFloat(InputPembayaran));
                    CollapseOnLocked = Visibility.Collapsed;
                } else
                {
                    CollapseOnLocked = Visibility.Visible;
                }
                foreach (var item in ItemsSourceProduk)
                {
                    item.IsTransaksiLocked = value;
                }
            }
        }

        private bool CheckValidProduk()
        {
            foreach (var item in ItemsSourceDetailProduk)
            {
                if (item.stok_kuantitas < 1)
                {
                    return false;
                }
            }
            return true;
        }

        public void UnlockedTransaksi(string idTransaksi)
        {
            string status = _transaksiRepository.GetStatusTransaksi(idTransaksi);
            IsTransaksiLocked = status == "Selesai" || status == "Dibatalkan";
        }
    }
}
