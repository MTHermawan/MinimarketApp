using MinimarketApp.Model;
using MinimarketApp.Repository;
using MinimarketApp.Utilities;
using System.Data;
using System.Windows.Input;
using System.Windows;

namespace MinimarketApp.ViewModel
{
    public class LevelVM : ViewModelBase
    {
        private LevelRepository _levelRepository = new();

        public LevelVM()
        {
            ResetAll();
            AddCommand = new RelayCommand(Add, CanAdd);
            EditCommand = new RelayCommand(Edit, CanEdit);
            DeleteCommand = new RelayCommand(Delete, CanDelete);
            FreeIdCommand = new RelayCommand(FreeId);
        }

        public void RefreshDatabase()
        {
            Items = _levelRepository.GetAll().DefaultView;
        }

        public void ResetAll()
        {
            InputIdLevel = _levelRepository.freeId;
            InputNamaLevel = "";
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
                    InputIdLevel = SelectedItem["id_level"].ToString();
                    InputNamaLevel = SelectedItem["nama_level"].ToString();
                }
            }
        }


        private string? inputIdLevel;
        public string? InputIdLevel
        {
            get { return inputIdLevel; }
            set
            {
                value = value.Length > 1 ? value[..1] : value;
                inputIdLevel = value;
                OnPropertyChanged();
                levelExist = _levelRepository.IsExist(inputIdLevel);
            }
        }

        public bool levelExist { get; set; } = false;

        private string? inputNamaLevel;
        public string? InputNamaLevel
        {
            get { return inputNamaLevel; }
            set
            {
                value = value.Length > 20 ? value[..20] : value;
                inputNamaLevel = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand FreeIdCommand { get; }

        private bool CanAdd(object obj)
        {
            return !string.IsNullOrEmpty(InputIdLevel) && !string.IsNullOrEmpty(InputNamaLevel) && !levelExist;
        }

        private void Add(object obj)
        {
            LevelModel model = new()
            {
                id_level = InputIdLevel,
                nama_level = InputNamaLevel
            };

            var result = _levelRepository.Insert(model);
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
            return !string.IsNullOrEmpty(InputIdLevel) && !string.IsNullOrEmpty(InputNamaLevel) && levelExist;
        }

        private void Edit(object obj)
        {
            LevelModel model = new()
            {
                id_level = InputIdLevel,
                nama_level = InputNamaLevel
            };

            var result = _levelRepository.Update(model);
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
            return !string.IsNullOrEmpty(InputIdLevel) && levelExist;
        }

        private void Delete(object obj)
        {
            LevelModel model = new()
            {
                id_level = InputIdLevel,
            };
            var result = _levelRepository.Delete(model);
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
            InputIdLevel = _levelRepository.freeId;
        }
    }
}
