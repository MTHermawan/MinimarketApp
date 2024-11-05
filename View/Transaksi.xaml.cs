using MinimarketApp.ViewModel;
using PdfSharp;
using PdfSharp.Pdf;
using System.Data;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TheArtOfDev.HtmlRenderer.PdfSharp;

namespace MinimarketApp.View
{
    public partial class Transaksi : UserControl
    {
        public Transaksi()
        {
            DataContext = new TransaksiVM();

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
                var viewModel = DataContext as TransaksiVM;
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
                else
                {
                    viewModel.SelectedCbProduk = null;
                    viewModel.CbProdukText = "";
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

        private void ComboPelanggan_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape) return;
            if (e.Key == Key.Insert)
            {
                var viewModel = DataContext as TransaksiVM;
                if (ComboPelanggan.Items.Count > 0)
                {
                    DataRowView firstRow = viewModel.CbPelanggan[0];

                    viewModel.SelectedCbPelanggan = firstRow;
                    viewModel.CbPelangganText = "";
                    System.Threading.Thread.Sleep(100);
                    viewModel.CbPelangganText = firstRow["telp_pelanggan"].ToString();
                }
            }
            else if (!ComboPelanggan.IsDropDownOpen)
            {
                RefreshDropdown(sender);
            }
        }

        private void txtBoxJumlah_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void ketStok_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (false)
            {
                /*if (ketStok.Visibility == Visibility.Visible)
                {
                    txtBoxJumlah.BorderBrush = Brushes.Red;
                    txtBoxJumlah.BorderThickness = new Thickness(2);
                    txtBoxJumlah.Padding = new Thickness(0, 0, 0, 0);
                }
                else
                {
                    txtBoxJumlah.BorderBrush = Brushes.Black;
                    txtBoxJumlah.BorderThickness = new Thickness(1);
                    txtBoxJumlah.Padding = new Thickness(1, 1, 1, 1);
                }*/
            }
        }

        private void DataTransaksi_TargetUpdated(object sender, System.Windows.Data.DataTransferEventArgs e)
        {
            var viewModel = DataContext as TransaksiVM;
            if (viewModel != null)
            {
                viewModel.RefreshAutoInput();
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as TransaksiVM;

            if (viewModel != null && Session.isLoadedTransaksi)
            {
                viewModel.ContinueTransaksi(Session.EditedIdTransaksi);
                Session.isLoadedTransaksi = false;
            }

            ComboProduk.Focus();
        }

        public Visibility CollapseOnLocked
        {
            get
            {
                var viewModel = DataContext as TransaksiVM;
                return viewModel.CollapseOnLocked;
            }
        }

        private void ComboProduk_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            var viewModel = DataContext as TransaksiVM;
            if (e.Key == Key.RightShift || e.Key == Key.Enter)
            {
                MessageBox.Show(e.Key.ToString());
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
            var viewModel = DataContext as TransaksiVM;
            /*if (viewModel != null)
            {
                if (!ComboProduk.IsFocused)
                {
                    //ComboProduk.Focus();
                }
                ComboProduk.IsDropDownOpen = true;
            }*/
        }

        private void ComboPelanggan_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            var viewModel = DataContext as TransaksiVM;
            if (e.Key == Key.Enter)
            {
                MessageBox.Show(e.Key.ToString());
                if (ComboPelanggan.Items.Count > 0)
                {
                    if (ComboPelanggan.IsDropDownOpen)
                    {
                        ComboPelanggan_PreviewKeyDown(sender, e);
                    }
                    DataRowView firstRow = viewModel.CbPelanggan[0];

                    viewModel.SelectedCbPelanggan = firstRow;
                    viewModel.CbPelangganText = "";
                    viewModel.CbPelangganText = firstRow["ComboPelangganView"].ToString();
                }
            }
            if (viewModel != null)
            {
                if (!ComboPelanggan.IsFocused)
                {
                    //ComboPelanggan.Focus();
                }
                ComboPelanggan.IsDropDownOpen = true;
            }
        }

        private void ComboPelanggan_TextChanged(object sender, TextChangedEventArgs e)
        {
            /*var viewModel = DataContext as TransaksiVM;
            if (viewModel != null)
            {
                if (!ComboPelanggan.IsFocused)
                {
                    //ComboPelanggan.Focus();
                }
                ComboPelanggan.IsDropDownOpen = true;
            }*/
        }

