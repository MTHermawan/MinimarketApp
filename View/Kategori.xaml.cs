using System.Windows.Controls;
using MinimarketApp.ViewModel;

namespace MinimarketApp.View
{
    public partial class Kategori : UserControl
    {
        public Kategori()
        {
            DataContext = new KategoriVM();

            InitializeComponent();
        }
    }
}
