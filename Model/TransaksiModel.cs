using MinimarketApp.Model;
using MinimarketApp.Repository;
using MinimarketApp.View;
using MinimarketApp.ViewModel;
using System.Collections.ObjectModel;
using System.Data;
using System.Windows;

public class TransaksiModel : TransaksiVM
{
    public string _id_transaksi { get; set; }
    public string id_transaksi
    {
        get { return _id_transaksi; }
        set
        {
            if (value != null)
            {
                _id_transaksi = value;
                OnPropertyChanged();
            }
        }
    }
    public int? id_detail_transaksi { get; set; }
    public string? barcode { get; set; }
    public string? id_user { get; set; }
    public string? username { get; set; }
    public string? telp_pelanggan { get; set; }
    public string? nama_pelanggan { get; set; }
    public string? id_produk { get; set; }
    public ObservableCollection<DiskonModel>? diskons { get; set; }
    public string? nama_produk { get; set; }
    public DateTime? tanggal_transaksi { get; set; }
    public string? formatted_tanggal_transaksi { get; set; }
    public float? harga_jual
    {
        get
        {
            if (Satuans != null && Satuans.Count > 0)
            {
                DataRow[] rows = Satuans.Table.Select("id_satuan = '" + id_satuan + "'");
                if (rows.Length > 0)
                {
                    return (float)ParseFloat(rows[0]["harga_jual"].ToString());
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }
    }
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
            OnPropertyChanged(nameof(harga_jual));
            OnPropertyChanged(nameof(formatted_harga_jual));
            OnPropertyChanged(nameof(kuantitas));
            OnPropertyChanged(nameof(subtotal));
            OnPropertyChanged(nameof(formatted_subtotal));
            OnPropertyChanged(nameof(harga_diskon));
            OnPropertyChanged(nameof(formatted_harga_diskon));
            OnPropertyChanged(nameof(stok));
        }
    }

    public string? formatted_harga_jual { get => FloatToCurrency((float)harga_jual); }
    
    private int? _kuantitas;
    public int? kuantitas
    {
        get { return _kuantitas; }
        set
        {
            if (value == null) return;
            value = value < 1 ? 1 : value;
            if (value > 0)
            {
                _kuantitas = ParseInt(value.ToString());
                OnPropertyChanged();
                OnPropertyChanged(nameof(subtotal));
                OnPropertyChanged(nameof(formatted_subtotal));
                OnPropertyChanged(nameof(kuantitas));
                OnPropertyChanged(nameof(subtotal));
                OnPropertyChanged(nameof(formatted_subtotal));
                OnPropertyChanged(nameof(harga_diskon));
                OnPropertyChanged(nameof(formatted_harga_diskon));
            }
        }
    }
    private bool _diskonAktif { get; set; } = false;
    public bool diskonAktif
    {
        get { return _diskonAktif; }
        set
        {
            _diskonAktif = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(harga_diskon));
            OnPropertyChanged(nameof(formatted_harga_diskon));
        }
    }

    public float harga_diskon
    {
        get
        {
            if (!diskonAktif) return 0;
            float per_diskon = 0;
            float total_diskon = 0;
            int multiplier = 0;
            if (diskons != null && diskons.Count > 0)
            {
                foreach (var diskon in diskons)
                {
                    if (diskon.id_satuan != id_satuan) continue;

                    if (diskon != null)
                    {
                        if (diskon.jenis_diskon == "Harga")
                        {
                            multiplier = (int)(subtotal / diskon.total);
                        }
                        else if (diskon.jenis_diskon == "Kuantitas")
                        {
                            multiplier = (int)(kuantitas / diskon.total);
                        }

                        if (diskon.pilihan_diskon == "Diskon Rupiah")
                        {
                            per_diskon = diskon.nilai;
                            total_diskon += (float)(per_diskon * multiplier);
                        }
                        else if (diskon.pilihan_diskon == "Diskon Persen")
                        {
                            per_diskon = (float)(diskon.nilai / 100);
                            total_diskon += (float)(per_diskon * (harga_jual * multiplier));
                        }
                        else if (diskon.pilihan_diskon == "Diskon Ekstra")
                        {
                            total_diskon += (float)(harga_jual * multiplier);
                        }
                        else
                        {
                            return 0;
                        }

                    }
                }
            }
            return total_diskon;
        }
    }
    public string? formatted_harga_diskon { get { return FloatToCurrency(harga_diskon); } }
    public string? nama_satuan { get; set; }
    public float? subtotal
    {
        get
        {
            return (float)((float)harga_jual * kuantitas);
        }
    }
    public string? formatted_subtotal { get { return FloatToCurrency((float)subtotal); } }
    private float? _total_harga { get; set; } = 0;
    public float? total_harga
    {
        get { return _total_harga; }
        set
        {
            _total_harga = ParseFloat(value.ToString());
            OnPropertyChanged();
            OnPropertyChanged(nameof(formatted_total_harga));
            OnPropertyChanged(nameof(kembalian));
            OnPropertyChanged(nameof(formatted_kembalian));
        }
    }
    public string formatted_total_harga { get { return FloatToCurrency((float)total_harga); } }
    private float? _pembayaran { get; set; } = 0;
    public float? pembayaran
    {
        get { return _pembayaran; }
        set
        {
            _pembayaran = ParseFloat(value.ToString());
            OnPropertyChanged();
            OnPropertyChanged(nameof(formatted_pembayaran));
            OnPropertyChanged(nameof(kembalian));
            OnPropertyChanged(nameof(formatted_kembalian));
        }
    }
    public string? formatted_pembayaran { get; set; }
    public float? kembalian { get { return (float)(pembayaran - total_harga); } }
    public string? formatted_kembalian { get { return FloatToCurrency((float)kembalian); } }
    public string? status_transaksi { get; set; }
    public int? stok
    {
        get
        {
            if (Satuans != null && Satuans.Count > 0)
            {
                DataRow[] rows = Satuans.Table.Select("id_satuan = '" + id_satuan + "'");
                if (rows.Length > 0)
                {
                    return (int)ParseInt(rows[0]["stok"].ToString());
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }
    }
    public bool isTransaksiLocked { get; set; } = false;
}
