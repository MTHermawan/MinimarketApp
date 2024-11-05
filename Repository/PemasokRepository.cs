using MinimarketApp.Model;
using MinimarketApp.Utilities;
using MySql.Data.MySqlClient;
using System.Data;

namespace MinimarketApp.Repository
{
    public class PemasokRepository : RepositoryBase
    {
        public int Insert(PemasokModel model)
        {
            int result = 0;
            string sql = "INSERT INTO pemasok (id_pemasok, nama_pemasok, alamat, nomor_telepon) VALUES (@IdPemasok, @NamaPemasok, @Alamat, @NoTelp)";
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@IdPemasok", model.id_pemasok),
                new MySqlParameter("@NamaPemasok", model.nama_pemasok),
                new MySqlParameter("@Alamat", model.alamat),
                new MySqlParameter("@NoTelp", model.nomor_telepon)
            };
            result = ExecuteNonQuery(sql, parameters);
            return result;
        }

        public int Update(PemasokModel model)
        {
            int result = 0;
            string sql = "UPDATE pemasok SET nama_pemasok = @NamaPemasok, alamat = @Alamat, nomor_telepon = @NoTelp WHERE id_pemasok = @IdPemasok";
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@IdPemasok", model.id_pemasok),
                new MySqlParameter("@NamaPemasok", model.nama_pemasok),
                new MySqlParameter("@Alamat", model.alamat),
                new MySqlParameter("@NoTelp", model.nomor_telepon)
            };
            result = ExecuteNonQuery(sql, parameters);
            return result;
        }

        public int Delete(PemasokModel model)
        {
            int result = 0;
            string sql = "DELETE FROM pemasok WHERE id_pemasok = @IdPemasok";
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@IdPemasok", model.id_pemasok)
            };
            result = ExecuteNonQuery(sql, parameters);
            return result;
        }

        public DataTable GetAll(string text = "")
        {
            string sql = $"SELECT *, CONCAT(id_pemasok, ' - ', nama_pemasok) as ComboPemasokView FROM pemasok HAVING ComboPemasokView LIKE '%{text}%' ORDER BY id_pemasok";
            MySqlParameter[] parameters = [];
            DataTable dt = ExecuteDataTable(sql, parameters);
            return dt;
        }

        public bool IsExist(string id)
        {
            string sql = "SELECT * FROM pemasok WHERE id_pemasok = @IdPemasok";
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@IdPemasok", id)
            };
            DataTable dt = ExecuteDataTable(sql, parameters);
            return dt.Rows.Count > 0;
        }

        public string freeId
        {
            get => GetFreeId("pemasok", "SUP", 4);
        }
    }
}
