using MinimarketApp.Utilities;
using MinimarketApp.View;
using System.Windows.Input;

namespace MinimarketApp.ViewModel
{
    internal class NavigationMenuVM : ViewModelBase
    {
        private object currentView;
        public object CurrentView
        {
            get { return currentView; }
            set 
            {
                currentView = value;
                OnPropertyChanged();
            }
        }

        public ICommand DashboardCommand { get; set; }
        public ICommand ProdukCommand { get; set; }
        public ICommand KategoriCommand { get; set; }
        public ICommand SatuanCommand { get; set; }
        public ICommand PelangganCommand { get; set; }
        public ICommand TransaksiCommand { get; set; }
        public ICommand PemasokCommand { get; set; }
        public ICommand UserCommand { get; set; }
        public ICommand LevelCommand { get; set; }
        public ICommand TransaksiPembelianCommand { get; set; }

        public NavigationMenuVM()
        {
            DashboardCommand = new RelayCommand(Dashboard);
            ProdukCommand = new RelayCommand(Produk);
            KategoriCommand = new RelayCommand(Kategori);
            SatuanCommand = new RelayCommand(Satuan);
            PelangganCommand = new RelayCommand(Pelanggan);
            TransaksiCommand = new RelayCommand(Transaksi);
            PemasokCommand = new RelayCommand(Pemasok);
            UserCommand = new RelayCommand(User);
            LevelCommand = new RelayCommand(Level);
            TransaksiPembelianCommand = new RelayCommand(TransaksiPembelian);

            CurrentView = new DashboardVM();
        }

        private void Dashboard(object obj) => CurrentView = new DashboardVM();
        private void Produk(object obj) => CurrentView = new ProdukVM();
        private void Kategori(object obj) => CurrentView = new KategoriVM();
        private void Satuan(object obj) => CurrentView = new SatuanVM();
        private void Pelanggan(object obj) => CurrentView = new PelangganVM();
        private void Transaksi(object obj) => CurrentView = new TransaksiVM();
        private void Pemasok(object obj) => CurrentView = new PemasokVM();
        private void User(object obj) => CurrentView = new UserVM();
        private void Level(object obj) => CurrentView = new LevelVM();
        private void TransaksiPembelian(object obj) => CurrentView = new TransaksiPembelianVM();

    }
}
