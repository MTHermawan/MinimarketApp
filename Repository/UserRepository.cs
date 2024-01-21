using MinimarketApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Windows;

namespace MinimarketApp.Repository
{
    internal class UserRepository
    {
        public string Login(string username, string password)
        {
            string level = "";
            string sql = @"select * from user where username = @Username and password = @Password";
            using MySqlConnection conn = new(DbConfig.ConnString);
            try {
                using MySqlCommand cmd = new(sql, conn);
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Password", password);
                try
                {
                    conn.Open();
                    using MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        if (reader.HasRows)
                        {
                            Console.WriteLine(reader["id_level"].ToString());
                            level = reader["id_level"].ToString();
                        }
                        reader.Close();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return level;
        }
    }
}
