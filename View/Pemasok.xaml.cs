using MinimarketApp.ViewModel;
using System.Windows.Controls;

namespace MinimarketApp.View
{
    public partial class Pemasok : UserControl
    {
        public Pemasok()
        {
            DataContext = new PemasokVM();

            InitializeComponent();
        }
    }
}
