using MinimarketApp.ViewModel;

namespace MinimarketApp.Model
{
    public class DiskonModel : DiskonVM
    {
        public string id_diskon { get; set; }
        public string id_produk { get; set; }
        public string nama_produk { get; set; }
        public string jenis_diskon { get; set; }
        public string id_satuan { get; set; }
        private float _total { get; set; }
        public float total
        {
            get { return _total; }
            set
            {
                _total = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(formatted_total));
            }
        }
        public string formatted_total { get => FormattedCurrency(total); }
        public string pilihan_diskon { get; set; }
        private float _nilai { get; set; }
        public float nilai
        {
            get { return _nilai; }
            set
            {
                _nilai = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(formatted_nilai));
            }
        }
        public string formatted_nilai { get => FormattedCurrency(nilai); }
        private DateTime _tanggal_mulai { get; set; }
        public DateTime tanggal_mulai
        {
            get { return _tanggal_mulai; }
            set
            {
                _tanggal_mulai = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(formatted_tanggal_mulai));
            }
        }
        public string formatted_tanggal_mulai { get => FormattedDate(tanggal_mulai); }
        private DateTime _tanggal_berakhir { get; set; }
        public DateTime tanggal_berakhir
        {
            get { return _tanggal_berakhir; }
            set
            {
                _tanggal_berakhir = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(formatted_tanggal_akhir));
            }
        }
        public string formatted_tanggal_akhir { get => FormattedDate(tanggal_berakhir); }

        public string? keterangan { get; set; }

    }
}
