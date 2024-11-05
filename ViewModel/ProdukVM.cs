using MinimarketApp.Model;
using MinimarketApp.Repository;
using MinimarketApp.Utilities;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Input;

namespace MinimarketApp.ViewModel
{
    internal class ProdukVM : ViewModelBase
    {
        private ProdukRepository _produkRepository = new();
        private KategoriRepository _kategoriRepository = new();
        private SatuanRepository _satuanRepository = new();

        public ProdukVM()
        {
            ResetAll();

            AddCommand = new RelayCommand(Add, CanAdd);
            EditCommand = new RelayCommand(Edit, CanEdit);
            DeleteCommand = new RelayCommand(Delete, CanDelete);
            FreeIdCommand = new RelayCommand(FreeId);
            ResetCommand = new RelayCommand(ResetAll);
           
            RefreshDatabase();
        }

        private void ResetAll()
        {
            InputIdProduk = _produkRepository.freeId;
            ComboKategori = _kategoriRepository.GetAll()?.DefaultView;
            SelectedKategori = ComboKategori?.Count > 0 ? ComboKategori?[0] : null;
            ComboSatuan = _satuanRepository.GetAll()?.DefaultView;
            SelectedSatuan = ComboSatuan?.Count > 0 ? ComboSatuan?[0] : null;
            ComboStatus = _produkRepository?.GetStatus();
            SelectedStatus = ComboStatus?.Count > 0 ? ComboStatus?[0] : null;

            InputBarcode = "";
            InputNamaProduk = "";
            InputHargaJual = "";

            RefreshDatabase();
        }

        public void RefreshDatabase()
        {
            Items = _produkRepository.GetAllProduk().DefaultView;
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
                if (SelectedItem != null)
                {
                    InputIdProduk = SelectedItem["id_produk"].ToString();
                    InputBarcode = SelectedItem["barcode"].ToString();
                    InputNamaProduk = SelectedItem["nama_produk"].ToString();
                    InputHargaJual = SelectedItem["harga_jual"].ToString();
                    ComboKategori.Sort = "id_kategori";
                    SelectedKategori = ComboKategori[ComboKategori.Find(SelectedItem["id_kategori"])];
                    ComboSatuan.Sort = "id_satuan";
                    SelectedSatuan = ComboSatuan[ComboSatuan.Find(SelectedItem["id_satuan"])];
                    SelectedStatus = SelectedItem["status"].ToString();
                }
            }
        }

        private void CheckProdukAndBarcode()
        {
            produkExist = _produkRepository.IsExist(InputIdProduk);
            barcodeExist = _produkRepository.IsBarcodeExist(InputBarcode, InputIdProduk);
        }

        private string? inputIdProduk;
        public string? InputIdProduk
        {
            get { return inputIdProduk; }
            set
            {
                if (value.Length > 10)
                {
                    value = value.Substring(3, 7);
                }
                inputIdProduk = value;
                OnPropertyChanged();
                CheckProdukAndBarcode();
            }
        }

        private string? inputBarcode;
        public string? InputBarcode
        {
            get { return inputBarcode; }
            set
            {
                if (value.Length > 15)
                {
                    value = value.Substring(0, 15);
                }
                inputBarcode = value;
                OnPropertyChanged();
                CheckProdukAndBarcode();
            }
        }

        private string? inputNamaProduk;
        public string? InputNamaProduk
        {
            get { return inputNamaProduk; }
            set
            {
                if (value.Length > 50)
                {
                    value = value.Substring(0, 50);
                }
                inputNamaProduk = value;
                OnPropertyChanged();
            }
        }

        private DataView comboKategori;
        public DataView ComboKategori
        {
            get { return comboKategori; }
            set
            {
                comboKategori = value;
                OnPropertyChanged();
            }
        }

        private DataRowView selectedKategori;
        public DataRowView SelectedKategori
        {
            get { return selectedKategori; }
            set
            {
                selectedKategori = value;
                OnPropertyChanged();
            }
        }

        public DataView comboSatuan;
        public DataView ComboSatuan
        {
            get { return comboSatuan; }
            set
            {
                comboSatuan = value;
                OnPropertyChanged();
            }
        }

        private DataRowView selectedSatuan;
        public DataRowView SelectedSatuan
        {
            get { return selectedSatuan; }
            set
            {
                selectedSatuan = value;
                OnPropertyChanged();
            }
        }

        private string? inputHargaJual;
        public string? InputHargaJual
        {
            get { return inputHargaJual; }
            set
            {
                inputHargaJual = value;
                OnPropertyChanged();
            }
        }

        private List<string> comboStatus;
        public List<string> ComboStatus
        {
            get { return comboStatus; }
            set
            {
                comboStatus = value;
                OnPropertyChanged();
            }
        }

        private string selectedStatus;
        public string SelectedStatus
        {
            get { return selectedStatus; }
            set
            {
                selectedStatus = value;
                OnPropertyChanged();
            }
        }

        private bool produkExist { get; set; } = false;
        private bool barcodeExist { get; set; } = false;

        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand FreeIdCommand { get; }
        public ICommand ResetCommand { get; }

        private bool CanAdd(object obj)
        {
            return !string.IsNullOrEmpty(InputBarcode) && !string.IsNullOrEmpty(InputNamaProduk) && !produkExist && !barcodeExist && Session.IsAdmin();
        }

        private void Add(object obj)
        {
            ProdukModel model = new()
            {
                id_produk = InputIdProduk,
                barcode = InputBarcode,
                nama_produk = InputNamaProduk,
                id_kategori = SelectedKategori["id_kategori"].ToString(),
                id_satuan = SelectedSatuan["id_satuan"].ToString(),
                harga_jual = ParseInt(InputHargaJual),
                status = SelectedStatus
            };

            var result = _produkRepository.Insert(model);
            if (result > 0)
            {
                ResetAll();
            }
            else
            {
                MessageBox.Show("Data gagal ditambahkan");
            }
        }

        private bool CanEdit(object obj)
        {
            return !string.IsNullOrEmpty(InputBarcode) && !string.IsNullOrEmpty(InputNamaProduk) && produkExist && !barcodeExist && Session.IsAdmin();
        }

        private void Edit(object obj)
        {
            ProdukModel model = new()
            {
                id_produk = InputIdProduk,
                barcode = InputBarcode,
                nama_produk = InputNamaProduk,
                id_kategori = SelectedKategori["id_kategori"].ToString(),
                id_satuan = SelectedSatuan["id_satuan"].ToString(),
                harga_jual = ParseInt(InputHargaJual),
                status = SelectedStatus
            };

            var result = _produkRepository.Update(model);
            if (result > 0)
            {
                RefreshDatabase();
            }
            else
            {
                MessageBox.Show("Data gagal ditambahkan");
            }
        }

        private bool CanDelete(object obj)
        {
            return !string.IsNullOrEmpty(InputBarcode) && Session.IsAdmin();
        }

        private void Delete(object obj)
        {
            ProdukModel model = new()
            {
                barcode = InputBarcode,
            };
            var result = _produkRepository.Delete(model);
            if (result > 0)
            {
                ResetAll();
            }
            else
            {
                MessageBox.Show("Data gagal dihapus");
            }
        }

        private void FreeId(object obj)
        {
            InputIdProduk = _produkRepository.freeId;
        }

        private void ResetAll(object obj)
        {
            ResetAll();
        }
    }
}
