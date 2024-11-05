using MinimarketApp.Model;
using MinimarketApp.Utilities;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;

namespace MinimarketApp.Repository
{
    public class DiskonRepository : RepositoryBase
    {
        public int Insert(DiskonModel diskon)
        {
            string sql = "INSERT INTO diskon (id_diskon, id_produk, id_satuan, jenis_diskon, total, pilihan_diskon, nilai, tanggal_mulai, tanggal_akhir, keterangan) VALUES (@id_diskon, @id_produk, @id_satuan, @jenis_diskon, @total, @pilihan_diskon, @nilai, @tanggal_mulai, @tanggal_akhir, @keterangan)";
            MySqlParameter[] parameters =
            {
                new("@id_diskon", diskon.id_diskon),
                new("@id_produk", diskon.id_produk),
                new("id_satuan", diskon.id_satuan),
                new("@jenis_diskon", diskon.jenis_diskon),
                new("@total", diskon.total),
                new("@pilihan_diskon", diskon.pilihan_diskon),
                new("@nilai", diskon.nilai),
                new("@tanggal_mulai", diskon.tanggal_mulai),
                new("@tanggal_akhir", diskon.tanggal_berakhir),
                new("@keterangan", diskon.keterangan)
            };
            return ExecuteNonQuery(sql, parameters);
        }

        public int Update(DiskonModel diskon)
        {
            string sql = "UPDATE diskon SET id_produk = @id_produk, id_satuan = @id_satuan, jenis_diskon = @jenis_diskon, total = @total, pilihan_diskon = @pilihan_diskon, nilai = @nilai, tanggal_mulai = @tanggal_mulai, tanggal_akhir = @tanggal_akhir, keterangan = @keterangan WHERE id_diskon = @id_diskon";
            MySqlParameter[] parameters =
            {
                new("@id_diskon", diskon.id_diskon),
                new("@id_produk", diskon.id_produk),
                new("id_satuan", diskon.id_satuan),
                new("@jenis_diskon", diskon.jenis_diskon),
                new("@total", diskon.total),
                new("@pilihan_diskon", diskon.pilihan_diskon),
                new("@nilai", diskon.nilai),
                new("@tanggal_mulai", diskon.tanggal_mulai),
                new("@tanggal_akhir", diskon.tanggal_berakhir),
                new("@keterangan", diskon.keterangan)
            };
            return ExecuteNonQuery(sql, parameters);
        }

        public int Delete(DiskonModel diskon)
        {
            string sql = "DELETE FROM diskon WHERE id_diskon = @id_diskon";
            MySqlParameter[] parameters =
            {
                new("@id_diskon", diskon.id_diskon)
            };
            return ExecuteNonQuery(sql, parameters);
        }

        public DataTable GetAll()
        {
            string sql = "SELECT * FROM diskon LEFT JOIN produk ON produk.id_produk = diskon.id_produk LEFT JOIN satuan ON satuan.id_satuan = diskon.id_satuan ORDER BY id_diskon ASC";
            MySqlParameter[] parameters = [];
            DataTable dt = ExecuteDataTable(sql, parameters);
            DataColumn formatted_tanggal_mulai = new("formatted_tanggal_mulai", typeof(string));
            DataColumn formatted_tanggal_akhir = new("formatted_tanggal_akhir", typeof(string));
            DataColumn formatted_total = new("formatted_total", typeof(string));
            DataColumn formatted_nilai = new("formatted_nilai", typeof(string));
            dt.Columns.Add(formatted_tanggal_mulai);
            dt.Columns.Add(formatted_tanggal_akhir);
            dt.Columns.Add(formatted_total);
            dt.Columns.Add(formatted_nilai);
            foreach (DataRow dr in dt.Rows)
            {
                if (dr["total"] != DBNull.Value)
                {
                    switch (dr["jenis_diskon"].ToString())
                    {
                        case "Harga":
                            dr["formatted_total"] = FormattedCurrency((float)dr["total"]);
                            break;
                        case "Kuantitas":
                            dr["formatted_total"] = ((float)dr["total"]).ToString() + " Barang";
                            break;
                    }
                }
                if (dr["nilai"] != DBNull.Value)
                {
                    switch (dr["pilihan_diskon"].ToString())
                    {
                        case "Diskon Rupiah":
                            dr["formatted_nilai"] = FormattedCurrency((float)dr["nilai"]);
                            break;
                        case "Diskon Persen":
                            dr["formatted_nilai"] = ((float)dr["nilai"]).ToString() + "%";
                            break;
                    }
                }
                dr["formatted_tanggal_mulai"] = FormattedDate((DateTime)dr["tanggal_mulai"]);
                dr["formatted_tanggal_akhir"] = FormattedDate((DateTime)dr["tanggal_akhir"]);
            }
            return dt;
        }

