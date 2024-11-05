using System.Windows;
using MinimarketApp.Services;
using MinimarketApp.Utilities;
using System.Windows.Input;
using System.Diagnostics;
using MinimarketApp.View;

namespace MinimarketApp.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private INavigationService _navigation;
        public INavigationService Navigation
        {
            get => _navigation;
            set
            {
                _navigation = value;
                OnPropertyChanged();
            }
        }

        public ICommand NavigateToDashboardCommand { get; set; }
        public ICommand NavigateToProdukCommand { get; set; }
        public ICommand NavigateToPelangganCommand { get; set; }
        public ICommand NavigateToTransaksiCommand { get; set; }
        public ICommand NavigateToRiwayatPenjualanCommand { get; set; }
        public ICommand NavigateToTransaksiPembelianCommand { get; set; }
        public ICommand NavigateToRiwayatPembelianCommand { get; set; }
        public ICommand NavigateToKategoriCommand { get; set; }
        public ICommand NavigateToSatuanCommand { get; set; }
        public ICommand NavigateToPemasokCommand { get; set; }
        public ICommand NavigateToLevelCommand { get; set; }
        public ICommand NavigateToKaryawanCommand { get; set; }
        public ICommand NavigateToUserCommand { get; set; }
        public ICommand NavigateToStokKeluarCommand { get; set; }
        public ICommand NavigateToDiskonCommand { get; set; }
        public ICommand NavigateToGrupProdukCommand { get; set; }

        public MainViewModel(INavigationService navService)
        {
            Navigation = navService;
            MainView = true;
            NavigateToDashboardCommand = new RelayCommand(o => Navigation.NavigateTo<DashboardVM>(), o => true);
            NavigateToProdukCommand = new RelayCommand(o => Navigation.NavigateTo<ProdukVM>(), o => true);
            NavigateToKategoriCommand = new RelayCommand(o => Navigation.NavigateTo<KategoriVM>(), o => true);
            NavigateToSatuanCommand = new RelayCommand(o => Navigation.NavigateTo<SatuanVM>(), o => true);
            NavigateToPelangganCommand = new RelayCommand(o => Navigation.NavigateTo<PelangganVM>(), o => true);
            NavigateToTransaksiCommand = new RelayCommand(o => Navigation.NavigateTo<TransaksiVM>(), o => true);
            NavigateToRiwayatPenjualanCommand = new RelayCommand(o => Navigation.NavigateTo<RiwayatPenjualanVM>(), o => true);
            NavigateToTransaksiPembelianCommand = new RelayCommand(o => Navigation.NavigateTo<TransaksiPembelianVM>(), o => true);
            NavigateToRiwayatPembelianCommand = new RelayCommand(o => Navigation.NavigateTo<RiwayatPembelianVM>(), o => true);
            NavigateToPemasokCommand = new RelayCommand(o => Navigation.NavigateTo<PemasokVM>(), o => true);
            NavigateToLevelCommand = new RelayCommand(o => Navigation.NavigateTo<LevelVM>(), o => true);
            NavigateToKaryawanCommand = new RelayCommand(o => Navigation.NavigateTo<KaryawanVM>(), o => true);
            NavigateToUserCommand = new RelayCommand(o => Navigation.NavigateTo<UserVM>(), o => true);
            NavigateToStokKeluarCommand = new RelayCommand(o => Navigation.NavigateTo<StokKeluarVM>(), o => true);
            NavigateToDiskonCommand = new RelayCommand(o => Navigation.NavigateTo<DiskonVM>(), o => true);
            NavigateToGrupProdukCommand = new RelayCommand(o => Navigation.NavigateTo<GrupProdukVM>(), o => true);

            Logout = new RelayCommand(LogoutAction);

            if (Session.IsAdmin())
            {
                AdminVisibility = Visibility.Visible;
            }
        }

        private bool _mainView;
        public bool MainView
        {
            get => _mainView;
            set
            {
                _mainView = value;
                OnPropertyChanged();
                Navigation.NavigateTo<DashboardVM>();
            }
        }

        public ICommand Logout { get; set; }

        public void LogoutAction(object obj)
        {
            ((App)Application.Current).Logout();
        }

        public void NavigateToTransaksi(string idTransaksi)
        {
            Session.EditedIdTransaksi = idTransaksi;
            Session.isLoadedTransaksi = true;
            IsTransaksi = true;
            Navigation.NavigateTo<TransaksiVM>();
        }

        internal void NavigateToTransaksiPembelian(string? idTransaksiPembelian)
        {
            Session.EditedIdTransaksiPembelian = idTransaksiPembelian;
            Session.isLoadedTransaksiPembelian = true;
            IsTransaksiPembelian = true;
            Navigation.NavigateTo<TransaksiPembelianVM>();
        }

        private bool _isTransaksi;
        public bool IsTransaksi
        {
            get => _isTransaksi;
            set
            {
                _isTransaksi = value;
                OnPropertyChanged();
            }
        }
        
        private bool _isTransaksiPembelian;
        public bool IsTransaksiPembelian
        {
            get => _isTransaksiPembelian;
            set
            {
                _isTransaksiPembelian = value;
                OnPropertyChanged();
            }
        }

        private Visibility _adminVisibility { get; set; } = Visibility.Collapsed;
        public Visibility AdminVisibility
        {
            get => _adminVisibility;
            set
            {
                _adminVisibility = value;
                OnPropertyChanged();
            }
        }
    }
}
