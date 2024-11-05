using MinimarketApp.Model;
using MinimarketApp.Repository;
using MinimarketApp.Utilities;
using System.Data;
using System.Windows;
using System.Windows.Input;

namespace MinimarketApp.ViewModel
{
    internal class PelangganVM : ViewModelBase
    {
        private PelangganRepository _pelangganRepository = new();

        public PelangganVM()
        {
            ResetAll();

            AddCommand = new RelayCommand(Add, CanAdd);
            EditCommand = new RelayCommand(Edit, CanEdit);
            DeleteCommand = new RelayCommand(Delete, CanDelete);
        }

        public void RefreshDatabase()
        {
            Items = _pelangganRepository.GetAll().DefaultView;
        }

        public void ResetAll()
        {
            InputNamaPelanggan = "";
            InputAlamat = "";
            InputNomorTelepon = "";
            ComboKelamin = _pelangganRepository.GetJenisKelamin();
            SelectedKelamin = ComboKelamin?.Count > 0 ? ComboKelamin?[0] : null;

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
                    InputNomorTelepon = SelectedItem["telp_pelanggan"].ToString();
                    InputNamaPelanggan = SelectedItem["nama_pelanggan"].ToString();
                    InputAlamat = SelectedItem["alamat"].ToString();
                    SelectedKelamin = SelectedItem["jenis_kelamin"].ToString();
                }
            }
        }

        public bool pelangganExist { get; set; } = false;

        private string? inputNamaPelanggan;
        public string? InputNamaPelanggan
        {
            get { return inputNamaPelanggan; }
            set
            {
                value = value.Length > 50 ? value.Substring(0, 50) : value;
                inputNamaPelanggan = value;
                OnPropertyChanged();
            }
        }

        private string? inputAlamat;
        public string? InputAlamat
        {
            get { return inputAlamat; }
            set
            {
                value = value.Length > 100 ? value.Substring(0, 100) : value;
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
                value = value.Length > 13 ? value.Substring(0, 13) : value;
                inputNomorTelepon = value;
                OnPropertyChanged();
                pelangganExist = _pelangganRepository.IsExist(value);
            }
        }

        private List<string> comboKelamin;
        public List<string> ComboKelamin
        {
            get { return comboKelamin; }
            set
            {
                comboKelamin = value;
                OnPropertyChanged();
            }
        }

        private string selectedKelamin;
        public string SelectedKelamin
        {
            get { return selectedKelamin; }
            set
            {
                selectedKelamin = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }

        private bool CanAdd(object obj)
        {
            return !string.IsNullOrEmpty(InputNamaPelanggan) && !string.IsNullOrEmpty(InputAlamat) && !string.IsNullOrEmpty(InputNomorTelepon) && SelectedKelamin != null && !pelangganExist;
        }

        private void Add(object obj)
        {
            PelangganModel model = new()
            {
                nama_pelanggan = InputNamaPelanggan,
                alamat = InputAlamat,
                telp_pelanggan = InputNomorTelepon,
                jenis_kelamin = SelectedKelamin
            };
            var result = _pelangganRepository.Insert(model);
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
            return !string.IsNullOrEmpty(InputNamaPelanggan) && !string.IsNullOrEmpty(InputAlamat) && !string.IsNullOrEmpty(InputNomorTelepon) && SelectedKelamin != null && pelangganExist;
        }

        private void Edit(object obj)
        {
            PelangganModel model = new()
            {
                nama_pelanggan = InputNamaPelanggan,
                alamat = InputAlamat,
                telp_pelanggan = InputNomorTelepon,
                jenis_kelamin = SelectedKelamin
            };
            var result = _pelangganRepository.Update(model);
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
            return pelangganExist;
        }

        private void Delete(object obj)
        {
            PelangganModel model = new()
            {
                telp_pelanggan = InputNomorTelepon
            };
            var result = _pelangganRepository.Delete(model);
            if (result > 0)
            {
                ResetAll();
            }
            else
            {
                MessageBox.Show("Data gagal dihapus");
            }
        }
    }
}
