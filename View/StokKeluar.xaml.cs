using MinimarketApp.ViewModel;
using System.Data;
using System.Windows.Controls;
using System.Windows.Input;

namespace MinimarketApp.View
{
    public partial class StokKeluar : UserControl
    {
        public StokKeluar()
        {
            DataContext = new StokKeluarVM();
            
            InitializeComponent();
        }

        private void ComboProduk_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Escape) return;
            if (e.Key == Key.Insert)
            {
                var viewModel = DataContext as StokKeluarVM;
                if (ComboProduk.Items.Count > 0 && ComboProduk.IsDropDownOpen)
                {
                    DataRowView firstRow = viewModel.CbProduk[0];

                    viewModel.SelectedCbProduk = firstRow;
                    viewModel.CbProdukText = "";
                    System.Threading.Thread.Sleep(100);
                    viewModel.CbProdukText = firstRow["ComboProdukView"].ToString();
                }
                else
                {
                    viewModel.SelectedCbProduk = null;
                    viewModel.CbProdukText = "";
                }
            }
            else if (!ComboProduk.IsDropDownOpen)
            {
                // RefreshDropdown(sender);
            }
        }

        private void ComboProduk_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            var viewModel = DataContext as StokKeluarVM;
            if (e.Key == Key.Enter)
            {
                if (ComboProduk.Items.Count > 0)
                {
                    if (ComboProduk.IsDropDownOpen)
                    {
                        ComboProduk_PreviewKeyDown(sender, e);
                    }
                    DataRowView firstRow = viewModel.CbProduk[0];

                    viewModel.SelectedCbProduk = firstRow;
                    viewModel.CbProdukText = "";
                    viewModel.CbProdukText = firstRow["ComboProdukView"].ToString();
                }

            }
            if (viewModel != null)
            {
                if (!ComboProduk.IsFocused)
                {
                    // ComboProduk.Focus();
                }
                ComboProduk.IsDropDownOpen = true;
            }
        }

        private void ComboProduk_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void RefreshDropdown(object sender)
        {
            var comboBox = sender as ComboBox;
            comboBox.Focus();
            comboBox.IsDropDownOpen = true;
        }
    }
}
