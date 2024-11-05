using MinimarketApp.Model;
using MinimarketApp.Repository;
using MinimarketApp.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System.Globalization;
using System.Diagnostics;

namespace MinimarketApp.ViewModel
{
    public class DiskonVM : ViewModelBase
    {
        private ProdukRepository _produkRepository = new();
        protected DiskonRepository _diskonRepository = new();
        private SatuanRepository _satuanRepository = new();

        public DiskonVM()
        {
            ResetAll();

            AddCommand = new RelayCommand(Add, CanAdd);
            EditCommand = new RelayCommand(Edit, CanEdit);
            DeleteCommand = new RelayCommand(Delete, CanDelete);
            FreeIdCommand = new RelayCommand(FreeId);

        }

        public void ResetAll()
        {
            ClearAll();
            RefreshDatabase();

            // ComboSatuan = _satuanRepository.GetAll()?.DefaultView;
            // SelectedSatuan = ComboSatuan?.Count > 0 ? ComboSatuan?[0] : null;

            ComboDiskonBerdasarkan = _diskonRepository.GetComboJenisDiskon();
            SelectedComboDiskonBerdasarkan = ComboDiskonBerdasarkan?.Count > 0 ? ComboDiskonBerdasarkan?[0] : null;

            ComboTipeDiskon = _diskonRepository.GetComboPilihanDiskon();
            SelectedComboTipeDiskon = ComboTipeDiskon?.Count > 0 ? ComboTipeDiskon?[0] : null;

            InputTanggalMulai = DateTime.Now;
            InputTanggalBerakhir = DateTime.Now;
        }

        public void RefreshDatabase()
        {
            Items = _diskonRepository.GetAll().DefaultView;
        }

