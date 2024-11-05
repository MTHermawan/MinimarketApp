using MinimarketApp.ViewModel;
using System.Windows.Controls;

namespace MinimarketApp.View
{
    public partial class Karyawan : UserControl
    {
        public Karyawan()
        {
            DataContext = new KaryawanVM();

            InitializeComponent();
        }
    }
}
