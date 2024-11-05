using MinimarketApp.Model;
using MinimarketApp.Repository;
using MinimarketApp.Utilities;
using System.Data;
using System.Windows;
using System.Windows.Input;

namespace MinimarketApp.ViewModel
{
    internal class KategoriVM : ViewModelBase
    {
        private KategoriRepository _kategoriRepository = new();

        public KategoriVM()
        {
            ResetAll();

            AddCommand = new RelayCommand(Add, CanAdd);
            EditCommand = new RelayCommand(Edit, CanEdit);
            DeleteCommand = new RelayCommand(Delete, CanDelete);
            FreeIdCommand = new RelayCommand(FreeId);
        }

        public void RefreshDatabase()
        {
            Items = _kategoriRepository.GetAll().DefaultView;
        }

        public void ResetAll()
        {
            InputIdKategori = _kategoriRepository.freeId;
            InputNamaKategori = "";
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
            set {
                selectedItem  = value;
                OnPropertyChanged();
                if (SelectedItem != null)
                {
                    InputIdKategori = SelectedItem["id_kategori"].ToString();
                    InputNamaKategori = SelectedItem["nama_kategori"].ToString();
                }
            }
        }

        private string? inputIdKategori;
        public string? InputIdKategori
        {
            get { return inputIdKategori; }
            set
            {
                if (value.Length > 6)
                {
                    value = value.Substring(0, 6);
                }
                inputIdKategori = value;
                OnPropertyChanged();
                kategoriExist = _kategoriRepository.IsExist(inputIdKategori);
            }
        }

        private bool kategoriExist;

        private string? inputNamaKategori;
        public string? InputNamaKategori
        {
            get { return inputNamaKategori; }
            set
            {
                if (value.Length > 40)
                {
                    value = value.Substring(0, 40);
                }
                inputNamaKategori = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand FreeIdCommand { get; }

        private bool CanAdd(object obj)
        {
            return !string.IsNullOrEmpty(InputIdKategori) && !string.IsNullOrEmpty(InputNamaKategori) && !kategoriExist;
        }

        private void Add(object obj)
        {
            KategoriModel model = new()
            {
                id_kategori = InputIdKategori,
                nama_kategori = InputNamaKategori
            };

            var result = _kategoriRepository.Insert(model);
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
            return !string.IsNullOrEmpty(InputIdKategori) && !string.IsNullOrEmpty(InputNamaKategori) && kategoriExist;
        }

        private void Edit(object obj)
        {
            KategoriModel model = new()
            {
                id_kategori = InputIdKategori,
                nama_kategori = InputNamaKategori
            };

            var result = _kategoriRepository.Update(model);
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
            return !string.IsNullOrEmpty(InputIdKategori) && kategoriExist;
        }

        private void Delete(object obj)
        {
            KategoriModel model = new()
            {
                id_kategori = InputIdKategori,
            };
            var result = _kategoriRepository.Delete(model);
            if (result > 0)
            {
                ResetAll();
            }
            else
            {
                MessageBox.Show("Data gagal dihapus");
            }
        }

        public void FreeId(object obj)
        {
            InputIdKategori = _kategoriRepository.freeId;
        }
    }
}
