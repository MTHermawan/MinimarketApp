using MinimarketApp.Model;
using MinimarketApp.Repository;
using MinimarketApp.Utilities;
using System.Data;
using System.Windows;
using System.Windows.Input;

namespace MinimarketApp.ViewModel
{
    internal class PemasokVM : ViewModelBase
    {
        private PemasokRepository _pemasokRepository = new();

        public PemasokVM()
        {
            ResetAll();
            AddCommand = new RelayCommand(Add, CanAdd);
            EditCommand = new RelayCommand(Edit, CanEdit);
            DeleteCommand = new RelayCommand(Delete, CanDelete);
        }

        public void RefreshDatabase()
        {
            Items = _pemasokRepository.GetAll().DefaultView;
        }

        public void ResetAll()
        {
            InputIdPemasok = _pemasokRepository.freeId;
            InputNamaPemasok = "";
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
                    InputIdPemasok = SelectedItem["id_pemasok"].ToString();
                    InputNamaPemasok = SelectedItem["nama_pemasok"].ToString();
                    InputAlamat = SelectedItem["alamat"].ToString();
                    InputNomorTelepon = SelectedItem["nomor_telepon"].ToString();
                }
            }
        }

        private string? inputIdPemasok;
        public string? InputIdPemasok
        {
            get { return inputIdPemasok; }
            set
            {
                value = value.Length > 7 ? value.Substring(0, 7) : value;
                inputIdPemasok = value;
                OnPropertyChanged();
                pemasokExist = _pemasokRepository.IsExist(inputIdPemasok);
            }
        }

        private bool pemasokExist { get; set; } = false;

        private string? inputNamaPemasok;
        public string? InputNamaPemasok
        {
            get { return inputNamaPemasok; }
            set
            {
                value = value.Length > 40 ? value.Substring(0, 40) : value;
                inputNamaPemasok = value;
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
            }
        }

        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand FreeIdCommand { get; }

        private bool CanAdd(object obj)
        {
            return !string.IsNullOrEmpty(InputIdPemasok) && !string.IsNullOrEmpty(InputNamaPemasok) && !string.IsNullOrEmpty(InputAlamat) && !string.IsNullOrEmpty(InputNomorTelepon) && !pemasokExist;
        }

        private void Add(object obj)
        {
            PemasokModel model = new()
            {
                id_pemasok = InputIdPemasok,
                nama_pemasok = InputNamaPemasok,
                alamat = InputAlamat,
                nomor_telepon = InputNomorTelepon
            };

            var result = _pemasokRepository.Insert(model);
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
            return !string.IsNullOrEmpty(InputIdPemasok) && !string.IsNullOrEmpty(InputNamaPemasok) && !string.IsNullOrEmpty(InputAlamat) && !string.IsNullOrEmpty(InputNomorTelepon) && pemasokExist;
        }

        private void Edit(object obj)
        {
            PemasokModel model = new()
            {
                id_pemasok = InputIdPemasok,
                nama_pemasok = InputNamaPemasok,
                alamat = InputAlamat,
                nomor_telepon = InputNomorTelepon
            };

            var result = _pemasokRepository.Update(model);
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
            return !string.IsNullOrEmpty(InputIdPemasok) && pemasokExist;
        }

        private void Delete(object obj)
        {
            PemasokModel model = new()
            {
                id_pemasok = InputIdPemasok,
            };
            var result = _pemasokRepository.Delete(model);
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
            InputIdPemasok = _pemasokRepository.freeId;
        }
    }
}
