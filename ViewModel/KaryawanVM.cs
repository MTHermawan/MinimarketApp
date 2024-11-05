using MinimarketApp.Model;
using MinimarketApp.Repository;
using MinimarketApp.Utilities;
using System.Data;
using System.Windows;
using System.Windows.Input;

namespace MinimarketApp.ViewModel
{
    internal class KaryawanVM : ViewModelBase
    {
        private KaryawanRepository _karyawanRepository = new();

        public KaryawanVM()
        {
            ResetAll();

            AddCommand = new RelayCommand(Add, CanAdd);
            EditCommand = new RelayCommand(Edit, CanEdit);
            DeleteCommand = new RelayCommand(Delete, CanDelete);
            FreeIdCommand = new RelayCommand(FreeId);
        }

        public void RefreshDatabase()
        {
            Items = _karyawanRepository.GetAll().DefaultView;
        }

        public void ResetAll()
        {
            InputIdKaryawan = _karyawanRepository.freeId;
            InputNamaKaryawan = "";
            InputAlamat = "";
            InputNomorTelepon = "";

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
                    InputIdKaryawan = SelectedItem["id_karyawan"].ToString();
                    InputNamaKaryawan = SelectedItem["nama_karyawan"].ToString();
                }
            }
        }

        private string? inputIdKaryawan;
        public string? InputIdKaryawan
        {
            get { return inputIdKaryawan; }
            set
            {
                value = value.Length > 7 ? value[..20] : value;
                inputIdKaryawan = value;
                OnPropertyChanged();
                karyawanExist = _karyawanRepository.IsExist(inputIdKaryawan);
            }
        }

        private bool karyawanExist { get; set; } = false;

        private string? inputNamaKaryawan;
        public string? InputNamaKaryawan
        {
            get { return inputNamaKaryawan; }
            set
            {
                value = value.Length > 50 ? value[..50] : value;
                inputNamaKaryawan = value;
                OnPropertyChanged();
            }
        }

        private string? inputAlamat;
        public string? InputAlamat
        {
            get { return inputAlamat; }
            set
            {
                value = value.Length > 100 ? value[..100] : value;
                inputAlamat = value;
                OnPropertyChanged();
            }
        }

        private string? inputNomorTelepon;
        public string? InputNomorTelepon
        {
            get { return inputNomorTelepon; }
            set
            {
                value = value.Length > 13 ? value[..13] : value;
                inputNomorTelepon = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand FreeIdCommand { get; }

        private bool CanAdd(object obj)
        {
            return !string.IsNullOrEmpty(InputIdKaryawan) && !string.IsNullOrEmpty(InputNamaKaryawan) && !string.IsNullOrEmpty(InputAlamat) && !string.IsNullOrEmpty(InputNomorTelepon) && !karyawanExist;
        }

        private void Add(object obj)
        {
            KaryawanModel model = new()
            {
                id_karyawan = InputIdKaryawan,
                nama_karyawan = InputNamaKaryawan,
                alamat = InputAlamat,
                nomor_telepon = InputNomorTelepon
            };

            var result = _karyawanRepository.Insert(model);
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
            return !string.IsNullOrEmpty(InputIdKaryawan) && !string.IsNullOrEmpty(InputNamaKaryawan) && !string.IsNullOrEmpty(InputAlamat) && !string.IsNullOrEmpty(InputNomorTelepon) && karyawanExist;
        }

        private void Edit(object obj)
        {
            KaryawanModel model = new()
            {
                id_karyawan = InputIdKaryawan,
                nama_karyawan = InputNamaKaryawan,
                alamat = InputAlamat,
                nomor_telepon = InputNomorTelepon
            };

            var result = _karyawanRepository.Update(model);
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
            return !string.IsNullOrEmpty(InputIdKaryawan) && karyawanExist;
        }

        private void Delete(object obj)
        {
            KaryawanModel model = new()
            {
                id_karyawan = InputIdKaryawan,
            };
            var result = _karyawanRepository.Delete(model);
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
            InputIdKaryawan = _karyawanRepository.freeId;
        }
    }
}
