using Microsoft.EntityFrameworkCore.Storage.Json;
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
    public class GrupProdukVM : ViewModelBase
    {
        private GrupProdukRepository _grupProdukRepository = new();
        private SatuanRepository _satuanRepository = new();
        private ProdukRepository _produkRepository = new();


        public GrupProdukVM()
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
            InputIdGrupProduk = _grupProdukRepository.freeId;
            ComboSatuan = _satuanRepository.GetAll()?.DefaultView;
            SelectedSatuan = ComboSatuan?.Count > 0 ? ComboSatuan?[0] : null;

            CbProdukText = "";
            System.Threading.Thread.Sleep(100);
            SelectedCbProduk = null;

            InputBarcodeUnit = "";
            InputHargaJualUnit = "";

            RefreshDatabase();
        }

        public void RefreshDatabase()
        {
            Items = _grupProdukRepository.GetAll().DefaultView;
        }

        public void ClearAll()
        {
            DataTable ProdukData = _produkRepository.GetAllProdukLike("");

            CbProduk = ProdukData.DefaultView;
            SelectedCbProduk = null;
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
                    InputIdGrupProduk = SelectedItem["id_grup_produk"].ToString();
                    InputBarcodeUnit = SelectedItem["barcode_unit"].ToString();
                    ComboSatuan.Sort = "id_satuan";
                    SelectedSatuan = ComboSatuan[ComboSatuan.Find(SelectedItem["id_satuan"])];
                    InputKuantitasProduk = SelectedItem["kuantitas_produk"].ToString();
                    InputHargaJualUnit = SelectedItem["harga_jual_unit"].ToString();

                    CbProdukText = "";
                    System.Threading.Thread.Sleep(50);
                    CbProdukText = SelectedItem["barcode"].ToString() + " - " + SelectedItem["nama_produk"].ToString();
                    CbProduk.Sort = "ComboProdukView";
                    ProdukExist = CbProduk.FindRows(CbProdukText).Length > 0;
                }
            }
        }

        private void CheckProdukAndBarcode()
        {
            grupExist = _grupProdukRepository.IsExist(InputIdGrupProduk);
            barcodeExist = _grupProdukRepository.IsBarcodeExist(InputBarcodeUnit, InputIdGrupProduk);
        }

        private string inputIdGrupProduk;
        public string InputIdGrupProduk
        {
            get { return inputIdGrupProduk; }
            set
            {
                value = value.Length > 10 ? value[..10] : value;
                inputIdGrupProduk = value;
                OnPropertyChanged();
                CheckProdukAndBarcode();
            }
        }

        private string? inputBarcodeUnit;
        public string? InputBarcodeUnit
        {
            get { return inputBarcodeUnit; }
            set
            {
                if (value.Length > 15)
                {
                    value = value.Substring(0, 15);
                }
                inputBarcodeUnit = value;
                OnPropertyChanged();
                CheckProdukAndBarcode();
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

        private string? _cbProdukText;
        public string? CbProdukText
        {
            get { return _cbProdukText; }
            set
            {
                value = value.Length > 100 ? value[..100] : value;
                _cbProdukText = value;
                OnPropertyChanged();
                DataTable ProdukData = new();
                ProdukData = _produkRepository.GetAllProdukLike(value);

                CbProduk = ProdukData.DefaultView;

                if (ProdukData.Columns.Contains("ComboProdukView"))
                {
                    CbProduk.Sort = "ComboProdukView";
                    ProdukExist = CbProduk.FindRows(value).Length > 0;
                }
                else
                {
                    CbProduk = _produkRepository.GetAllProdukLike().DefaultView;
                }
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

        private string? inputKuantitasProduk;
        public string? InputKuantitasProduk
        {
            get { return inputKuantitasProduk; }
            set
            {
                inputKuantitasProduk = value;
                OnPropertyChanged();
            }
        }

        private string? inputHargaJualUnit;
        public string? InputHargaJualUnit
        {
            get { return inputHargaJualUnit; }
            set
            {
                inputHargaJualUnit = value;
                OnPropertyChanged();
            }
        }

        private bool grupExist { get; set; } = false;
        private bool barcodeExist { get; set; } = false;

        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand FreeIdCommand { get; }
        public ICommand ResetCommand { get; }

        private bool CanAdd(object obj)
        {
            return !string.IsNullOrEmpty(InputIdGrupProduk) && !string.IsNullOrEmpty(InputKuantitasProduk) && !string.IsNullOrEmpty(InputHargaJualUnit) && !grupExist && (!barcodeExist || string.IsNullOrEmpty(InputBarcodeUnit)) && Session.IsAdmin();
        }

        private void Add(object obj)
        {
            GrupProdukModel model = new()
            {
                id_grup_produk = InputIdGrupProduk,
                barcode_unit = InputBarcodeUnit,
                produk = _produkRepository.GetProdukById(SelectedCbProduk["id_produk"].ToString()),
                satuan = new() { id_satuan = SelectedSatuan["id_satuan"].ToString() },
                kuantitas_produk = ParseInt(InputKuantitasProduk),
                harga_jual_unit = ParseFloat(InputHargaJualUnit)
            };

            var result = _grupProdukRepository.Insert(model);
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
            return !string.IsNullOrEmpty(InputIdGrupProduk) && !string.IsNullOrEmpty(InputKuantitasProduk) && !string.IsNullOrEmpty(InputHargaJualUnit) && grupExist && (!barcodeExist || string.IsNullOrEmpty(InputBarcodeUnit)) && Session.IsAdmin();
        }

        private void Edit(object obj)
        {
            GrupProdukModel model = new()
            {
                id_grup_produk = InputIdGrupProduk,
                barcode_unit = InputBarcodeUnit,
                produk = _produkRepository.GetProdukById(SelectedCbProduk["id_produk"].ToString()),
                satuan = new() { id_satuan = SelectedSatuan["id_satuan"].ToString() },
                kuantitas_produk = ParseInt(InputKuantitasProduk),
                harga_jual_unit = ParseFloat(InputHargaJualUnit)
            };

            var result = _grupProdukRepository.Update(model);
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
            return !string.IsNullOrEmpty(InputIdGrupProduk) && grupExist && Session.IsAdmin();
        }

        private void Delete(object obj)
        {
            GrupProdukModel model = new()
            {
                id_grup_produk = InputIdGrupProduk
            };
            var result = _grupProdukRepository.Delete(model);
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
            InputIdGrupProduk = _grupProdukRepository.freeId;
        }

        private void ResetAll(object obj)
        {
            ResetAll();
        }
    }
}
