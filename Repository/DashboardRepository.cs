using MinimarketApp.Utilities;
using MySql.Data.MySqlClient;
using System.Data;

namespace MinimarketApp.Repository
{
    public class DashboardRepository : RepositoryBase
    {
        public string GetTotalLabaBulanIni()
        {
            float total_laba = 0;
            string sql =
                "WITH total_penjualan AS (SELECT dp.id_produk, SUM(COALESCE(detail_transaksi.kuantitas * (SELECT kuantitas_produk FROM grup_produk WHERE id_produk = dp.id_produk AND id_satuan = detail_transaksi.id_satuan), detail_transaksi.kuantitas)) AS total_kuantitas_pembelian, SUM(subtotal - diskon) AS total_subtotal_penjualan " +
                "FROM detail_transaksi INNER JOIN transaksi ON transaksi.id_transaksi = detail_transaksi.id_transaksi INNER JOIN produk dp ON dp.id_produk = detail_transaksi.id_produk WHERE MONTH(transaksi.tanggal_transaksi) = MONTH(CURRENT_DATE()) AND YEAR(transaksi.tanggal_transaksi) = YEAR(CURRENT_DATE()) GROUP BY dp.id_produk), " +
                "harga_beli_rata_rata AS (SELECT dp.id_produk, AVG(COALESCE(harga_beli / (SELECT kuantitas_produk FROM grup_produk WHERE id_produk = dtp.id_produk AND id_satuan = dtp.id_satuan), harga_beli)) AS harga_beli_rata_rata FROM detail_transaksi_pembelian dtp INNER JOIN transaksi_pembelian tp ON tp.id_transaksi_pembelian = dtp.id_transaksi_pembelian INNER JOIN produk dp ON dp.id_produk = dtp.id_produk GROUP BY dp.id_produk)" +
                "SELECT dp.id_produk, dp.total_kuantitas_pembelian AS kuantitas_terjual, dp.total_subtotal_penjualan AS subtotal_penjualan, hbr.harga_beli_rata_rata AS harga_beli, (dp.total_subtotal_penjualan - dp.total_kuantitas_pembelian * hbr.harga_beli_rata_rata) AS laba " +
                "FROM total_penjualan dp INNER JOIN harga_beli_rata_rata hbr ON dp.id_produk = hbr.id_produk;";

            MySqlParameter[] parameters = { };
            MySqlDataReader reader = ExecuteReader(sql, parameters);
            if (reader == null) return "Rp0,00";
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    if (reader.IsDBNull("laba")) return "Rp0,00";
                    total_laba += reader.GetFloat("laba");
                }
            }

            return FormattedCurrency(total_laba);
        }

        public string GetTotalPengeluaranBulanIni()
        {
            string total_pengeluaran = "Rp0,00";
            string sql = "SELECT SUM(total_harga) as total_pengeluaran FROM transaksi_pembelian WHERE MONTH(tanggal_masuk) = MONTH(CURRENT_DATE()) AND YEAR(tanggal_masuk) = YEAR(CURRENT_DATE())";
            MySqlParameter[] parameters = { };
            MySqlDataReader reader = ExecuteReader(sql, parameters);
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    if (reader.IsDBNull("total_pengeluaran")) return "Rp0,00";
                    total_pengeluaran = FormattedCurrency(reader.GetFloat("total_pengeluaran"));
                }
            }
            return total_pengeluaran;
        }

        public string GetTotalPemasukanBulanIni()
        {
            string total_pemasukan = "Rp0,00";
            string sql = "SELECT SUM(total_harga) as total_pemasukan FROM transaksi WHERE MONTH(tanggal_transaksi) = MONTH(CURRENT_DATE()) AND YEAR(tanggal_transaksi) = YEAR(CURRENT_DATE())";
            MySqlParameter[] parameters = { };
            MySqlDataReader reader = ExecuteReader(sql, parameters);
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    if (reader.IsDBNull("total_pemasukan")) return "Rp0,00";
                    total_pemasukan = FormattedCurrency(reader.GetFloat("total_pemasukan"));
                }
            }
            return total_pemasukan;
        }

        public string GetJumlahTransaksiBulanIni()
        {
            string jumlah_transaksi = "0";
            string sql = "SELECT COUNT(*) as jumlah_transaksi FROM transaksi WHERE MONTH(tanggal_transaksi) = MONTH(CURRENT_DATE()) AND YEAR(tanggal_transaksi) = YEAR(CURRENT_DATE())";
            MySqlParameter[] parameters = { };
            MySqlDataReader reader = ExecuteReader(sql, parameters);
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    if (reader.IsDBNull("jumlah_transaksi")) return "0 Transaksi";
                    jumlah_transaksi = reader.GetInt32("jumlah_transaksi").ToString();
                }
            }
            return jumlah_transaksi + " Transaksi";
        }



        public DataView GetProdukHampirHabis()
        {
            string sql = "SELECT * FROM produk INNER JOIN kategori ON kategori.id_kategori = produk.id_kategori INNER JOIN satuan ON satuan.id_satuan = produk.id_satuan WHERE stok < 10 ORDER BY produk.id_produk";
            MySqlParameter[] parameters = [];
            DataTable dt = ExecuteDataTable(sql, parameters);
            DataColumn formatted_harga_jual_column = new("formatted_harga_jual", typeof(string));
            // DataColumn formatted_harga_beli_column = new("formatted_harga_beli", typeof(string));
            dt.Columns.Add(formatted_harga_jual_column);
            // dt.Columns.Add(formatted_harga_beli_column);
            foreach (DataRow row in dt.Rows)
            {
                row["formatted_harga_jual"] = FormattedCurrency((float)row["harga_jual"]);
                // row["formatted_harga_beli"] = FormattedCurrency((float)row["harga_beli"]);
            }
            return dt.DefaultView;
        }

        public DataView GetProdukTerlarisSemingguTerakhir()
        {
            string sql = "SELECT produk.id_produk, produk.barcode, produk.nama_produk, SUM(COALESCE(detail_transaksi.kuantitas * (SELECT kuantitas_produk FROM grup_produk WHERE id_produk = produk.id_produk AND id_satuan = detail_transaksi.id_satuan), 1)) AS jumlah_terjual, detail_transaksi.id_satuan, detail_transaksi.id_satuan FROM detail_transaksi INNER JOIN produk ON produk.id_produk = detail_transaksi.id_produk INNER JOIN transaksi ON transaksi.id_transaksi = detail_transaksi.id_transaksi WHERE DATE(transaksi.tanggal_transaksi) BETWEEN DATE_SUB(CURRENT_DATE(), INTERVAL 1 WEEK) AND CURRENT_DATE() GROUP BY produk.nama_produk ORDER BY jumlah_terjual DESC LIMIT 10;";
            MySqlParameter[] parameters = { };
            DataTable dt = ExecuteDataTable(sql, parameters);
            return dt.DefaultView;
        }
    }
}