        public List<string> GetComboJenisDiskon()
        {
            return GetFieldEnum("diskon", "jenis_diskon");
        }

        public List<string> GetComboPilihanDiskon()
        {
            return GetFieldEnum("diskon", "pilihan_diskon");
        }

        public DiskonModel GetDiskon(string id)
        {
            string sql = "SELECT * FROM diskon WHERE id_produk = @IdProduk";
            MySqlParameter[] parameters =
            {
                new("@IdProduk", id)
            };
            DataTable dt = ExecuteDataTable(sql, parameters);
            DiskonModel diskon = new();
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                diskon.id_diskon = dr["id_diskon"].ToString();
                diskon.id_produk = dr["id_produk"].ToString();
                diskon.id_satuan = dr["id_satuan"].ToString();
                diskon.jenis_diskon = dr["jenis_diskon"].ToString();
                diskon.total = (float)dr["total"];
                diskon.pilihan_diskon = dr["pilihan_diskon"].ToString();
                diskon.nilai = (float)dr["nilai"];
                diskon.tanggal_mulai = (DateTime)dr["tanggal_mulai"];
                diskon.tanggal_berakhir = (DateTime)dr["tanggal_akhir"];
                diskon.keterangan = dr["keterangan"].ToString();
            }
            return diskon;
        }

        public ObservableCollection<DiskonModel> GetActiveDiskons(string id_produk)
        {
            string sql = "SELECT * FROM diskon WHERE id_produk = @IdProduk AND tanggal_mulai <= @Tanggal AND tanggal_akhir >= @Tanggal";
            MySqlParameter[] parameters =
            {
                new("@IdProduk", id_produk),
                new("@Tanggal", DateTime.Now.Date)
            };
            DataTable dt = ExecuteDataTable(sql, parameters);
            ObservableCollection<DiskonModel> diskons = new();
            foreach (DataRow dr in dt.Rows)
            {
                DiskonModel diskon = new();
                diskon.id_diskon = dr["id_diskon"].ToString();
                diskon.id_produk = dr["id_produk"].ToString();
                diskon.jenis_diskon = dr["jenis_diskon"].ToString();
                diskon.id_satuan = dr["id_satuan"].ToString();
                diskon.total = (float)dr["total"];
                diskon.pilihan_diskon = dr["pilihan_diskon"].ToString();
                diskon.nilai = (float)dr["nilai"];
                diskon.tanggal_mulai = (DateTime)dr["tanggal_mulai"];
                diskon.tanggal_berakhir = (DateTime)dr["tanggal_akhir"];
                diskon.keterangan = dr["keterangan"].ToString();
                diskons.Add(diskon);
            }
            return diskons;
        }

        public bool IsExist(string id)
        {
            string sql = "SELECT * FROM diskon WHERE id_diskon = @IdDiskon";
            MySqlParameter[] parameters =
            {
                new("@IdDiskon", id)
            };
            return ExecuteDataTable(sql, parameters).Rows.Count > 0;
        }

        public string freeId
        {
            get => GetFreeId("diskon", "DSK", 7);
        }
    }
}
