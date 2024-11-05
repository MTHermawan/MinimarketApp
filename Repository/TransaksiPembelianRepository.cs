using MinimarketApp.Model;
using MinimarketApp.Utilities;
using MySql.Data.MySqlClient;
using System.Collections.ObjectModel;
using System.Data;
using System.Windows.Controls.Primitives;

namespace MinimarketApp.Repository
{
    public class TransaksiPembelianRepository : RepositoryBase
    {
        private SatuanRepository _satuanRepository = new();
        public int Insert(TransaksiPembelianModel model, ObservableCollection<TransaksiPembelianModel> Items)
        {
            int result = 0;
            string transaksi = "INSERT INTO transaksi_pembelian (id_transaksi_pembelian, id_pemasok, total_harga, pembayaran) VALUES (@IdTransaksiPembelian, @IdPemasok, @TotalHarga, @Pembayaran)";
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@IdTransaksiPembelian", model.id_transaksi_pembelian),
                new MySqlParameter("@IdPemasok", model.id_pemasok),
                new MySqlParameter("@TotalHarga", model.total_harga),
                new MySqlParameter("@Pembayaran", model.pembayaran)
            };
            result = ExecuteNonQuery(transaksi, parameters);

            MySqlParameter[] reset_increment_parameters = { };
            ExecuteNonQuery("ALTER TABLE detail_transaksi_pembelian AUTO_INCREMENT = 1", reset_increment_parameters);
            // ExecuteNonQuery("ALTER TABLE detail_produk AUTO_INCREMENT = 1", reset_increment_parameters);
            ProdukRepository _produkRepository = new();
            foreach (TransaksiPembelianModel item in Items)
            {
                string detail_transaksi = "INSERT INTO detail_transaksi_pembelian (id_transaksi_pembelian, id_produk, id_satuan, kuantitas, harga_beli, subtotal) VALUES (@IdTransaksiPembelian, @IdProduk, @IdSatuan, @Kuantitas, @HargaBeli, @Subtotal)";
                MySqlParameter[] parameters_detail_transaksi =
                {
                    new MySqlParameter("@IdTransaksiPembelian", item.id_transaksi_pembelian),
                    new MySqlParameter("@IdProduk", item.id_produk),
                    new MySqlParameter("@IdSatuan", item.id_satuan),
                    new MySqlParameter("@Kuantitas", item.kuantitas),
                    new MySqlParameter("@HargaBeli", item.harga_beli),
                    new MySqlParameter("@Subtotal", item.subtotal)
                };
                result = (result == 1) ? ExecuteNonQuery(detail_transaksi, parameters_detail_transaksi) : 0;

                string produk = "UPDATE produk SET stok = stok + @Kuantitas WHERE id_produk = @IdProduk";
                MySqlParameter[] parameters_produk =
                {
                    new MySqlParameter("@Kuantitas", item.kuantitas * _satuanRepository.GrupProdukKuantitas(item.id_produk, item.id_satuan)),
                    new MySqlParameter("@IdProduk", item.id_produk)
                };

                result = (result == 1) ? ExecuteNonQuery(produk, parameters_produk) : 0;
            }
            return result;
        }

        public int Update(TransaksiPembelianModel model)
        {
            int result = 0;
            string sql = "";
            MySqlParameter[] parameters =
            {

            };
            result = ExecuteNonQuery(sql, parameters);
            return result;
        }