        private void ClearAll()
        {
            SelectedCbProduk = null;
            DataTable ProdukData = _produkRepository.GetAllTersediaProdukLike("");
            CbProduk = ProdukData.DefaultView;
            System.Threading.Thread.Sleep(100);

            InputKodeDiskon = _diskonRepository.freeId;
            CbProdukText = "";
            InputJumlah = "";
            InputNilaiDiskon = "";
            InputKeterangan = "";

            
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
                    InputKodeDiskon = SelectedItem["id_diskon"].ToString();
                    InputJumlah = SelectedItem["total"].ToString();
                    SelectedComboDiskonBerdasarkan = SelectedItem["jenis_diskon"].ToString();
                    SelectedComboTipeDiskon = SelectedItem["pilihan_diskon"].ToString();
                    InputNilaiDiskon = SelectedItem["nilai"].ToString();
                    InputTanggalMulai = (DateTime)SelectedItem["tanggal_mulai"];
                    InputTanggalBerakhir = (DateTime)SelectedItem["tanggal_akhir"];
                    InputKeterangan = SelectedItem["keterangan"].ToString();

                    CbProdukText = "";
                    
                    ComboSatuan = _satuanRepository.GetProdukSatuanPenjualan(SelectedItem["id_produk"].ToString()).DefaultView;
                    System.Threading.Thread.Sleep(50);
                    
                    CbProduk.Sort = "ComboProdukView";
                    CbProdukText = SelectedItem["barcode"].ToString() + " - " + SelectedItem["nama_produk"].ToString();
                    ProdukExist = CbProduk.FindRows(CbProdukText).Length > 0;
                }
            }
        }

        private string? _inputKodeDiskon;
        public string? InputKodeDiskon
        {
            get { return _inputKodeDiskon; }
            set
            {
                value = value.Length > 10 ? value[..10] : value;
                _inputKodeDiskon = value;
                OnPropertyChanged();
                isExist = _diskonRepository.IsExist(value);
            }
        }

        private bool isExist = false;

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

                        ComboSatuan = _satuanRepository.GetProdukSatuanPenjualan(SelectedCbProduk["id_produk"].ToString()).DefaultView;
                        SelectedSatuan = ComboSatuan?.Count > 0 ? ComboSatuan?[0] : null;
                    }
                    else
                    {
                        SelectedCbProduk = null;
                        _produkExist = false;
                        ComboSatuan = new();
                        SelectedSatuan = null;
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
                // Debug.WriteLine(value);
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
                SatuanHasItems = ComboSatuan.Count > 0;
            }
        }

        public bool SatuanHasItems { get; set; } = false;

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

        private List<string> _comboDiskonBerdasarkan;
        public List<string> ComboDiskonBerdasarkan
        {
            get { return _comboDiskonBerdasarkan; }
            set
            {
                _comboDiskonBerdasarkan = value;
                OnPropertyChanged();
            }
        }

        private string _selectedComboDiskonBerdasarkan;
        public string SelectedComboDiskonBerdasarkan
        {
            get { return _selectedComboDiskonBerdasarkan; }
            set
            {
                _selectedComboDiskonBerdasarkan = value;
                OnPropertyChanged();
            }
        }

        private string? _inputJumlah;
        public string? InputJumlah
        {
            get { return _inputJumlah; }
            set
            {
                value = value.Length > 11 ? value[..11] : value;
                _inputJumlah = value;
                OnPropertyChanged();
            }
        }
        
        private string? _inputNilaiDiskon;
        public string? InputNilaiDiskon
        {
            get { return _inputNilaiDiskon; }
            set
            {
                value = value.Length > 11 ? value[..11] : value;
                _inputNilaiDiskon = value;
                OnPropertyChanged();
            }
        }

        private List<string> _comboTipeDiskon;
        public List<string> ComboTipeDiskon
        {
            get { return _comboTipeDiskon; }
            set
            {
                _comboTipeDiskon = value;
                OnPropertyChanged();
            }
        }

        private string _selectedComboTipeDiskon;
        public string SelectedComboTipeDiskon
        {
            get { return _selectedComboTipeDiskon; }
            set
            {
                _selectedComboTipeDiskon = value;
                OnPropertyChanged();
            }
        }

        private DateTime? _inputTanggalMulai;
        public DateTime? InputTanggalMulai
        {
            get { return _inputTanggalMulai; }
            set
            {
                _inputTanggalMulai = value;
                OnPropertyChanged();
            }
        }

        private DateTime? _inputTanggalBerakhir;
        public DateTime? InputTanggalBerakhir
        {
            get { return _inputTanggalBerakhir; }
            set
            {
                _inputTanggalBerakhir = value;
                OnPropertyChanged();
            }
        }


        private string? _inputKeterangan;
        public string? InputKeterangan
        {
            get { return _inputKeterangan; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    value = value.Length > 100 ? value[..100] : value;
                }
                 _inputKeterangan = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand FreeIdCommand { get; }

        private bool CanAdd(object obj)
        {
            return !isExist && SelectedComboDiskonBerdasarkan != null && ParseFloat(InputJumlah) > 0 && SelectedComboTipeDiskon != null && ParseFloat(InputNilaiDiskon) > 0 && InputTanggalMulai != null && InputTanggalBerakhir != null && ProdukExist && Session.IsAdmin();
        }

        private void Add(object obj)
        {
            DiskonModel model = new()
            {
                id_diskon = InputKodeDiskon,
                id_produk = SelectedCbProduk["id_produk"].ToString(),
                id_satuan = SelectedSatuan["id_satuan"].ToString(),
                jenis_diskon = SelectedComboDiskonBerdasarkan,
                total = ParseInt(InputJumlah),
                pilihan_diskon = SelectedComboTipeDiskon,
                nilai = ParseInt(InputNilaiDiskon),
                tanggal_mulai = (DateTime)InputTanggalMulai,
                tanggal_berakhir = (DateTime)InputTanggalBerakhir,
                keterangan = InputKeterangan
            };
            var result = _diskonRepository.Insert(model);
            if (result > 0)
            {
                ResetAll();
                isExist = _diskonRepository.IsExist(InputKodeDiskon);
                MessageBox.Show("Diskon berhasil ditambahkan");
            }
            else
            {
                MessageBox.Show("Diskon gagal ditambahkan");
            }
        }

        private bool CanEdit(object obj)
        {
            return isExist && SelectedComboDiskonBerdasarkan != null && ParseFloat(InputJumlah) > 0 && SelectedComboTipeDiskon != null && ParseFloat(InputNilaiDiskon) > 0 && InputTanggalMulai != null && InputTanggalBerakhir != null && ProdukExist && Session.IsAdmin();
        }

        private void Edit(object obj)
        {

            DiskonModel model = new()
            {
                id_diskon = InputKodeDiskon,
                id_produk = SelectedCbProduk["id_produk"].ToString(),
                id_satuan = SelectedSatuan["id_satuan"].ToString(),
                jenis_diskon = SelectedComboDiskonBerdasarkan,
                total = ParseInt(InputJumlah),
                pilihan_diskon = SelectedComboTipeDiskon,
                nilai = ParseInt(InputNilaiDiskon),
                tanggal_mulai = (DateTime)InputTanggalMulai,
                tanggal_berakhir = (DateTime)InputTanggalBerakhir,
                keterangan = InputKeterangan
            };
            var result = _diskonRepository.Update(model);
            if (result > 0)
            {
                RefreshDatabase();
                isExist = _diskonRepository.IsExist(InputKodeDiskon);
                MessageBox.Show("Diskon berhasil diubah");
            }
            else
            {
                MessageBox.Show("Diskon gagal diubah");
            }
        }

        private bool CanDelete(object obj)
        {
            return isExist && Session.IsAdmin();
        }

        private void Delete(object obj)
        {
            DiskonModel model = new()
            {
                id_diskon = InputKodeDiskon
            };
            var result = _diskonRepository.Delete(model);
            if (result > 0)
            {
                ResetAll();
                isExist = _diskonRepository.IsExist(InputKodeDiskon);
                MessageBox.Show("Diskon berhasil dihapus");
            }
            else
            {
                MessageBox.Show("Diskon gagal dihapus");
            }
        }

        private void FreeId(object obj)
        {
            InputKodeDiskon = _diskonRepository.freeId;
        }
    }
}
