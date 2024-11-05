using MinimarketApp.ViewModel;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows;
using MinimarketApp.Utilities;
using Microsoft.Extensions.DependencyInjection;

namespace MinimarketApp.View
{
    public partial class RiwayatPenjualan : UserControl
    {
        public object MainDataContext { get; set; }
        public RiwayatPenjualan()
        {
            DataContext = new RiwayatPenjualanVM();
            MainDataContext = Application.Current.MainWindow.DataContext;

            InitializeComponent();
        }

        private void btnDetail_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            // popupDetail.IsOpen = !popupDetail.IsOpen;
        }

        public float MainWindowWidth => (float)((App)Application.Current).MainWindow.Width;
        public float MainWindowHeight => (float)((App)Application.Current).MainWindow.Height;

    }
}
