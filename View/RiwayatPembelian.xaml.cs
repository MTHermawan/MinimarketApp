using MinimarketApp.ViewModel;
using System.Windows;
using System.Windows.Controls;

namespace MinimarketApp.View
{
    public partial class RiwayatPembelian : UserControl
    {
        public object MainDataContext { get; set; }
        public RiwayatPembelian()
        {
            DataContext = new RiwayatPembelianVM();
            MainDataContext = Application.Current.MainWindow.DataContext;

            InitializeComponent();
        }

        private void btnDetail_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            popupDetail.IsOpen = !popupDetail.IsOpen;
        }

        public float MainWindowWidth => (float)((App)Application.Current).MainWindow.Width;
        public float MainWindowHeight => (float)((App)Application.Current).MainWindow.Height;
    }
}
