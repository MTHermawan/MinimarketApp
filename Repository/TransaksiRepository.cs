using Microsoft.Expression.Interactivity.Media;
using MinimarketApp.Utilities;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Security;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;

namespace MinimarketApp.Repository
{
    internal class TransaksiRepository : RepositoryBase
    {
        private SatuanRepository _satuanRepository = new();
        public int Insert(TransaksiModel model, ObservableCollection<TransaksiModel> Items)
        {
            int result = 0;
            string transaksi = "INSERT INTO transaksi (id_transaksi, id_user, telp_pelanggan, total_harga, pembayaran) VALUES (@IdTransaksi, @IdUser, @TelpPelanggan, @TotalHarga, @Pembayaran)";
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@IdTransaksi", model.id_transaksi),
                new MySqlParameter("@IdUser", model.id_user),
                new MySqlParameter("@TelpPelanggan", model.telp_pelanggan),
                new MySqlParameter("@TotalHarga", model.total_harga),
                new MySqlParameter("@Pembayaran", model.pembayaran)
                //new MySqlParameter("@StatusTransaksi", model.status_transaksi)
            };
            result = ExecuteNonQuery(transaksi, parameters);

            string reset_increment = "ALTER TABLE detail_transaksi AUTO_INCREMENT = 1";
            MySqlParameter[] parameters_reset_increment = { };
            ExecuteNonQuery(reset_increment, parameters_reset_increment);
            foreach (TransaksiModel item in Items)
            {
                string detail_transaksi = "INSERT INTO detail_transaksi (id_transaksi, id_produk, id_satuan, kuantitas, subtotal, diskon) VALUES (@IdTransaksi, @IdProduk, @IdSatuan, @Kuantitas, @Subtotal, @Diskon)";
                MySqlParameter[] parameters_detail_transaksi =
                {
                    new MySqlParameter("@IdTransaksi", item.id_transaksi),
                    new MySqlParameter("@IdProduk", item.id_produk),
                    new MySqlParameter("@IdSatuan", item.id_satuan),
                    new MySqlParameter("@Kuantitas", item.kuantitas),
                    new MySqlParameter("@Subtotal", item.subtotal),
                    new MySqlParameter("@Diskon", item.harga_diskon)
                };
                result = ExecuteNonQuery(detail_transaksi, parameters_detail_transaksi);

                /*string update_stok_penjualan = "UPDATE produk SET stok = stok - @Kuantitas WHERE id_produk = @IdProduk";
                MySqlParameter[] parameters_update_stok_penjualan =
                {
                    new MySqlParameter("@Kuantitas", item.kuantitas),
                    new MySqlParameter("@IdProduk", item.id_produk)
                };
                result = ExecuteNonQuery(update_stok_penjualan, parameters_update_stok_penjualan);*/
            }
            return result;
        }

