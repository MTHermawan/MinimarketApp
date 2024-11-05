using MinimarketApp.Repository;
using MinimarketApp.Utilities;
using System.Data;
using System.Windows;

namespace MinimarketApp.ViewModel
{
    public class DashboardVM : ViewModelBase
    {
        private DashboardRepository _dashboardRepository = new();

        public DashboardVM()
        {
            RefreshDatabase();
            LoadTableList();
        }

        public void RefreshDatabase()
        {
            TotalPenjualanBulanIni = _dashboardRepository.GetTotalPemasukanBulanIni();
            TotalLabaBulanIni = _dashboardRepository.GetTotalLabaBulanIni();
            JumlahTransaksiBulanIni = _dashboardRepository.GetJumlahTransaksiBulanIni();
            TotalPengeluaranBulanIni = _dashboardRepository.GetTotalPengeluaranBulanIni();

            ProdukTerlarisSemingguTerakhir = _dashboardRepository.GetProdukTerlarisSemingguTerakhir();
            StokHampirHabis = _dashboardRepository.GetProdukHampirHabis();
        }

        private string _totalPenjualanBulanIni { get; set; }
        public string TotalPenjualanBulanIni
        {
            get { return _totalPenjualanBulanIni; }
            set
            {
                _totalPenjualanBulanIni = value;
                OnPropertyChanged();
            }
        }

        private string _totalLabaBulanIni { get; set; }
        public string TotalLabaBulanIni
        {
            get { return _totalLabaBulanIni; }
            set
            {
                _totalLabaBulanIni = value;
                OnPropertyChanged();
            }
        }

        private DataView _stokHampirHabis { get; set; }
        public DataView StokHampirHabis
        {
            get { return _stokHampirHabis; }
            set
            {
                _stokHampirHabis = value;
                OnPropertyChanged();
            }
        }

        private DataView _produkTerlarisSemingguTerakhir { get; set; }
        public DataView ProdukTerlarisSemingguTerakhir
        {
            get { return _produkTerlarisSemingguTerakhir; }
            set
            {
                _produkTerlarisSemingguTerakhir = value;
                OnPropertyChanged();
            }
        }

        private string _jumlahTransaksiBulanIni;

        public string JumlahTransaksiBulanIni
        {
            get { return _jumlahTransaksiBulanIni; }
            set
            {
                _jumlahTransaksiBulanIni = value; 
                OnPropertyChanged();
            }
        }


        private DataView _tableList { get; set; }
        public DataView TableList
        {
            get { return _tableList; }
            set
            {
                _tableList = value;
                OnPropertyChanged();
            }
        }

        private DataRowView _selectedTable;
        public DataRowView SelectedTable
        {
            get { return _selectedTable; }
            set
            {
                _selectedTable = value;
                OnPropertyChanged();
                if (value != null)
                {
                    CurrentTable = value.Row["Value"].ToString();
                }
            }
        }

        private string _totalPengeluaranBulanIni { get; set; }
        public string TotalPengeluaranBulanIni
        {
            get { return _totalPengeluaranBulanIni; }
            set
            {
                _totalPengeluaranBulanIni = value;
                OnPropertyChanged();
            }
        }

        public void LoadTableList()
        {
            DataTable table = new DataTable();
            table.Columns.Add("Name", typeof(string));
            table.Columns.Add("Value", typeof(string));
            table.Rows.Add("Stok Hampir Habis", "StokHampirHabis");
            table.Rows.Add("Produk Terlaris", "ProdukTerlaris");
            TableList = table.DefaultView;

            SelectedTable = TableList[0];
        }

        private string _currentTable { get; set; }
        public string CurrentTable
        {
            get { return _currentTable; }
            set
            {
                _currentTable = value;
                OnPropertyChanged();
            }
        }
    }
}