        public int Delete(TransaksiPembelianModel model)
        {
            int result = 0;
            string sql = "DELETE FROM transaksi_pembelian WHERE id_transaksi_pembelian = @IdTransaksiPembelian";
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@IdTransaksiPembelian", model.id_transaksi_pembelian)
            };
            result = ExecuteNonQuery(sql, parameters);
            return result;
        }

        public DataTable GetAllDetail()
        {
            string sql = "SELECT * FROM detail_transaksi_pembelian as dtp INNER JOIN transaksi_pembelian tp ON dtp.id_transaksi_pembelian = tp.id_transaksi_pembelian INNER JOIN pemasok ON pemasok.id_pemasok = tp.id_pemasok INNER JOIN produk ON produk.id_produk = dtp.id_produk ORDER BY detail_transaksi_pembelian.id_detail_transaksi_pembelian;";
            MySqlParameter[] parameters = { };
            return ExecuteDataTable(sql, parameters);
        }

        public DataTable GetAllTransaksi()
        {
            DataTable dt = new();
            string sql = "SELECT * FROM transaksi_pembelian as tp LEFT JOIN pemasok ON pemasok.id_pemasok = tp.id_pemasok ORDER BY tp.tanggal_masuk DESC;";
            MySqlParameter[] parameters = { };
            dt = ExecuteDataTable(sql, parameters);
            DataColumn formmated_tanggal_masuk = new("formatted_tanggal_masuk", typeof(string));
            DataColumn formmated_harga_beli = new("formatted_total_harga", typeof(string));
            DataColumn formatted_pembayaran = new("formatted_pembayaran", typeof(string));
            dt.Columns.Add(formmated_tanggal_masuk);
            dt.Columns.Add(formmated_harga_beli);
            dt.Columns.Add(formatted_pembayaran);
            foreach (DataRow row in dt.Rows)
            {
                row["formatted_tanggal_masuk"] = FormattedSqlDateTime((DateTime)row["tanggal_masuk"]);
                row["formatted_total_harga"] = FormattedCurrency((float)row["total_harga"]);
                row["formatted_pembayaran"] = FormattedCurrency((float)row["pembayaran"]);
            }
            return dt;
        }

        public ObservableCollection<TransaksiPembelianModel> GetDetailById(string id)
        {
            ObservableCollection<TransaksiPembelianModel> items = new();
            string sql = "SELECT * FROM detail_transaksi_pembelian as dtp LEFT JOIN transaksi_pembelian tp ON dtp.id_transaksi_pembelian = tp.id_transaksi_pembelian LEFT JOIN pemasok ON pemasok.id_pemasok = tp.id_pemasok LEFT JOIN produk ON produk.id_produk = dtp.id_produk WHERE dtp.id_transaksi_pembelian = @IdTransaksiPembelian ORDER BY dtp.id_detail_transaksi_pembelian;";
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@IdTransaksiPembelian", id)
            };
            MySqlDataReader reader = ExecuteReader(sql, parameters);
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    items.Add(new TransaksiPembelianModel()
                    {
                        id_detail_transaksi_pembelian = reader.GetInt32("id_detail_transaksi_pembelian"),
                        id_transaksi_pembelian = reader.GetString("id_transaksi_pembelian"),
                        id_pemasok = reader.GetString("id_pemasok"),
                        nama_pemasok = reader["nama_pemasok"].ToString(),
                        id_produk = reader.GetString("id_produk"),
                        barcode = reader["barcode"].ToString(),
                        id_satuan = reader.GetString("id_satuan"),
                        nama_produk = reader["nama_produk"].ToString(),
                        tanggal_masuk = reader.GetDateTime("tanggal_masuk"),
                        formatted_tanggal_masuk = FormattedSqlDateTime(reader.GetDateTime("tanggal_masuk")),
                        kuantitas = reader.GetInt32("kuantitas"),
                        harga_beli = reader.GetFloat("harga_beli"),
                        total_harga = reader.GetFloat("total_harga"),
                        pembayaran = reader.GetFloat("pembayaran")
                    });
                }
            }
            return items;
        }

        public ObservableCollection<TransaksiPembelianModel> GetObservableCollection()
        {
            ObservableCollection<TransaksiPembelianModel> items = new();
            string sql = "SELECT * FROM detail_transaksi_pembelian as dtp INNER JOIN transaksi_pembelian tp ON dtp.id_transaksi_pembelian = tp.id_transaksi_pembelian INNER JOIN pemasok ON pemasok.id_pemasok = tp.id_pemasok INNER JOIN detail_produk ON detail_produk.id_detail_produk = dtp.id_detail_transaksi_pembelian INNER JOIN produk ON produk.id_produk = detail_produk.id_produk ORDER BY detail_transaksi_pembelian.id_detail_transaksi_pembelian;";
            MySqlParameter[] parameters = { };
            MySqlDataReader reader = ExecuteReader(sql, parameters);
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    items.Add(new TransaksiPembelianModel()
                    {
                        id_transaksi_pembelian = reader.GetString("id_transaksi_pembelian"),
                        id_detail_transaksi_pembelian = reader.GetInt32("id_detail_transaksi_pembelian"),
                        id_pemasok = reader.GetString("id_pemasok"),
                        id_produk = reader.GetString("id_produk"),
                        nama_produk = reader["nama_produk"].ToString(),
                        tanggal_masuk = reader.GetDateTime("tanggal_masuk"),
                        formatted_tanggal_masuk = FormattedSqlDateTime(reader.GetDateTime("tanggal_masuk")),
                        kuantitas = reader.GetInt32("kuantitas"),
                        harga_beli = reader.GetFloat("harga_beli"),
                        total_harga = reader.GetFloat("total_harga"),
                        pembayaran = reader.GetFloat("pembayaran"),
                    });
                }
            }
            return items;
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
                    total_pengeluaran = FormattedCurrency(reader.GetFloat("total_pengeluaran"));
                }
            }
            return total_pengeluaran;
        }

        public string GetSatuanTransaksi(string id_detail_transaksi_pembelian)
        {
            string id_satuan = "";
            string sql = "SELECT id_satuan FROM detail_transaksi_pembelian WHERE id_detail_transaksi_pembelian = @IdDetailTransaksiPembelian";
            MySqlParameter[] parameters = { new("@IdDetailTransaksiPembelian", id_detail_transaksi_pembelian) };
            MySqlDataReader reader = ExecuteReader(sql, parameters);
            if (reader == null) return id_satuan;
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    id_satuan = reader.GetString("id_satuan");
                }
            }
            return id_satuan;
        }

        public bool IsExist(string id)
        {
            string sql = "SELECT * FROM transaksi_pembelian WHERE id_transaksi_pembelian = @IdTransaksiPembelian";
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@IdTransaksiPembelian", id)
            };
            DataTable dt = ExecuteDataTable(sql, parameters);
            return dt.Rows.Count > 0;
        }

        public string freeId
        {
            get => GetFreeId("transaksi_pembelian", "TRP", 7);
        }
    }
}
