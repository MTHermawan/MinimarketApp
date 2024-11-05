using MinimarketApp.ViewModel;

namespace MinimarketApp.Model
{
    public class GrupProdukModel : GrupProdukVM
    {
        public string id_grup_produk { get; set; }
        public ProdukModel produk { get; set; }
        public SatuanModel satuan { get; set; }
        public string barcode_unit { get; set; }
        public int kuantitas_produk { get; set; }
        public float harga_jual_unit { get; set; }
    }
}
