using MinimarketApp.Model;
using MinimarketApp.Repository;
using MinimarketApp.Utilities;
using MinimarketApp.View;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Input;


namespace MinimarketApp.ViewModel
{
    public class TransaksiPembelianVM : ViewModelBase
    {
        private ProdukRepository _produkRepository = new();
        private TransaksiPembelianRepository _transaksiPembelianRepository = new();
        private PemasokRepository _pemasokRepository = new();
        private UserRepository _userRepository = new();
        private SatuanRepository _satuanRepository = new();

        public TransaksiPembelianVM()
        {
            ClearAll();
            ResetAll();

            AddRowCommand = new RelayCommand(AddRow, CanAddRow);
            DeleteRowCommand = new RelayCommand(DeleteRow);
            SubmitTransaksiCommand = new RelayCommand(SubmitTransaksi, CanSubmitTransaksi);
            NewTransaksiCommand = new RelayCommand(NewTransaksi);
            ItemsSourceProduk = new ObservableCollection<TransaksiPembelianModel>();

            RefreshAutoInput();
        }

        private void ClearAll()
        {
            DataTable ProdukData = _produkRepository.GetAllTersediaProdukLike("");
            CbProduk = ProdukData.DefaultView;
            SelectedCbPemasok = null;
            System.Threading.Thread.Sleep(100);

            // InputJumlah = "";
            CbProdukText = "";
            

            RefreshAutoInput();
        }

        private void ResetAll()
        {
            ClearAll();
            InputKembalian = "Rp0,00";
            GrandTotal = "Rp0,00";
            InputIdTransaksiPembelian = _transaksiPembelianRepository.freeId;
            itemsSourceProduk?.Clear();
            
            CbPemasok = _pemasokRepository.GetAll().DefaultView;
            CbPemasokText = "";
            SelectedCbPemasok = null;

            InputPembayaran = "";
            RefreshAutoInput();
        }

        private string _grandTotal;
        public string GrandTotal
        {
            get { return _grandTotal; }
            set
            {
                _grandTotal = value;
                OnPropertyChanged();
            }
        }

        public void RefreshAutoInput()
        {
            if (ItemsSourceProduk?.Count > 0)
            {
                TotalHarga = FormattedCurrency((float)ItemsSourceProduk.Sum(x => x.subtotal));
                GrandTotal = FormattedCurrency((float)ItemsSourceProduk.Sum(x => x.subtotal));
                InputKembalian = FormattedCurrency(ParseFloat(InputPembayaran) - CurrencyToFloat(GrandTotal));
            }
            else
            {
                TotalHarga = "Rp0,00";
                InputKembalian = "Rp0,00";
            }
        }

        private string? inputIdTransaksiPembelian;
        public string? InputIdTransaksiPembelian
        {
            get { return inputIdTransaksiPembelian; }
            set
            {
                inputIdTransaksiPembelian = value;
                OnPropertyChanged();

            }
        }

        private DataView _cbPemasok;
        public DataView CbPemasok
        {
            get { return _cbPemasok; }
            set
            {
                _cbPemasok = value;
                OnPropertyChanged();
            }
        }

        private DataRowView _selectedCbPemasok;
        public DataRowView SelectedCbPemasok
        {
            get { return _selectedCbPemasok; }
            set
            {
                _selectedCbPemasok = value;
                OnPropertyChanged();
                if (SelectedCbPemasok != null)
                {
                    InputIdPemasok = SelectedCbPemasok["id_pemasok"].ToString();
                }
            }
        }

        private string _cbPemasokText;
        public string CbPemasokText
        {
            get { return _cbPemasokText; }
            set
            {
                _cbPemasokText = value;
                OnPropertyChanged();
                DataTable PemasokData = new();  
                PemasokData = _pemasokRepository.GetAll(value);

                CbPemasok = PemasokData.DefaultView;

                if (PemasokData.Columns.Contains("ComboPemasokView"))
                {
                    CbPemasok.Sort = "ComboPemasokView";
                    SupplierExist = CbPemasok.FindRows(value).Length > 0;
                }
                else
                {
                    CbPemasok = _pemasokRepository.GetAll().DefaultView;
                }
            }
        }