        private void Print_CLick(object sender, RoutedEventArgs e)
        {
            string content =
                "<html>" +
                "<head>" +
                "<title>Nota Transaksi</title>" +
                "<style>" +
                "body {" +
                "font-family: Arial, sans-serif;" +
                "margin: 0;" +
                "padding: 0;" +
                "}" +
                ".container {" +
                "max-width: 600px;" +
                "margin: 20px auto;" +
                "padding: 20px;" +
                "border: 1px solid #ccc;" +
                "border-radius: 5px;" +
                "background-color: #fff;" +
                "}.header {" +
                "text-align: center;" +
                "margin-bottom: 20px;" +
                "}" +
                ".transaksi-info {" +
                "margin-bottom: 20px;" +
                "}" +
                ".produk-table {" +
                "width: 100%;" +
                "border-collapse: collapse;" +
                "margin-bottom: 20px;" +
                "}" +
                ".produk-table th," +
                ".produk-table td {" +
                "border: 1px solid #ccc;" +
                "padding: 8px;" +
                "text-align: left;" +
                "}" +
                ".produk-table th {" +
                "background-color: #f2f2f2;" +
                "}" +
                ".footer {" +
                "text-align: center;" +
                "margin-top: 20px;" +
                "}" +
                "</style>" +
                "</head>" +
                "<body>" +
                "<div class=\"container\">" +
                "<div class=\"header\">" +
                "<h2>Nota Transaksi</h2>" +
                "<p>Minimarket Guweh</p>" +
                "<p>Jalan Guweh RT. 100 kel. Anu,</p>" +
                "<p>SAMARINDA</p>" +
                "<p>Nomor Telepon: 081269793111</p>" +
                "</div>" +
                "<div class=\"transaksi-info\">" +
                "<p><strong>ID Transaksi:</strong> {{ $transaksi->id }}</p>" +
                "<p><strong>Kasir:</strong> {{ $transaksi->user->username }}</p>" +
                "<p><strong>Tanggal Transaksi:</strong> {{ $transaksi->tanggal_transaksi }}</p>" +
                "<p><strong>Pelanggan:</strong> {{ $transaksi->pelanggan->nama_pelanggan ?? 'UMUM' }}</p>" +
                "</div>" +
                "<div class=\"produk-table\">" +
                "--------------------------------------------------------------------------" +
                "<div class=\"produk-table\">" +
                "{{-- @php $counter = 1; @endphp --}}" +
                "@forelse($detail_transaksi as $detail)" +
                "<div>" +
                "<p>{{ $loop->iteration }}. {{ $detail->produk->nama_produk }}</p>" +
                "<p>{{ $detail->produk->barcode }} <span style=\"margin-right: 10px;\"></span> {{ $detail->kuantitas }}  <span style=\"margin-right: 10px;\"></span> Rp{{ number_format($detail->produk->harga_jual, 2, ',', '.') }} <span style=\"margin-right: 40px;\"></span>Rp{{ number_format($detail->subtotal, 2, ',', '.') }}</p>" +
                "</div>" +
                "@empty" +
                "<p>Tidak ada data detail transaksi.</p>" +
                "@endforelse" +
                "--------------------------------------------------------------------------" +
                "</div>" +
                "</div>" +
                "<div class=\"transaksi-info\">" +
                "<p><strong>TOTAL:</strong> Rp{{ number_format($transaksi->total_harga, 2, ',', '.') }}</p>" +
                "<p><strong>DISKON:</strong> Rp{{ number_format($transaksi->pelanggan && $transaksi->pelanggan->nama_pelanggan != null && $detail_transaksi->sum('subtotal') >= 20000 ? $detail_transaksi->sum('subtotal') * 5 / 100 : 0, 2, ',', '.') }}</p>" +
                "<p><strong>BAYAR:</strong> Rp{{ number_format($transaksi->pembayaran, 2, ',', '.') }}</p>" +
                "<p><strong>KEMBALIAN:</strong> Rp{{ number_format($transaksi->pembayaran - $transaksi->total_harga, 2, ',', '.') }}</p>" +
                "</div>" +
                "<br>" +
                "<div class=\"footer\">" +
                "<p>Terima kasih atas kunjungan Anda!</p>" +
                "<p>&copy; {{ date('Y') }} Minimarket Guweh. All rights reserved.</p>" +
                "</div>" +
                "</div>" +
                "</body>" +
                "</html>";

            // custom page size


            PdfDocument pdf = PdfGenerator.GeneratePdf(content, PageSize.A4);

            string filename = "test.pdf";
            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            pdf.Save(path + "\\" + filename);

            var psi = new ProcessStartInfo
            {
                FileName = path + "\\" + filename,
                UseShellExecute = true
            };
            Process.Start(psi);
        }

    }
}
