using MinimarketApp.Model;
using MinimarketApp.Repository;
using MinimarketApp.Utilities;
using System.Data;
using System.Windows;
using System.Windows.Input;

namespace MinimarketApp.ViewModel
{
    public class StokKeluarVM : ViewModelBase
    {
        private StokKeluarRepository _stokKeluarRepository = new();
        private ProdukRepository _produkRepository = new();

        public StokKeluarVM()
        {
            ResetAll();
            AddCommand = new RelayCommand(Add, CanAdd);
            EditCommand = new RelayCommand(Edit, CanEdit);
            DeleteCommand = new RelayCommand(Delete, CanDelete);
        }

        public void RefreshDatabase()
        {
            Items = _stokKeluarRepository.GetAll().DefaultView;
        }

        private void ClearAll()
        {
            DataTable ProdukData = _produkRepository.GetAllActiveProdukLike();
            CbProduk = ProdukData.DefaultView;
            SelectedCbProduk = null;

            ComboKeterangan = _stokKeluarRepository.GetKeterangan;
            SelectedKeterangan = ComboKeterangan?.Count > 0 ? ComboKeterangan?[0] : null;
            System.Threading.Thread.Sleep(100);

            InputIdStokKeluar = "";
            CbProdukText = "";
            InputJumlah = "";
        }

        private void ResetAll()
        {
            ClearAll();
            RefreshDatabase();
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
                        /*InputIdStokKeluar = SelectedItem["id_stok_keluar"].ToString();
                        SelectedCbProduk = CbProduk?.Count > 0 ? CbProduk?[0] : null;
                        InputJumlah = SelectedItem["kuantitas"].ToString();
                        SelectedKeterangan = SelectedItem["keterangan"].ToString();*/
                }
            }
        }

        private string inputIdStokKeluar;
        public string InputIdStokKeluar
        {
            get { return inputIdStokKeluar; }
            set
            {
                inputIdStokKeluar = value;
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
                        // CbProdukStok = SelectedCbProduk["stok"].ToString();
                    }
                    else
                    {
                        SelectedCbProduk = null;
                        _produkExist = false;
                        InputJumlah = "";
                    }
                    // TidakTersediaVisibility = value ? Visibility.Collapsed : Visibility.Visible;
                }
            }
        }

        private string _cbProdukText;
        public string CbProdukText
        {
            get { return _cbProdukText; }
            set
            {
                value = value.Length > 100 ? value.Substring(0, 100) : value;
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

        private string? _inputJumlah;
        public string? InputJumlah
        {
            get { return _inputJumlah; }
            set
            {
                value = value.Length > 10 ? value.Substring(0, 10) : value;
                _inputJumlah = value;
                OnPropertyChanged();
                if (SelectedCbProduk != null)
                {
                    if (ParseInt(value) > ParseInt(SelectedCbProduk["stok"].ToString()))
                    {
                        IsStokCukup = false;
                    }
                    else
                    {
                        IsStokCukup = true;
                    }
                } else
                {
                    IsStokCukup = false;
                }
            }
        }

        private bool _isStokCukup { get; set; } = false;
        public bool IsStokCukup
        {
            get { return _isStokCukup; }
            set
            {
                _isStokCukup = value;
                OnPropertyChanged();
            }
        }

        private List<string> comboKeterangan;
        public List<string> ComboKeterangan
        {
            get { return comboKeterangan; }
            set
            {
                comboKeterangan = value;
                OnPropertyChanged();
            }
        }

        private string selectedKeterangan;
        public string SelectedKeterangan
        {
            get { return selectedKeterangan; }
            set
            {
                selectedKeterangan = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }

        private bool CanAdd(object obj)
        {
            return ParseInt(InputJumlah) > 0 && SelectedKeterangan != null && ProdukExist && IsStokCukup;
        }

        private void Add(object obj)
        {
            StokKeluarModel model = new()
            {
                id_stok_keluar = InputIdStokKeluar,
                id_produk = SelectedCbProduk["id_produk"].ToString(),
                kuantitas = ParseInt(InputJumlah),
                keterangan = SelectedKeterangan
            };
            var result = _stokKeluarRepository.Insert(model);
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
            return ParseInt(InputJumlah) > 0 && SelectedKeterangan != null;
        }

        private void Edit(object obj)
        {
            StokKeluarModel model = new()
            {
                id_stok_keluar = InputIdStokKeluar,
                id_produk = SelectedCbProduk["id_produk"].ToString(),
                kuantitas = ParseInt(InputJumlah),
                keterangan = SelectedKeterangan
            };
            var result = _stokKeluarRepository.Update(model);
            if (result > 0)
            {
                RefreshDatabase();
                MessageBox.Show("Data berhasil diubah");
            }
            else
            {
                MessageBox.Show("Data gagal diubah");
            }
        }

        private bool CanDelete(object obj)
        {
            return !string.IsNullOrEmpty(InputIdStokKeluar);
        }

        private void Delete(object obj)
        {
            StokKeluarModel model = new()
            {
                id_stok_keluar = InputIdStokKeluar
            };
            var result = _stokKeluarRepository.Delete(model);
            if (result > 0)
            {
                RefreshDatabase();
            }
            else
            {
                MessageBox.Show("Data gagal dihapus");
            }
        }
    }
}