        public int Update(TransaksiModel model)
        {
            int result = 0;
            string sql = "UPDATE transaksi SET id_user = @IdUser, telp_pelanggan = @TelpPelanggan, total_harga = @TotalHarga, pembayaran = @Pembayaran WHERE id_transaksi = @IdTransaksi";
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@IdTransaksi", model.id_transaksi),
                new MySqlParameter("@IdUser", model.id_user),
                new MySqlParameter("@TelpPelanggan", model.telp_pelanggan),
                new MySqlParameter("@TotalHarga", model.total_harga),
                new MySqlParameter("@Pembayaran", model.pembayaran),
                // new MySqlParameter("@Status", model.status_transaksi)
            };
            result = ExecuteNonQuery(sql, parameters);
            return result;
        }

        public int Delete(TransaksiModel model)
        {
            int result = 0;
            string sql = "DELETE FROM transaksi WHERE id_transaksi = @IdTransaksi";
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@IdTransaksi", model.id_transaksi)
            };
            result = ExecuteNonQuery(sql, parameters);
            return result;
        }

        public DataTable GetAllTransaksi()
        {
            DataTable dt = new();
            string sql = "SELECT * FROM transaksi LEFT JOIN user ON user.id_user = transaksi.id_user LEFT JOIN pelanggan ON pelanggan.telp_pelanggan = transaksi.telp_pelanggan ORDER BY transaksi.tanggal_transaksi DESC;";
            MySqlParameter[] parameters = { };

            dt = ExecuteDataTable(sql, parameters);
            DataColumn formatted_tanggal_transaksi = new("formatted_tanggal_transaksi", typeof(string));
            DataColumn formatted_total_harga = new("formatted_total_harga", typeof(string));
            DataColumn formatted_pembayaran = new("formatted_pembayaran", typeof(string));
            dt.Columns.Add(formatted_tanggal_transaksi);
            dt.Columns.Add(formatted_total_harga);
            dt.Columns.Add(formatted_pembayaran);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    row["formatted_tanggal_transaksi"] = FormattedSqlDateTime(row.Field<DateTime>("tanggal_transaksi"));
                    row["formatted_total_harga"] = FormattedCurrency(row.Field<float>("total_harga"));
                    row["formatted_pembayaran"] = FormattedCurrency(row.Field<float>("pembayaran"));
                    row["nama_pelanggan"] = string.IsNullOrEmpty(row["nama_pelanggan"].ToString()) ? "UMUM" : row["nama_pelanggan"];
                }
            }
            return dt;
        }

        public TransaksiModel GetTransaksiById(string idTransaksi)
        {
            TransaksiModel item = new();
            string sql = "SELECT * FROM transaksi LEFT JOIN user ON user.id_user = transaksi.id_user LEFT JOIN pelanggan ON pelanggan.telp_pelanggan = transaksi.tepl_pelanggan ORDER BY transaksi.id_transaksi WHERE transaksi.id_transaksi = @IdTransaksi";
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@IdTransaksi", idTransaksi)
            };
            MySqlDataReader reader = ExecuteReader(sql, parameters);
            if (reader == null) return item;
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    item.id_transaksi = reader.GetString("id_transaksi");
                    item.id_user = reader.GetString("id_user");
                    item.username = reader.GetString("username");
                    item.telp_pelanggan = reader.GetString("telp_pelanggan");
                    // item.nama_pelanggan = reader.GetString("nama_pelanggan");
                    item.tanggal_transaksi = reader.GetDateTime("tanggal_transaksi");
                    item.formatted_tanggal_transaksi = FormattedSqlDateTime(reader.GetDateTime("tanggal_transaksi"));
                    item.total_harga = reader.GetFloat("total_harga");
                    item.pembayaran = reader.GetFloat("pembayaran");
                    // item.status_transaksi = reader.GetString("status_transaksi");
                }
            }
            return item;
        }

        public ObservableCollection<TransaksiModel> GetDetailById(string idTransaksi)
        {
            ObservableCollection<TransaksiModel> items = new();
            string sql = "SELECT * FROM detail_transaksi INNER JOIN transaksi ON transaksi.id_transaksi = detail_transaksi.id_transaksi LEFT JOIN produk ON produk.id_produk = detail_transaksi.id_produk LEFT JOIN pelanggan ON pelanggan.telp_pelanggan = transaksi.telp_pelanggan LEFT JOIN satuan ON satuan.id_satuan = produk.id_satuan WHERE transaksi.id_transaksi = @IdTransaksi";
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@IdTransaksi", idTransaksi)
            };
            MySqlDataReader reader = ExecuteReader(sql, parameters);
            if (reader == null) return items;
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    items.Add(new TransaksiModel()
                    {
                        id_detail_transaksi = reader.GetInt32("id_detail_transaksi"),
                        id_transaksi = reader.GetString("id_transaksi"),
                        id_user = reader.GetString("id_user"),
                        telp_pelanggan = reader.IsDBNull("telp_pelanggan") ? "" : reader.GetString("telp_pelanggan"),
                        // nama_pelanggan = reader.GetString("nama_pelanggan"),
                        id_produk = reader.GetString("id_produk"),
                        barcode = reader.GetString("barcode"),
                        nama_produk = reader.GetString("nama_produk"),
                        nama_satuan = reader.GetString("nama_satuan"),
                        tanggal_transaksi = reader.GetDateTime("tanggal_transaksi"),
                        formatted_tanggal_transaksi = FormattedSqlDateTime(reader.GetDateTime("tanggal_transaksi")),
                        id_satuan = reader.GetString("id_satuan"),
                        // id_detail_produk = reader.GetInt32("id_detail_produk"),
                        // harga_jual = reader.GetFloat("harga_jual"),
                        pembayaran = reader.GetFloat("pembayaran"),
                        kuantitas = reader.GetInt32("kuantitas"),
                        // status_transaksi = reader.GetString("status_transaksi"),
                        // formatted_harga_jual = FormattedCurrency(reader.GetFloat("harga_jual")),
                        formatted_pembayaran = FormattedCurrency(reader.GetFloat("pembayaran")),
                    });
                }
            }
            return items;
        }

        public ObservableCollection<TransaksiModel> GetObservableCollection()
        {
            ObservableCollection<TransaksiModel> items = new();
            string sql = "SELECT * FROM detail_produk as dp INNER JOIN produk as prod ON prod.id_produk = dp.id_produk INNER JOIN kategori as ktg ON ktg.id_kategori = prod.id_kategori INNER JOIN satuan as stn ON stn.id_satuan = prod.id_satuan INNER JOIN detail_transaksi_pembelian ON detail_transaksi_pembelian.id_detail_produk = dp.id_detail_produk INNER JOIN transaksi_pembelian ON transaksi_pembelian.id_transaksi_pembelian = detail_transaksi_pembelian.id_transaksi_pembelian;";
            MySqlParameter[] parameters = { };
            MySqlDataReader reader = ExecuteReader(sql, parameters);
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    items.Add(new TransaksiModel()
                    {
                        id_transaksi = reader.GetString("id_transaksi"),
                        id_detail_transaksi = reader.GetInt32("id_detail_transaksi"),
                        id_user = reader.GetString("id_user"),
                        telp_pelanggan = reader.GetString("telp_pelanggan"),
                        id_produk = reader.GetString("id_produk"),
                        // id_detail_produk = reader.GetInt32("id_detail_produk"),
                        nama_produk = reader.GetString("nama_produk"),
                        tanggal_transaksi = reader.GetDateTime("tanggal_transaksi"),
                        formatted_tanggal_transaksi = FormattedSqlDateTime(reader.GetDateTime("formatted_tanggal_transaksi")),
                        // harga_jual = reader.GetInt32("harga_jual"),
                        kuantitas = reader.GetInt32("kuantitas"),
                        nama_satuan = reader.GetString("nama_satuan"),
                        total_harga = reader.GetFloat("total_harga"),
                        pembayaran = reader.GetFloat("pembayaran"),
                        // kembalian = reader.GetFloat("kembalian"),
                        status_transaksi = reader.GetString("status_transaksi"),
                    });
                }
            }
            return items;
        }

        public string freeId
        {
            get => GetFreeId("transaksi", "TR", 7);
        }

        public int UpdateStatusTransaksi(string idTransaksi, string status)
        {
            int result = 0;
            if (string.IsNullOrEmpty(idTransaksi)) return result;
            string sql = "UPDATE transaksi SET status_transaksi = @StatusTransaksi WHERE id_transaksi = @IdTransaksi";
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@IdTransaksi", idTransaksi),
                new MySqlParameter("@StatusTransaksi", status)
            };
            ExecuteNonQuery(sql, parameters);

            if (status == "Selesai")
            {
                ObservableCollection<TransaksiModel> detailTransaksi = GetDetailById(idTransaksi);

                foreach (var item in detailTransaksi)
                {
                    string update_stok_penjualan = "UPDATE produk SET stok = stok - @Kuantitas WHERE id_produk = @IdProduk";
                    MySqlParameter[] parameters_update_stok_penjualan =
                    {
                    new MySqlParameter("@Kuantitas", item.kuantitas * _satuanRepository.GrupProdukKuantitas(item.id_produk, item.id_satuan)),
                    new MySqlParameter("@IdProduk", item.id_produk)
                };
                    result = ExecuteNonQuery(update_stok_penjualan, parameters_update_stok_penjualan);
                }
            }
            return result;
        }

        public void CancelTransaksi(string idTransaksi)
        {
            ObservableCollection<TransaksiModel> items = GetDetailById(idTransaksi);
            if (items != null)
            {
                /*string update_stok_penjualan = "UPDATE produk SET stok = stok + @Kuantitas WHERE id_produk = @IdProduk";
                foreach (TransaksiModel item in items)
                {
                    MySqlParameter[] parameters_update_stok_penjualan =
                    {
                        new MySqlParameter("@Kuantitas", item.kuantitas),
                        new MySqlParameter("@IdProduk", item.id_produk)
                    };
                    ExecuteNonQuery(update_stok_penjualan, parameters_update_stok_penjualan);
                }*/
            }
        }

        public string GetStatusTransaksi(string idTransaksi)
        {
            string sql = "SELECT status_transaksi FROM transaksi WHERE id_transaksi = @IdTransaksi";
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@IdTransaksi", idTransaksi)
            };
            MySqlDataReader reader = ExecuteReader(sql, parameters);
            if (reader == null) return "";
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    return reader.GetString("status_transaksi");
                }
            }
            return "";
        }

        public int RefreshTransaksi(TransaksiModel model, ObservableCollection<TransaksiModel> items)
        {
            int result = 0;
            string update_transaksi = "UPDATE transaksi SET id_user = @IdUser, telp_pelanggan = @TelpPelanggan, total_harga = @TotalHarga, pembayaran = @Pembayaran WHERE id_transaksi = @IdTransaksi";
            MySqlParameter[] parameters_update_transaksi =
            {
                new MySqlParameter("@IdTransaksi", model.id_transaksi),
                new MySqlParameter("@IdUser", model.id_user),
                new MySqlParameter("@TelpPelanggan", model.telp_pelanggan),
                new MySqlParameter("@TotalHarga", model.total_harga),
                new MySqlParameter("@Pembayaran", model.pembayaran)
            };

            ExecuteNonQuery(update_transaksi, parameters_update_transaksi);

            /*string old_detail = "SELECT * FROM detail_transaksi WHERE id_transaksi = @IdTransaksi";
            MySqlParameter[] parameters_old_detail =
            {
                new MySqlParameter("@IdTransaksi", model.id_transaksi)
            };
            MySqlDataReader reader = ExecuteReader(old_detail, parameters_old_detail);*/

            /*if (reader.HasRows)
            {
                while (reader.Read())
                {
                    string update_stok_penjualan = "UPDATE produk SET stok = stok + @Kuantitas WHERE id_produk = @IdProduk";
                    MySqlParameter[] parameters_update_stok_penjualan =
                    {
                        new MySqlParameter("@Kuantitas", reader.GetInt32("kuantitas")),
                        new MySqlParameter("@IdProduk", reader.GetString("id_produk"))
                    };
                    result = ExecuteNonQuery(update_stok_penjualan, parameters_update_stok_penjualan);
                }
            }*/

            string delete_detail_transaksi = "DELETE FROM detail_transaksi WHERE id_transaksi = @IdTransaksi";
            MySqlParameter[] parameters_delete_detail_transaksi =
            {
                new MySqlParameter("@IdTransaksi", model.id_transaksi)
            };
            result = ExecuteNonQuery(delete_detail_transaksi, parameters_delete_detail_transaksi);

            System.Threading.Thread.Sleep(500);

            foreach (TransaksiModel item in items)
            {
                string detail_transaksi = "INSERT INTO detail_transaksi (id_transaksi, id_produk, id_satuan, kuantitas, subtotal, diskon) VALUES (@IdTransaksi, @IdProduk, @IdSatuan, @Kuantitas, @Subtotal, @Diskon)";
                MySqlParameter[] parameters_detail_transaksi =
                {
                    new MySqlParameter("@IdTransaksi", item.id_transaksi),
                    new MySqlParameter("@IdProduk", item.id_produk),
                    new MySqlParameter("@IdSatuan", item.id_satuan),
                    new MySqlParameter("@Kuantitas", item.kuantitas),
                    new MySqlParameter("@Subtotal", item.subtotal),
                    new MySqlParameter("@Diskon", item.harga_diskon)
                };
                result = result > 0 ? ExecuteNonQuery(detail_transaksi, parameters_detail_transaksi) : 0;
            }

            /*string update_stok = "UPDATE produk SET stok = stok - @Kuantitas WHERE id_produk = @IdProduk";
            foreach (TransaksiModel item in items)
            {
                MySqlParameter[] parameters_update_stok =
                {
                    new MySqlParameter("@Kuantitas", item.kuantitas),
                    new MySqlParameter("@IdProduk", item.id_produk)
                };
                result = result > 0 ? ExecuteNonQuery(update_stok, parameters_update_stok) : 0;
            }*/
            return result;
        }

        public string GetTotalPenjualanBulanIni()
        {
            string penjualan = "Rp0,00";
            string sql = "SELECT SUM(total_harga) as total FROM transaksi WHERE MONTH(tanggal_transaksi) = MONTH(CURRENT_DATE()) AND YEAR(tanggal_transaksi) = YEAR(CURRENT_DATE())";
            MySqlParameter[] parameters = { };
            MySqlDataReader reader = ExecuteReader(sql, parameters);
            if (reader == null) return penjualan;
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    if (reader.IsDBNull("total")) return penjualan;
                    penjualan = FormattedCurrency(reader.GetFloat("total"));
                }
            }

            return penjualan;
        }

        /*public string GetTotalLabaBulanIni()
        {
            string laba = "Rp0,00";
            string sql = "SELECT SUM((produk.harga_jual - produk.harga_beli) * detail_transaksi.kuantitas) as total FROM detail_transaksi INNER JOIN produk ON produk.id_produk = detail_transaksi.id_produk INNER JOIN transaksi ON transaksi.id_transaksi = detail_transaksi.id_transaksi WHERE MONTH(transaksi.tanggal_transaksi) = MONTH(CURRENT_DATE()) AND YEAR(transaksi.tanggal_transaksi) = YEAR(CURRENT_DATE())";
            MySqlParameter[] parameters = { };
            MySqlDataReader reader = ExecuteReader(sql, parameters);
            if (reader == null) return laba;
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    if (reader.IsDBNull("total")) return laba;
                    laba = FormattedCurrency(reader.GetFloat("total"));
                }
            }
            return laba;
        }*/

        public string GetJumlahTransaksiBulanIni()
        {
            string jumlah = "0";
            string sql = "SELECT COUNT(*) as jumlah FROM transaksi WHERE MONTH(tanggal_transaksi) = MONTH(CURRENT_DATE()) AND YEAR(tanggal_transaksi) = YEAR(CURRENT_DATE())";
            MySqlParameter[] parameters = { };
            MySqlDataReader reader = ExecuteReader(sql, parameters);
            if (reader == null) return jumlah;
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    if (reader.IsDBNull("jumlah")) return jumlah;
                    if (reader.GetInt32("jumlah") == 0) return "Tidak ada transaksi";
                    jumlah = reader.GetInt32("jumlah").ToString() + " Transaksi";
                }
            }
            return jumlah;
        }

        public string GetSatuanTransaksi(string id_detail_transaksi)
        {
            string id_satuan = "";
            string sql = "SELECT id_satuan FROM detail_transaksi WHERE id_detail_transaksi = @IdDetailTransaksi";
            MySqlParameter[] parameters = { new("@IdDetailTransaksi", id_detail_transaksi) };
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

        public string GetKasirTransaksi(string id_transaksi)
        {
            string id_user = null;
            string sql = "SELECT username FROM transaksi LEFT JOIN user ON user.id_user = transaksi.id_user WHERE id_transaksi = @IdTransaksi";
            MySqlParameter[] parameters = { new("@IdTransaksi", id_transaksi) };
            MySqlDataReader reader = ExecuteReader(sql, parameters);
            if (reader == null) return id_user;
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    id_user = reader.GetString("username");
                }
            }
            return id_user;
        }

        public string GetTanggalTransaksi(string id_transaksi)
        {
            string tanggal = "";
            string sql = "SELECT tanggal_transaksi FROM transaksi WHERE id_transaksi = @IdTransaksi";
            MySqlParameter[] parameters = { new("@IdTransaksi", id_transaksi) };
            MySqlDataReader reader = ExecuteReader(sql, parameters);
            if (reader == null) return tanggal;
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    tanggal = reader.GetDateTime("tanggal_transaksi").ToString("dd MMMM yyyy", new CultureInfo("id-ID"));
                }
            }
            return tanggal;
        }

        public bool IsExist(string id)
        {
            string sql = "SELECT * FROM transaksi WHERE id_transaksi = @IdTransaksi";
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@IdTransaksi", id)
            };
            return ExecuteDataTable(sql, parameters).Rows.Count > 0;
        }
    }
}
