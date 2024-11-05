using MinimarketApp.Model;
using MinimarketApp.Utilities;
using MySql.Data.MySqlClient;
using System.Data;

namespace MinimarketApp.Repository
{
    internal class KategoriRepository : RepositoryBase
    {
        public int Insert(KategoriModel model)
        {
            string sql = @"insert into kategori (id_kategori, nama_kategori) values (@IdKategori, @NamaKategori)";
            MySqlParameter[] parameters =
            [
                new MySqlParameter("@IdKategori", model.id_kategori),
                new MySqlParameter("@NamaKategori", model.nama_kategori)
            ];
            int result = ExecuteNonQuery(sql, parameters);
            return result;
        }

        public int Update(KategoriModel model)
        {
            string sql = @"update kategori set nama_kategori = @NamaKategori where id_kategori = @IdKategori";
            MySqlParameter[] parameters =
            [
                new MySqlParameter("@IdKategori", model.id_kategori),
                new MySqlParameter("@NamaKategori", model.nama_kategori)
            ];
            int result = ExecuteNonQuery(sql, parameters);
            return result;
        }

        public int Delete(KategoriModel model)
        {
            string sql = @"delete from kategori where id_kategori = @IdKategori";
            MySqlParameter[] parameters =
            [
                new MySqlParameter("@IdKategori", model.id_kategori)
            ];
            int result = ExecuteNonQuery(sql, parameters);
            return result;
        }
        
        public DataTable GetAll()
        {
            string sql = @"select * from kategori ORDER BY id_kategori";
            MySqlParameter[] parameters = [];
            DataTable dt = ExecuteDataTable(sql, parameters);
            return dt;
        }

        public bool IsExist(string id)
        {
            string sql = @"select * from kategori where id_kategori = @IdKategori";
            MySqlParameter[] parameters =
            [
                new MySqlParameter("@IdKategori", id)
            ];
            DataTable dt = ExecuteDataTable(sql, parameters);
            return dt.Rows.Count > 0;
        }

        public string freeId
        {
            get => GetFreeId("kategori", "KTG", 3);
        }
    }
}