        private bool _supplierkExist = false;
        public bool SupplierExist
        {
            get { return _supplierkExist; }
            set
            {
                if (_supplierkExist != value)
                {
                    _supplierkExist = value;
                    CbPemasok.Sort = "ComboPemasokView";
                    if (value && CbPemasok != null && !string.IsNullOrEmpty(CbPemasokText))
                    {
                        SelectedCbPemasok = CbPemasok?.FindRows(CbPemasokText)?[0];
                    }
                    else
                    {
                        SelectedCbPemasok = null;
                        _supplierkExist = false;
                    }
                }
            }
        }

        private string? inputIdPemasok;
        public string? InputIdPemasok
        {
            get { return inputIdPemasok; }
            set
            {
                inputIdPemasok = value;
                OnPropertyChanged();
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

        private string _cbProdukText;
        public string CbProdukText
        {
            get { return _cbProdukText; }
            set
            {
                _cbProdukText = value;
                OnPropertyChanged();
                DataTable ProdukData = new();
                ProdukData = _produkRepository.GetAllTersediaProdukLike(value);

                CbProduk = ProdukData.DefaultView;

                if (ProdukData.Columns.Contains("ComboProdukView"))
                {
                    CbProduk.Sort = "ComboProdukView";
                    ProdukExist = CbProduk.FindRows(value).Length > 0;
                }
                else
                {
                    CbProduk = _produkRepository.GetAllTersediaProdukLike().DefaultView;
                }
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
                    }
                    else
                    {
                        SelectedCbProduk = null;
                        _produkExist = false;
                    }
                }
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
            }
        }

        private string inputHargaBeli;
        public string InputHargaBeli
        {
            get { return inputHargaBeli; }
            set
            {
                inputHargaBeli = value;
                OnPropertyChanged();
            }
        }

