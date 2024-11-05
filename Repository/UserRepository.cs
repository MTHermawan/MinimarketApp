using MinimarketApp.Model;
using MinimarketApp.Utilities;
using MySql.Data.MySqlClient;
using System.Data;

namespace MinimarketApp.Repository
{
    internal class UserRepository : RepositoryBase
    {
        public string Login(string username, string password)
        {
            string level = "0";
            string sql = @"select * from user where BINARY username = @Username and BINARY password = @Password";
            MySqlParameter[] parameters =
            {
                new("@Username", username),
                new("@Password", password)
            };
            MySqlDataReader reader = ExecuteReader(sql, parameters);
            if (reader == null) return "-1";

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    level = reader["id_level"].ToString();
                }
            }
            return level;
        }

        public int Insert(UserModel model)
        {
            string sql = @"INSERT INTO user (id_user, username, password, id_level, id_karyawan) VALUES (@IdUser, @Username, @Password, @IdLevel, @IdKaryawan)";
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@IdUser", model.id_user),
                new MySqlParameter("@Username", model.username),
                new MySqlParameter("@Password", model.password),
                new MySqlParameter("@IdLevel", model.id_level),
                new MySqlParameter("@IdKaryawan", model.id_karyawan)
            };
            int result = ExecuteNonQuery(sql, parameters);
            return result;
        }

        public int Update(UserModel model)
        {
            string sql = "UPDATE user SET username = @Username, password = @Password, id_level = @IdLevel, id_karyawan = @IdKaryawan WHERE id_user = @IdUser";
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@IdUser", model.id_user),
                new MySqlParameter("@Username", model.username),
                new MySqlParameter("@Password", model.password),
                new MySqlParameter("@IdLevel", model.id_level),
                new MySqlParameter("@IdKaryawan", model.id_karyawan)
            };
            int result = ExecuteNonQuery(sql, parameters);
            return result;
        }

        public int Delete(UserModel model)
        {
            string sql = @"DELETE FROM user WHERE id_user = @IdUser";
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@IdUser", model.id_user)
            };
            int result = ExecuteNonQuery(sql, parameters);
            return result;
        }

        public DataTable GetAll()
        {
            string sql = "SELECT * FROM user INNER JOIN level ON level.id_level = user.id_level INNER JOIN karyawan ON karyawan.id_karyawan = user.id_karyawan ORDER BY id_user";
            MySqlParameter[] parameters = [];
            DataTable dt = ExecuteDataTable(sql, parameters);
            return dt;
        }

        public UserModel GetCurrentUserData()
        {
            UserModel userModel = new();
            string sql = "SELECT * FROM user INNER JOIN level ON level.id_level = user.id_level INNER JOIN karyawan ON karyawan.id_karyawan = user.id_karyawan WHERE username = @Username";
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@Username", Session.Username)
            };
            MySqlDataReader reader = ExecuteReader(sql, parameters);
            if (reader == null) return userModel;
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    userModel.id_user = reader["id_user"].ToString();
                    userModel.username = reader["username"].ToString();
                    userModel.password = reader["password"].ToString();
                    userModel.id_level = reader["id_level"].ToString();
                    userModel.id_karyawan = reader["id_karyawan"].ToString();
                }
            }
            return userModel;
        }

        public bool IsExist(string id)
        {
            string sql = "SELECT * FROM user WHERE id_user = @IdUser";
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@IdUser", id)
            };
            DataTable dt = ExecuteDataTable(sql, parameters);
            return dt.Rows.Count > 0;
        }

        public string freeId
        {
            get => GetFreeId("user", "", 4); 
        }
    }
}
