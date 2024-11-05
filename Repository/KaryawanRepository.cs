using MinimarketApp.Model;
using MinimarketApp.Utilities;
using MySql.Data.MySqlClient;
using System.Data;

namespace MinimarketApp.Repository
{
    public class KaryawanRepository : RepositoryBase
    {
        public int Insert(KaryawanModel model)
        {
            int result = 0;
            string sql = "INSERT INTO karyawan (id_karyawan, nama_karyawan, alamat, nomor_telepon) VALUES (@IdKaryawan, @NamaKaryawan, @Alamat, @NoTelp)";
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@IdKaryawan", model.id_karyawan),
                new MySqlParameter("@NamaKaryawan", model.nama_karyawan),
                new MySqlParameter("@Alamat", model.alamat),
                new MySqlParameter("@NoTelp", model.nomor_telepon)
            };
            result = ExecuteNonQuery(sql, parameters);
            return result;
        }

        public int Update(KaryawanModel model)
        {
            int result = 0;
            string sql = "UPDATE karyawan SET nama_karyawan = @NamaKaryawan, alamat = @Alamat, nomor_telepon = @NoTelp WHERE id_karyawan = @IdKaryawan";
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@IdKaryawan", model.id_karyawan),
                new MySqlParameter("@NamaKaryawan", model.nama_karyawan),
                new MySqlParameter("@Alamat", model.alamat),
                new MySqlParameter("@NoTelp", model.nomor_telepon)
            };
            result = ExecuteNonQuery(sql, parameters);
            return result;
        }

        public int Delete(KaryawanModel model)
        {
            int result = 0;
            string sql = "DELETE FROM karyawan WHERE id_karyawan = @IdKaryawan";
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@IdKaryawan", model.id_karyawan)
            };
            result = ExecuteNonQuery(sql, parameters);
            return result;
        }

        public DataTable GetAll()
        {
            string sql = "SELECT * FROM karyawan ORDER BY id_karyawan";
            MySqlParameter[] parameters = [];
            DataTable dt = ExecuteDataTable(sql, parameters);
            return dt;
        }

        public KaryawanModel GetById(string id)
        {
            KaryawanModel model = new();
            string sql = "SELECT * FROM karyawan WHERE id_karyawan = @IdKaryawan";
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@IdKaryawan", id)
            };
            DataTable dt = ExecuteDataTable(sql, parameters);
            DataRow dr = dt.Rows[0];
            if (dr != null)
            {
                model.id_karyawan = dr["id_karyawan"].ToString();
                model.nama_karyawan = dr["nama_karyawan"].ToString();
                model.alamat = dr["alamat"].ToString();
                model.nomor_telepon = dr["nomor_telepon"].ToString();
            }
            return model;
        }

        public bool IsExist(string id)
        {
            string sql = "SELECT * FROM karyawan WHERE id_karyawan = @IdKaryawan";
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@IdKaryawan", id)
            };
            DataTable dt = ExecuteDataTable(sql, parameters);
            return dt.Rows.Count > 0;
        }

        public string freeId
        {
            get => GetFreeId("karyawan", "KRY", 4);
        }
    }
}
