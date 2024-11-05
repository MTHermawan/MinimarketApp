using MinimarketApp.Model;
using MinimarketApp.Utilities;
using MySql.Data.MySqlClient;
using System.Data;

namespace MinimarketApp.Repository
{
    public class PelangganRepository : RepositoryBase
    {
        public int Insert(PelangganModel model)
        {
            string sql = @"INSERT INTO pelanggan (nama_pelanggan, alamat, telp_pelanggan, jenis_kelamin) VALUES (@NamaPelanggan, @AlamatPelanggan, @TelpPelanggan, @JenisKelamin)";
            MySqlParameter[] parameters =
            [
                new MySqlParameter("@NamaPelanggan", model.nama_pelanggan),
                new MySqlParameter("@AlamatPelanggan", model.alamat),
                new MySqlParameter("@TelpPelanggan", model.telp_pelanggan),
                new MySqlParameter("@JenisKelamin", model.jenis_kelamin)
            ];
            int result = ExecuteNonQuery(sql, parameters);
            return result;
        }

        public int Update(PelangganModel model)
        {
            string sql = @"UPDATE pelanggan SET nama_pelanggan = @NamaPelanggan, alamat = @AlamatPelanggan, jenis_kelamin = @JenisKelamin WHERE telp_pelanggan = @TelpPelanggan";
            MySqlParameter[] parameters =
            [
                new MySqlParameter("@NamaPelanggan", model.nama_pelanggan),
                new MySqlParameter("@AlamatPelanggan", model.alamat),
                new MySqlParameter("@TelpPelanggan", model.telp_pelanggan),
                new MySqlParameter("@JenisKelamin", model.jenis_kelamin)
            ];
            int result = ExecuteNonQuery(sql, parameters);
            return result;
        }

        public int Delete(PelangganModel model)
        {
            string sql = @"delete from pelanggan where telp_pelanggan = @TelpPelanggan";
            MySqlParameter[] parameters =
            [
                new MySqlParameter("@TelpPelangganNomorTelepon", model.telp_pelanggan)
            ];
            int result = ExecuteNonQuery(sql, parameters);
            return result;
        }
        
        public DataTable GetAll(string text = "")
        {
            string sql = $"SELECT *, CONCAT(telp_pelanggan, ' - ', nama_pelanggan) AS ComboPelangganView FROM pelanggan HAVING ComboPelangganView LIKE '%{text}%' ORDER BY nama_pelanggan;";
            MySqlParameter[] parameters = [];
            DataTable dt = ExecuteDataTable(sql, parameters);
            return dt;
        }

        public bool IsExist(string telp_pelanggan)
        {
            string sql = @"select * from pelanggan where telp_pelanggan = @TelpPelanggan";
            MySqlParameter[] parameters =
            [
                new MySqlParameter("@TelpPelanggan", telp_pelanggan)
            ];
            DataTable dt = ExecuteDataTable(sql, parameters);
            return dt.Rows.Count > 0;
        }

        public List<string> GetJenisKelamin()
        {
            return GetFieldEnum("pelanggan", "jenis_kelamin");
        }
    }
}
