using MinimarketApp.Model;
using MinimarketApp.Utilities;
using MySql.Data.MySqlClient;
using System.Collections.ObjectModel;
using System.Data;

namespace MinimarketApp.Repository
{
    public class ProdukRepository : RepositoryBase
    {
        public int Insert(ProdukModel model)
        {
            int result = 0;
            string sql = "INSERT INTO produk (id_produk, barcode, nama_produk, id_kategori, id_satuan, harga_jual, status) VALUES (@IdProduk, @Barcode, @NamaProduk, @IdKategori, @IdSatuan, @HargaJual, @Status)";
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@IdProduk", model.id_produk),
                new MySqlParameter("@Barcode", model.barcode),
                new MySqlParameter("@NamaProduk", model.nama_produk),
                new MySqlParameter("@IdKategori", model.id_kategori),
                new MySqlParameter("@IdSatuan", model.id_satuan),
                new MySqlParameter("@HargaJual", model.harga_jual),
                new MySqlParameter("@Status", model.status)

            };
            result = ExecuteNonQuery(sql, parameters);
            return result;
        }

        public int Update(ProdukModel model)
        {
            int result = 0;
            string sql = "UPDATE produk SET barcode = @Barcode, nama_produk = @NamaProduk, id_kategori = @IdKategori, id_satuan = @IdSatuan, harga_jual = @HargaJual, status = @Status WHERE id_produk = @IdProduk";
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@IdProduk", model.id_produk),
                new MySqlParameter("@Barcode", model.barcode),
                new MySqlParameter("@NamaProduk", model.nama_produk),
                new MySqlParameter("@IdKategori", model.id_kategori),
                new MySqlParameter("@IdSatuan", model.id_satuan),
                new MySqlParameter("@HargaJual", model.harga_jual),
                new MySqlParameter("@Status", model.status)
            };
            result = ExecuteNonQuery(sql, parameters);
            return result;
        }

        public int Delete(ProdukModel model)
        {
            int result = 0;
            string sql = "DELETE FROM produk WHERE barcode = @Barcode";
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@Barcode", model.barcode)
            };
            result = ExecuteNonQuery(sql, parameters);
            return result;
        }

        public DataTable GetAllProduk()
        {
            string sql = "SELECT * FROM produk INNER JOIN kategori ON kategori.id_kategori = produk.id_kategori INNER JOIN satuan ON satuan.id_satuan = produk.id_satuan ORDER BY produk.id_produk";
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
            return dt;
        }
        
        public DataTable GetAllActiveProduk()
        {
            string sql = "SELECT * FROM produk INNER JOIN kategori ON kategori.id_kategori = produk.id_kategori INNER JOIN satuan ON satuan.id_satuan = produk.id_satuan WHERE status = 'Tersedia' AND stok > 0 ORDER BY produk.id_produk";
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
            return dt;
        }

        /*public DataTable GetAllDetail()
        {
            string sql = "SELECT * FROM detail_produk INNER JOIN produk ON produk.id_produk = detail_produk.id_produk LEFT JOIN kategori ON kategori.id_kategori = produk.id_kategori LEFT JOIN satuan ON satuan.id_satuan = produk.id_satuan ORDER BY detail_produk.id_detail_produk";
            MySqlParameter[] parameters = [];
            DataTable dt = ExecuteDataTable(sql, parameters);
            DataColumn formmatted_harga_jual = new("formatted_harga_jual", typeof(string));
            dt.Columns.Add(formmatted_harga_jual);
            foreach (DataRow row in dt.Rows)
            {
                row["formatted_harga_jual"] = FormattedCurrency((float)row["harga_jual"]);
            }
            return dt;
        }*/

        public DataTable GetAllProdukLike(string text = "")
        {
            string sql = $"SELECT *, CONCAT(produk.barcode, ' - ', produk.nama_produk) AS ComboProdukView FROM produk LEFT JOIN kategori ON kategori.id_kategori = produk.id_kategori LEFT JOIN satuan ON satuan.id_satuan = produk.id_satuan HAVING ComboProdukView LIKE '%{text}%' ORDER BY produk.id_produk;";
            MySqlParameter[] parameters =
            {

            };
            DataTable dt = ExecuteDataTable(sql, parameters);
            DataColumn formmatted_harga_jual = new("formatted_harga_jual", typeof(string));
            dt.Columns.Add(formmatted_harga_jual);
            foreach (DataRow row in dt.Rows)
            {
                row["formatted_harga_jual"] = FormattedCurrency((float)row["harga_jual"]);    
            }
            return dt;
        }
        
        public DataTable GetAllTersediaProdukLike(string text = "")
        {
            string sql = $"SELECT *, CONCAT(produk.barcode, ' - ', produk.nama_produk) AS ComboProdukView FROM produk LEFT JOIN kategori ON kategori.id_kategori = produk.id_kategori LEFT JOIN satuan ON satuan.id_satuan = produk.id_satuan HAVING ComboProdukView LIKE '%{text}%' AND status = 'Tersedia' ORDER BY produk.id_produk;";
            MySqlParameter[] parameters =
            {

            };
            DataTable dt = ExecuteDataTable(sql, parameters);
            DataColumn formmatted_harga_jual = new("formatted_harga_jual", typeof(string));
            dt.Columns.Add(formmatted_harga_jual);
            foreach (DataRow row in dt.Rows)
            {
                row["formatted_harga_jual"] = FormattedCurrency((float)row["harga_jual"]);    
            }
            return dt;
        }
        
        public DataTable GetAllActiveProdukLike(string text = "")
        {
            string sql = $"SELECT *, CONCAT(produk.barcode, ' - ', produk.nama_produk) AS ComboProdukView FROM produk LEFT JOIN kategori ON kategori.id_kategori = produk.id_kategori LEFT JOIN satuan ON satuan.id_satuan = produk.id_satuan HAVING ComboProdukView LIKE '%{text}%' AND status = 'Tersedia' AND stok > 0 ORDER BY produk.id_produk;";
            MySqlParameter[] parameters =
            {

            };
            DataTable dt = ExecuteDataTable(sql, parameters);
            DataColumn formmatted_harga_jual = new("formatted_harga_jual", typeof(string));
            dt.Columns.Add(formmatted_harga_jual);
            foreach (DataRow row in dt.Rows)
            {
                row["formatted_harga_jual"] = FormattedCurrency((float)row["harga_jual"]);    
            }
            return dt;
        }

        /*public DataTable GetAllDetailLike(string text)
        {
            string sql = $"SELECT *, CONCAT(produk.barcode, ' - ', produk.nama_produk) AS ComboProdukView FROM produk LEFT JOIN kategori ON kategori.id_kategori = produk.id_kategori LEFT JOIN satuan ON satuan.id_satuan = produk.id_satuan HAVING ComboProdukView LIKE '%{text}%' ORDER BY produk.id_produk;";
            MySqlParameter[] parameters =
            {

            };
            DataTable dt = ExecuteDataTable(sql, parameters);
            DataColumn formmatted_harga_jual = new("formatted_harga_jual", typeof(string));
            DataColumn stokColumn = new DataColumn("stok", typeof(int));
            dt.Columns.Add(formmatted_harga_jual);
            dt.Columns.Add(stokColumn);
            foreach (DataRow row in dt.Rows)
            {
                row["formatted_harga_jual"] = FormattedCurrency((float)row["harga_jual"]);
                row["stok"] = GetStok(row["id_produk"].ToString());
            }
            return dt;
        }*/

        /*public ObservableCollection<ProdukModel> GetDetailProduk()
        {
            ObservableCollection<ProdukModel> items = new();
            string sql = "SELECT * FROM detail_produk as dp INNER JOIN produk ON produk.id_produk = dp.id_produk INNER JOIN kategori ON kategori.id_kategori = produk.id_kategori INNER JOIN satuan ON satuan.id_satuan = produk.id_satuan ORDER BY dp.id_detail_produk";
            MySqlParameter[] parameters = [];
            MySqlDataReader reader = ExecuteReader(sql, parameters);
            if (reader != null)
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        items.Add(new ProdukModel
                        {
                            id_produk = reader.GetString("id_produk"),
                            id_detail_produk = reader.GetInt32("id_detail_produk"),
                            nama_produk = reader.GetString("nama_produk"),
                            nama_kategori = reader.GetString("nama_kategori"),
                            nama_satuan = reader.GetString("nama_satuan"),
                            harga_jual = reader.GetFloat("harga_jual"),
                            harga_beli = reader.GetFloat("harga_beli"),
                            status = reader.GetString("status"),
                            nomor_batch = reader.GetString("nomor_batch"),
                            tanggal_kadaluarsa = reader.GetDateTime("tanggal_kadaluarsa"),
                            formatted_tanggal_kadaluarsa = FormattedSqlDateTime(reader.GetDateTime("tanggal_kadaluarsa")),
                            stok_kuantitas = reader.GetInt32("stok_kuantitas")
                        });
                    }
                }
            }
            return items;
        }*/

        public ObservableCollection<ProdukModel> GetProdukItem()
        {
            ObservableCollection<ProdukModel> items = new();
            string sql = "SELECT * FROM produk INNER JOIN satuan ON satuan.id_satuan = produk.id_satuan INNER JOIN kategori ON kategori.id_kategori = produk.id_kategori ORDER BY produk.id_produk";
            MySqlParameter[] parameters = [];
            MySqlDataReader reader = ExecuteReader(sql, parameters);
            if (reader != null)
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        items.Add(new ProdukModel
                        {
                            id_produk = reader.GetString("id_produk"),
                            nama_produk = reader.GetString("nama_produk"),
                            nama_satuan = reader.GetString("nama_satuan"),
                            nama_kategori = reader.GetString("nama_kategori"),
                            harga_jual = reader.GetFloat("harga_jual")
                        });
                    }
                }
            }
            return items;
        }

        public ProdukModel GetProdukById(string id_produk)
        {
            ProdukModel item = new();
            string sql = "SELECT * FROM produk WHERE id_produk = @IdProduk";
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@IdProduk", id_produk)
            };
            MySqlDataReader reader = ExecuteReader(sql, parameters);
            if (reader != null)
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        item.id_produk = reader.GetString("id_produk");
                        item.nama_produk = reader.GetString("nama_produk");
                        item.id_kategori = reader.GetString("id_kategori");
                        item.id_satuan = reader.GetString("id_satuan");
                        item.harga_jual = reader.GetFloat("harga_jual");
                        item.status = reader.GetString("status");
                    }
                }
            }
            return item;
        }

        public int GetStok(string id_produk)
        {
            int result = 0;
            string sql = "SELECT stok FROM produk WHERE id_produk = @IdProduk";
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@IdProduk", id_produk)
            };

            DataTable dt = ExecuteDataTable(sql, parameters);
            if (dt.Rows.Count > 0)
            {
                result = (int)dt.Rows[0]["stok"];
            }

            return result;
        }

        public bool IsExist(string id_produk)
        {
            string sql = "SELECT * FROM produk WHERE id_produk = @IdProduk";
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@IdProduk", id_produk)
            };
            DataTable dt = ExecuteDataTable(sql, parameters);
            return dt.Rows.Count > 0;
        }

        public bool IsBarcodeExist(string barcode, string id)
        {
            string sql = "SELECT * FROM produk WHERE barcode = @Barcode AND id_produk != @id_produk";
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@Barcode", barcode),
                new MySqlParameter("@id_produk", id)
            };
            DataTable dt = ExecuteDataTable(sql, parameters);
            return dt.Rows.Count > 0;
        }

        public List<string> GetStatus()
        {
            return GetFieldEnum("produk", "status");
        }

        public string freeId
        {
            get => GetFreeId("produk", "PRD", 7);
        }

        /*public int freeDetailId
        {
            get
            {
                _ = int.TryParse(GetFreeId("detail_produk", "", 10), out int id_detail_produk);
                return id_detail_produk;
            }
        }*/
    }
}
