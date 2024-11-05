using MinimarketApp.Model;
using MinimarketApp.Utilities;
using MySql.Data.MySqlClient;
using System.Data;

namespace MinimarketApp.Repository
{
    public class StokKeluarRepository : RepositoryBase
    {
        public int Insert(StokKeluarModel model)
        {
            string reset_auto_increment = "ALTER TABLE stok_keluar AUTO_INCREMENT = 1";
            ExecuteNonQuery(reset_auto_increment, new MySqlParameter[] { });
            int result = 0;
            string sql = "INSERT INTO stok_keluar (id_stok_keluar, id_produk, kuantitas, keterangan) VALUES (@IdStokKeluar, @IdProduk, @kuantitas, @Keterangan)";
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@IdStokKeluar", model.id_stok_keluar),
                new MySqlParameter("@IdProduk", model.id_produk),
                new MySqlParameter("@kuantitas", model.kuantitas),
                new MySqlParameter("@Keterangan", model.keterangan)
            };
            result = ExecuteNonQuery(sql, parameters);

            string update_stok_produk = "UPDATE produk SET stok = stok - @Kuantitas WHERE id_produk = @IdProduk";
            MySqlParameter[] parameters_stok_produk =
            {
                new MySqlParameter("@Kuantitas", model.kuantitas),
                new MySqlParameter("@IdProduk", model.id_produk)
            };
            result = result > 0 ? ExecuteNonQuery(update_stok_produk, parameters_stok_produk) : 0;

            return result;
        }

        public int Update(StokKeluarModel model)
        {
            int result = 0;
            string sql = "UPDATE stok_keluar SET id_produk = @IdProduk, kuantitas = @kuantitas, keterangan = @Keterangan WHERE id_stok_keluar = @IdStokKeluar";
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@IdStokKeluar", model.id_stok_keluar),
                new MySqlParameter("@IdProduk", model.id_produk),
                new MySqlParameter("@kuantitas", model.kuantitas),
                new MySqlParameter("@Keterangan", model.keterangan)
            };
            result = ExecuteNonQuery(sql, parameters);
            return result;
        }

        public int Delete(StokKeluarModel model)
        {
            int result = 0;
            string sql = "DELETE FROM stok_keluar WHERE id_stok_keluar = @IdStokKeluar";
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@IdStokKeluar", model.id_stok_keluar)
            };
            result = ExecuteNonQuery(sql, parameters);
            return result;
        }

        public DataTable GetAll()
        {
            string sql = "SELECT * FROM stok_keluar LEFT JOIN produk ON produk.id_produk = stok_keluar.id_produk ORDER BY id_stok_keluar";
            MySqlParameter[] parameters = [];
            DataTable dt = ExecuteDataTable(sql, parameters);
            DataColumn formatted_tanggal_column = new("formatted_tanggal_keluar", typeof(string));
            dt.Columns.Add(formatted_tanggal_column);
            foreach (DataRow dr in dt.Rows)
            {
                dr["formatted_tanggal_keluar"] = FormattedSqlDateTime((DateTime)dr["tanggal_keluar"]);
            }
            return dt;
        }

        public StokKeluarModel GetById(string id)
        {
            StokKeluarModel model = new();
            string sql = "SELECT * FROM stok_keluar WHERE id_stok_keluar = @IdStokKeluar";
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@IdStokKeluar", id)
            };
            DataTable dt = ExecuteDataTable(sql, parameters);
            DataRow dr = dt.Rows[0];
            if (dr != null)
            {
                model.id_stok_keluar = dr["id_stok_keluar"].ToString();
                model.id_produk = dr["id_produk"].ToString();
                model.kuantitas = int.Parse(dr["kuantitas"].ToString());
                model.tanggal_keluar = (DateTime)dr["tanggal_keluar"];
                model.formatted_tanggal_keluar = FormattedSqlDateTime((DateTime)dr["tanggal_keluar"]);
                model.keterangan = dr["keterangan"].ToString();
            }
            return model;
        }

        public List<string> GetKeterangan { get => GetFieldEnum("stok_keluar", "keterangan"); }
    }
}
