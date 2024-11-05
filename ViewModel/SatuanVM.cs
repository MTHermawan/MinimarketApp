using MinimarketApp.Model;
using MinimarketApp.Repository;
using MinimarketApp.Utilities;
using System.Data;
using System.Windows.Input;
using System.Windows;

namespace MinimarketApp.ViewModel
{
    public class SatuanVM : ViewModelBase
    {
        private SatuanRepository _satuanRepository = new();

        public SatuanVM()
        {
            ResetAll();

            AddCommand = new RelayCommand(Add, CanAdd);
            EditCommand = new RelayCommand(Edit, CanEdit);
            DeleteCommand = new RelayCommand(Delete, CanDelete);
            FreeIdCommand = new RelayCommand(FreeId);
        }

        public void RefreshDatabase()
        {
            Items = _satuanRepository.GetAll().DefaultView;
        }

        public void ResetAll()
        {
            InputIdSatuan = _satuanRepository.freeId;
            InputNamaSatuan = "";

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
                    InputIdSatuan = SelectedItem["id_satuan"].ToString();
                    InputNamaSatuan = SelectedItem["nama_satuan"].ToString();
                }
            }
        }

        private string? inputIdSatuan;
        public string? InputIdSatuan
        {
            get { return inputIdSatuan; }
            set
            {
                inputIdSatuan = value;
                OnPropertyChanged();
                satuanExist = _satuanRepository.IsExist(inputIdSatuan);
            }
        }

        private bool satuanExist { get; set; } = false;

        private string? inputNamaSatuan;
        public string? InputNamaSatuan
        {
            get { return inputNamaSatuan; }
            set
            {
                inputNamaSatuan = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand FreeIdCommand { get; }

        private bool CanAdd(object obj)
        {
            return !string.IsNullOrEmpty(InputIdSatuan) && !string.IsNullOrEmpty(InputNamaSatuan) && !satuanExist;
        }

        private void Add(object obj)
        {
            SatuanModel model = new()
            {
                id_satuan = InputIdSatuan,
                nama_satuan = InputNamaSatuan
            };

            var result = _satuanRepository.Insert(model);
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
            return !string.IsNullOrEmpty(InputIdSatuan) && !string.IsNullOrEmpty(InputNamaSatuan) && satuanExist;
        }

        private void Edit(object obj)
        {
            SatuanModel model = new()
            {
                id_satuan = InputIdSatuan,
                nama_satuan = InputNamaSatuan
            };

            var result = _satuanRepository.Update(model);
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
            return !string.IsNullOrEmpty(InputIdSatuan) && satuanExist;
        }

        private void Delete(object obj)
        {
            SatuanModel model = new()
            {
                id_satuan = InputIdSatuan,
            };
            var result = _satuanRepository.Delete(model);
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
            InputIdSatuan = _satuanRepository.freeId;
        }
    }
}
