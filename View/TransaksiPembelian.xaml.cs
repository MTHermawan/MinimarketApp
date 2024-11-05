using MinimarketApp.Repository;
using MinimarketApp.ViewModel;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MinimarketApp.View
{
    public partial class TransaksiPembelian : UserControl
    {
        private ProdukRepository _produkRepository = new();

        public TransaksiPembelian()
        {
            DataContext = new TransaksiPembelianVM();

            InitializeComponent();
        }

        private void Combo_GotFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            RefreshDropdown(sender);
        }

        private void ComboProduk_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape) return;
            if (e.Key == Key.Insert)
            {
                var viewModel = DataContext as TransaksiPembelianVM;
                if (ComboProduk.Items.Count > 0 && ComboProduk.IsDropDownOpen)
                {
                    DataRowView firstRow = viewModel.CbProduk[0];

                    viewModel.SelectedCbProduk = firstRow;
                    viewModel.CbProdukText = "";
                    System.Threading.Thread.Sleep(100);
                    viewModel.CbProdukText = firstRow["ComboProdukView"].ToString();
                    RefreshDropdown(sender, e);
                    viewModel.AddRowCommand.Execute(null);
                }
            }
            else if (!ComboProduk.IsDropDownOpen)
            {
                RefreshDropdown(sender);
            }
        }

        private void RefreshDropdown(object sender)
        {
            var comboBox = sender as ComboBox;
            comboBox.Focus();
            comboBox.IsDropDownOpen = true;
        }

        private void RefreshDropdown(object sender, KeyEventArgs e)
        {
            var comboBox = sender as ComboBox;
            comboBox.Focus();
            comboBox.IsDropDownOpen = true;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as TransaksiPembelianVM;

            if (viewModel != null && Session.isLoadedTransaksiPembelian)
            {
                viewModel.ContinueTransaksi(Session.EditedIdTransaksiPembelian);
                Session.isLoadedTransaksiPembelian = false;
            }

            ComboProduk.Focus();
        }

        private void ComboSupplier_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape) return;
            if (e.Key == Key.Insert)
            {
                var viewModel = DataContext as TransaksiPembelianVM;
                if (ComboSupplier.Items.Count > 0)
                {
                    DataRowView firstRow = viewModel.CbPemasok[0];

                    viewModel.SelectedCbPemasok = firstRow;
                    viewModel.CbPemasokText = "";
                    System.Threading.Thread.Sleep(100);
                    viewModel.CbPemasokText = firstRow["ComboPemasokView"].ToString();
                }
            }
            else if (!ComboSupplier.IsDropDownOpen)
            {
                RefreshDropdown(sender);
            }
        }

        private void DataTransaksi_TargetUpdated(object sender, System.Windows.Data.DataTransferEventArgs e)
        {
            var viewModel = DataContext as TransaksiPembelianVM;
            if (viewModel != null)
            {
                viewModel.RefreshAutoInput();
            }
        }

        private void ComboProduk_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            var viewModel = DataContext as TransaksiPembelianVM;
            if (e.Key == Key.RightShift || e.Key == Key.Enter)
            {
                MessageBox.Show(e.Key.ToString());
                if (ComboProduk.Items.Count > 0)
                {
                    if (ComboProduk.IsDropDownOpen)
                    {
                        MessageBox.Show("Please select the product first");
                        // ComboProduk.IsDropDownOpen = false;
                        ComboProduk_PreviewKeyDown(sender, e);
                    }
                    DataRowView firstRow = viewModel.CbProduk[0];

                    viewModel.SelectedCbProduk = firstRow;
                    viewModel.CbProdukText = "";
                    viewModel.CbProdukText = firstRow["ComboProdukView"].ToString();
                    viewModel.AddRowCommand.Execute(null);
                }
            }
            if (viewModel != null)
            {
                if (!ComboProduk.IsFocused)
                {
                    //ComboProduk.Focus();
                }
                ComboProduk.IsDropDownOpen = true;
            }
        }

        private void ComboProduk_TextChanged(object sender, TextChangedEventArgs e)
        {
            var viewModel = DataContext as TransaksiPembelianVM;
            if (viewModel != null)
            {
                if (!ComboProduk.IsFocused)
                {
                    //ComboProduk.Focus();
                }
                ComboProduk.IsDropDownOpen = viewModel.IsTransaksiLocked ? false : true;
            }
        }

        private void ComboSupplier_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            var viewModel = DataContext as TransaksiPembelianVM;
            if (e.Key == Key.RightShift || e.Key == Key.Enter)
            {
                MessageBox.Show(e.Key.ToString());
                if (ComboSupplier.Items.Count > 0)
                {
                    if (ComboSupplier.IsDropDownOpen)
                    {
                        ComboSupplier_PreviewKeyDown(sender, e);
                    }
                    DataRowView firstRow = viewModel.CbPemasok[0];

                    viewModel.SelectedCbPemasok = firstRow;
                    viewModel.CbPemasokText = "";
                    viewModel.CbPemasokText = firstRow["ComboSupplierView"].ToString();
                }
            }
            if (viewModel != null)
            {
                if (!ComboSupplier.IsFocused)
                {
                    //ComboProduk.Focus();
                }
                ComboSupplier.IsDropDownOpen = true;
            }
        }

        private void ComboSupplier_TextChanged(object sender, TextChangedEventArgs e)
        {
            var viewModel = DataContext as TransaksiPembelianVM;
            if (viewModel != null)
            {
                if (!ComboSupplier.IsFocused)
                {
                    //ComboSupplier.Focus();
                }
                ComboSupplier.IsDropDownOpen = viewModel.IsTransaksiLocked ? false : true;
            }
        }

        private void ComboSupplier_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
