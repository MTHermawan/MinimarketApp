namespace MinimarketApp.Model
{
    public class ProdukModel
    {
        public string? id_produk { get; set; }
        // public int id_detail_produk { get; set; }
        public string? nama_produk { get; set; }
        public string? id_kategori { get; set; }
        public string? nama_kategori { get; set; }
        public string? id_satuan { get; set; }
        public string? nama_satuan { get; set; }
        public float? harga_jual { get; set; }
        public string? formatted_harga_jual { get; set; }
        // public float? harga_beli { get; set; }
        // public string? formatted_harga_beli { get; set; }
        public string? status { get; set; }
        public string? barcode { get; set; }
        public DateTime? tanggal_kadaluarsa { get; set; }
        public string? formatted_tanggal_kadaluarsa { get; set; }
        public int? stok_kuantitas { get; set; }
    }
}
