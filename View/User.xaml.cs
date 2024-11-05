using System.Windows.Controls;
using MinimarketApp.ViewModel;

namespace MinimarketApp.View
{
    public partial class User : UserControl
    {
        public User()
        {
            DataContext = new UserVM();

            InitializeComponent();
        }

        private void StackPanel_Scroll(object sender, System.Windows.Controls.Primitives.ScrollEventArgs e)
        {

        }
    }
}
