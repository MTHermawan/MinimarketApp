using MinimarketApp.ViewModel;
using System.Data;
using System.Diagnostics;

namespace MinimarketApp.Model
{
    public class TransaksiPembelianModel : TransaksiPembelianVM
    {
        public string id_transaksi_pembelian { get; set; }
        public int? id_detail_transaksi_pembelian { get; set; }
        public string? id_pemasok { get; set; }
        public string? nama_pemasok { get; set; }
        public string? id_produk { get; set; }
        public string? nama_produk { get; set; }
        public string? barcode { get; set; }
        public string? nama_satuan { get; set; }
        public DateTime? tanggal_masuk { get; set; }
        public string? formatted_tanggal_masuk { get; set; }
        private DataView _satuans { get; set; }
        public DataView Satuans
        {
            get { return _satuans; }
            set
            {
                _satuans = value;
                OnPropertyChanged();
                if (Satuans != null && Satuans.Count > 0)
                {
                    Satuans.Sort = "id_satuan";
                    id_satuan = Satuans[0][0].ToString();
                }
            }
        }
        private string? _id_satuan { get; set; }
        public string? id_satuan
        {
            get { return _id_satuan; }
            set
            {
                _id_satuan = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(kuantitas));
                OnPropertyChanged(nameof(subtotal));
                OnPropertyChanged(nameof(formatted_subtotal));
            }
        }
        private int? _kuantitas;
        public int? kuantitas
        {
            get { return _kuantitas; }
            set
            {
                if (value == null) return;
                value = value < 1 ? 1 : value;
                _kuantitas = ParseInt(value.ToString());
                OnPropertyChanged();
                OnPropertyChanged(nameof(subtotal));
                OnPropertyChanged(nameof(formatted_subtotal));
            }
        }
        private float? _harga_beli { get; set; }
        public float? harga_beli
        {
            get { return _harga_beli; }
            set
            {
                _harga_beli = ParseFloat(value.ToString());
                OnPropertyChanged();
                OnPropertyChanged(nameof(formatted_harga_beli));
                OnPropertyChanged(nameof(subtotal));
                OnPropertyChanged(nameof(formatted_subtotal));
            }
        }

        public string? formatted_harga_beli { get { return FormattedCurrency((float)harga_beli); } }
        public float? harga_jual { get; set; }
        public string? formatted_harga_jual { get; set; }
        public float? total_harga { get; set; } 
        public string? formatted_total_harga { get; set; }
        public float? pembayaran { get; set; }
        public float? subtotal { get { return (float)((float)harga_beli * kuantitas); } }
        public string? formatted_subtotal { get { return FormattedCurrency((float)subtotal); } }
    }
}