        private string totalHarga;
        public string TotalHarga
        {
            get { return totalHarga; }
            set
            {
                totalHarga = value;
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

        public ICommand AddRowCommand { get; set; }
        public ICommand DeleteRowCommand { get; set; }
        public ICommand SubmitTransaksiCommand { get; set; }
        public ICommand NewTransaksiCommand { get; set; }

        private ObservableCollection<TransaksiPembelianModel>? itemsSourceProduk;
        public ObservableCollection<TransaksiPembelianModel>? ItemsSourceProduk
        {
            get { return itemsSourceProduk; }
            set
            {
                itemsSourceProduk = value;
                OnPropertyChanged();
            }
        }

        private TransaksiPembelianModel selectedProduk;
        public TransaksiPembelianModel SelectedProduk
        {
            get { return selectedProduk; }
            set
            {
                selectedProduk = value;
                OnPropertyChanged();

            }
        }

        private bool CanAddRow(object obj)
        {
            return SelectedCbProduk != null && ProdukExist;
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
                ItemsSourceProduk.Add(new TransaksiPembelianModel
                {
                    id_transaksi_pembelian = InputIdTransaksiPembelian,
                    barcode = SelectedCbProduk["barcode"].ToString(),
                    kuantitas = 1,
                    Satuans = _satuanRepository.GetProdukSatuanPembelian(SelectedCbProduk["id_produk"].ToString()).DefaultView,
                    id_produk = SelectedCbProduk["id_produk"].ToString(),
                    nama_produk = SelectedCbProduk["nama_produk"].ToString(),
                    nama_satuan = SelectedCbProduk["nama_satuan"].ToString(),
                    harga_beli = 0,
                    harga_jual = ParseFloat(SelectedCbProduk["harga_jual"].ToString()),
                });
            }
            RefreshAutoInput();
            ClearAll();
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

        private bool CanSubmitTransaksi(object obj)
        {
            return !string.IsNullOrEmpty(InputIdTransaksiPembelian) && 
                CheckValidProduk() && 
                !string.IsNullOrEmpty(InputIdTransaksiPembelian) && 
                (CurrencyToFloat(InputKembalian) >= 0 && 
                CurrencyToFloat(GrandTotal) > 0) && 
                ItemsSourceProduk.Count > 0 &&
                SupplierExist &&
                !IsTransaksiLocked;
        }

        private void SubmitTransaksi(object obj)
        {
            int result = 0;
            result = _transaksiPembelianRepository.Insert(
                new TransaksiPembelianModel()
                {
                    // id_detail_produk = SelectedCbProduk.id_detail_produk,
                    id_pemasok = SelectedCbPemasok["id_pemasok"].ToString(),
                    id_transaksi_pembelian = InputIdTransaksiPembelian,
                    kuantitas = ParseInt(InputJumlah),
                    pembayaran = ParseFloat(InputPembayaran),
                    harga_beli = ParseFloat(InputHargaBeli),
                    total_harga = CurrencyToFloat(GrandTotal)
                },
                    ItemsSourceProduk
                );
            if (result > 0)
            {
                ResetAll();
                MessageBox.Show("Transaksi embelian berhasil disimpan");
            }
            TransaksiExist(InputIdTransaksiPembelian);
        }

        private void NewTransaksi(object obj)
        {
            InputIdTransaksiPembelian = _transaksiPembelianRepository.freeId;
            ClearAll();
            ItemsSourceProduk?.Clear();
        }

        private bool CheckValidProduk()
        {
            foreach (var item in ItemsSourceProduk)
            {
                if (item.harga_beli < 1 || item.kuantitas < 1)
                {
                    return false;
                }
            }
            return true;
        }

        private bool _isTransaksiExist = false;
        private bool isTransaksiExist
        {
            get { return _isTransaksiExist; }
            set
            {
                _isTransaksiExist = value;
                OnPropertyChanged();
                IsTransaksiLocked = value;
            }
        }

        private void TransaksiExist(string idTransaksi)
        {
            isTransaksiExist = _transaksiPembelianRepository.IsExist(idTransaksi);
        }

        public void ContinueTransaksi(string idTransaksi)
        {
            ObservableCollection<TransaksiPembelianModel> detailTransaksi = _transaksiPembelianRepository.GetDetailById(idTransaksi);
            CbPemasokText = "";
            if (detailTransaksi.Count > 0)
            {
                System.Threading.Thread.Sleep(100);
                ItemsSourceProduk = detailTransaksi;
                InputIdTransaksiPembelian = idTransaksi;
                GrandTotal = FloatToCurrency((float)ItemsSourceProduk.Sum(x => x.subtotal));
                InputPembayaran = !IsTransaksiLocked ? detailTransaksi[0].pembayaran.ToString() : FloatToCurrency((float)detailTransaksi[0].pembayaran);
                CbPemasokText = detailTransaksi[0].id_pemasok + " - " + detailTransaksi[0].nama_pemasok;
                TransaksiExist(idTransaksi);
                RefreshAutoInput();

                foreach (var item in ItemsSourceProduk)
                {
                    item.Satuans = _satuanRepository.GetProdukSatuanPembelian(item.id_produk.ToString()).DefaultView;
                    item.id_satuan = _transaksiPembelianRepository.GetSatuanTransaksi(item.id_detail_transaksi_pembelian.ToString());
                }
            }
        }

        private Visibility _collapseOnLocked;
        public Visibility CollapseOnLocked
        {
            get { return _collapseOnLocked; }
            set
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
                }
                else
                {
                    CollapseOnLocked = Visibility.Visible;
                }
                foreach (var item in ItemsSourceProduk)
                {
                    item.IsTransaksiLocked = value;
                }
            }
        }

        protected string? FloatToCurrency(float value)
        {
            return value.ToString("C", new CultureInfo("id-ID"));
        }

        protected float CurrencyToFloat(string value)
        {
            return float.Parse(value, NumberStyles.Currency, new CultureInfo("id-ID"));
        }
    }
}
