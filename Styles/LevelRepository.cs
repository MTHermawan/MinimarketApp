using MinimarketApp.Model;
using MinimarketApp.Utilities;
using MySql.Data.MySqlClient;
using System.Data;

namespace MinimarketApp.Repository
{
    public class LevelRepository : RepositoryBase
    {
        public int Insert(LevelModel model)
        {
            string sql = @"insert into level (id_level, nama_level) values (@IdLevel, @NamaLevel)";
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@IdLevel", model.id_level),
                new MySqlParameter("@NamaLevel", model.nama_level)
            };
            int result = ExecuteNonQuery(sql, parameters);
            return result;
        }

        public int Update(LevelModel model)
        {
            string sql = @"update level set nama_level = @NamaLevel where id_level = @IdLevel";
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@IdLevel", model.id_level),
                new MySqlParameter("@NamaLevel", model.nama_level)
            };
            int result = ExecuteNonQuery(sql, parameters);
            return result;
        }

        public int Delete(LevelModel model)
        {
            string sql = @"delete from level where id_level = @IdLevel";
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@IdLevel", model.id_level)
            };
            int result = ExecuteNonQuery(sql, parameters);
            return result;
        }

        public DataTable GetAll()
        {
            string sql = "SELECT * FROM level ORDER BY id_level";
            MySqlParameter[] parameters = [];
            DataTable dt = ExecuteDataTable(sql, parameters);
            return dt;
        }

        public bool IsExist(string id)
        {
            string sql = @"select * from level where id_level = @IdLevel";
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@IdLevel", id)
            };
            DataTable dt = ExecuteDataTable(sql, parameters);
            return dt.Rows.Count > 0;
        }
        public string freeId
        {
            get => GetFreeId("level", "", 1);
        }
    }
}
