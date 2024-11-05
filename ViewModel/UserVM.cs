using MinimarketApp.Model;
using MinimarketApp.Repository;
using MinimarketApp.Utilities;
using System.Data;
using System.Windows;
using System.Windows.Input;

namespace MinimarketApp.ViewModel
{
    internal class UserVM : ViewModelBase
    {
        private UserRepository _userRepository = new();
        private LevelRepository _levelRepository = new();
        private KaryawanRepository _karyawanRepository = new();

        public UserVM()
        {
            ResetAll();

            AddCommand = new RelayCommand(Add, CanAdd);
            EditCommand = new RelayCommand(Edit, CanEdit);
            DeleteCommand = new RelayCommand(Delete, CanDelete);
            FreeIdCommand = new RelayCommand(FreeId);
        }

        private void ResetAll()
        {
            InputIdUser = _userRepository.freeId;
            InputUsername = "";
            InputPassword = "";

            ComboLevel = _levelRepository.GetAll().DefaultView;
            ComboKaryawan = _karyawanRepository.GetAll().DefaultView;
            SelectedLevel = ComboLevel?.Count > 0 ? ComboLevel?[0] : null;
            SelectedKaryawan = ComboKaryawan?.Count > 0 ? ComboKaryawan?[0] : null;

            RefreshDatabase();
        }

        public void RefreshDatabase()
        {
            Items = _userRepository.GetAll().DefaultView;
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
                    InputIdUser = SelectedItem["id_user"].ToString();
                    InputUsername = SelectedItem["username"].ToString();
                    InputPassword = SelectedItem["password"].ToString();
                    ComboLevel.Sort = "id_level";
                    SelectedLevel = ComboLevel[ComboLevel.Find(SelectedItem["id_level"])];
                    ComboKaryawan.Sort = "id_karyawan";
                    SelectedKaryawan = ComboKaryawan[ComboKaryawan.Find(SelectedItem["id_karyawan"])];
                }
            }
        }

        private string inputIdUser;
        public string InputIdUser
        {
            get { return inputIdUser; }
            set
            {
                value = value.Length > 4 ? value.Substring(0, 5) : value;
                inputIdUser = value;
                OnPropertyChanged();
                userExist = _userRepository.IsExist(inputIdUser);
            }
        }

        private bool userExist { get; set; } = false;

        private string? inputUsername;
        public string? InputUsername
        {
            get { return inputUsername; }
            set
            {
                value = value.Length > 20 ? value[..20] : value;
                inputUsername = value;
                OnPropertyChanged();
            }
        }

        private string? inputPassword;
        public string? InputPassword
        {
            get { return inputPassword; }
            set
            {
                value = value.Length > 20 ? value[..20] : value;
                inputPassword = value;
                OnPropertyChanged();
            }
        }

        private DataView comboLevel;
        public DataView ComboLevel
        {
            get { return comboLevel; }
            set
            {
                comboLevel = value;
                OnPropertyChanged();
            }
        }

        private DataRowView selectedLevel;
        public DataRowView SelectedLevel
        {
            get { return selectedLevel; }
            set
            {
                selectedLevel = value;
                OnPropertyChanged();
            }
        }

        public DataView comboKaryawan;
        public DataView ComboKaryawan
        {
            get { return comboKaryawan; }
            set
            {
                comboKaryawan = value;
                OnPropertyChanged();
            }
        }

        private DataRowView selectedKaryawan;
        public DataRowView SelectedKaryawan
        {
            get { return selectedKaryawan; }
            set
            {
                selectedKaryawan = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand FreeIdCommand { get; }

        private bool CanAdd(object obj)
        {
            return !string.IsNullOrEmpty(InputIdUser) && !string.IsNullOrEmpty(InputUsername) && !string.IsNullOrEmpty(InputPassword) && SelectedLevel != null && SelectedKaryawan != null && !userExist;
        }

        private void Add(object obj)
        {
            UserModel model = new()
            {
                id_user = InputIdUser,
                username = InputUsername,
                password = InputPassword,
                id_level = SelectedLevel["id_level"].ToString(),
                id_karyawan = SelectedKaryawan["id_karyawan"].ToString()
            };
            var result = _userRepository.Insert(model);
            if (result > 0)
            {
                ResetAll();
                MessageBox.Show("Data berhasil ditambahkan");
            }
            else
            {
                MessageBox.Show("Data gagal ditambahkan");
            }
        }

        private bool CanEdit(object obj)
        {
            return !string.IsNullOrEmpty(InputIdUser) && !string.IsNullOrEmpty(InputUsername) && !string.IsNullOrEmpty(InputPassword) && SelectedLevel != null && SelectedKaryawan != null && userExist;
        }

        private void Edit(object obj)
        {
            UserModel model = new()
            {
                id_user = InputIdUser,
                username = InputUsername,
                password = InputPassword,
                id_level = SelectedLevel["id_level"].ToString(),
                id_karyawan = SelectedKaryawan["id_karyawan"].ToString()
            };
            var result = _userRepository.Update(model);
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
            return !string.IsNullOrEmpty(InputIdUser) && userExist;
        }

        private void Delete(object obj)
        {
            UserModel model = new()
            {
                id_user = InputIdUser,
            };
            var result = _userRepository.Delete(model);
            if (result > 0)
            {
                ResetAll();
                MessageBox.Show("Data berhasil dihapus");
            }
            else
            {
                MessageBox.Show("Data gagal dihapus");
            }
        }

        public void FreeId(object obj)
        {
            InputIdUser = _userRepository.freeId;
        }
    }
}
