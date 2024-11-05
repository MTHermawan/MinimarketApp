using MinimarketApp.ViewModel;
using System.Windows.Controls;

namespace MinimarketApp.View
{
    public partial class Satuan : UserControl
    {
        public Satuan()
        {
            DataContext = new SatuanVM();

            InitializeComponent();
        }
    }
}
