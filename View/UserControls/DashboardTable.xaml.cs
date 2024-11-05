using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace MinimarketApp.View.UserControls
{
    public partial class DashboardTable : UserControl
    {
        /*public string CurrentTable
        {
            get { return (string)GetValue(CurrentTableProperty); }
            set { SetValue(CurrentTableProperty, value); }
        }

        public static readonly DependencyProperty CurrentTableProperty =
            DependencyProperty.Register("CurrentTable", typeof(string), typeof(DashboardTable), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, CurrentTablePropertyChanged, null, false, UpdateSourceTrigger.PropertyChanged));

        private static void CurrentTablePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == null)
            {
                return;
            }
            var control = (DashboardTable)d;
            control.CurrentTable = (string)e.NewValue;
            if (control.CurrentTable != null)
            {
                List<DataGrid> tables = new List<DataGrid>();
                foreach (var item in control.FindVisualChildren<DataGrid>(control))
                    tables?.Add(item);

                *//*foreach (var item in tables)
                    item.Visibility = (item.Name == control.CurrentTable) ? Visibility.Visible : Visibility.Collapsed;*//*
            }
        }

        public IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj == null) yield return (T)Enumerable.Empty<T>();
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
            {
                DependencyObject ithChild = VisualTreeHelper.GetChild(depObj, i);
                if (ithChild == null) continue;
                if (ithChild is T t) yield return t;
                foreach (T childOfChild in FindVisualChildren<T>(ithChild)) yield return childOfChild;
            }
        }

        public DataView GetDataGridNames()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("Value", typeof(string));
            dt.Rows.Add("Stok Hampir Habis", "StokHampirHabis");
            dt.Rows.Add("Produk Terlaris", "ProdukTerlaris");
            return dt.DefaultView;
        }*/

        /*private void cmbTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbTable.SelectedValue != null)
            {
                CurrentTable = cmbTable.SelectedValue.ToString();
            }
        }*/
    }
}
