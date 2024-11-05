using MinimarketApp.ViewModel;
using System.Windows.Controls;

namespace MinimarketApp.View
{
    public partial class Level : UserControl
    {
        public Level()
        {
            DataContext = new LevelVM();

            InitializeComponent();
        }
    }
}
