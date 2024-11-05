using MinimarketApp.ViewModel;

namespace MinimarketApp.Model
{
    public class StokKeluarModel : StokKeluarVM
    {
        public string id_stok_keluar { get; set; }
        public string? id_produk { get; set; }
        public int? kuantitas { get; set; }
        public DateTime? tanggal_keluar { get; set; }
        public string? formatted_tanggal_keluar { get; set; }
        public string? keterangan { get; set; }
    }
}
