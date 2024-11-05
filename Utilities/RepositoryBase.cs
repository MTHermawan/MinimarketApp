using MinimarketApp.Data;
using MySql.Data.MySqlClient;
using System.Data;
using System.Globalization;
using System.Text.RegularExpressions;

namespace MinimarketApp.Utilities
{
    public class RepositoryBase
    {
        protected string connectionString = DbConfig.ConnString;

        protected MySqlConnection GetOpenConnection()
        {
            MySqlConnection connection = new MySqlConnection(connectionString);
            try
            {
                connection.Open();
            }
            catch (MySqlException ex)
            {
                string msg = "";
                switch (ex.Number)
                {
                    case 0:
                        msg = "Tidak dapat terhubung ke server database.";
                        break;
                    case 1042:
                        msg = "Tidak dapat terhubung ke server database.";
                        break;
                    default:
                        Console.WriteLine(ex.Message);
                        break;
                }
            }
            return connection;
        }


        protected void CloseConnection(MySqlConnection connection)
        {
            connection.Close();
        }

        protected int ExecuteNonQuery(string sql, MySqlParameter[] parameters)
        {
            int result = 0;
            var connection = GetOpenConnection();
            using var command = new MySqlCommand(sql, connection);
            try
            {
                if (connection.State == ConnectionState.Open)
                {
                    command.Parameters.AddRange(parameters);
                    result = command.ExecuteNonQuery();
                    return result;
                }
            }
            catch (MySqlException ex)
            {
                throw new Exception(ex.Message);
            }
            return result;
        }

        protected MySqlDataReader ExecuteReader(string sql, MySqlParameter[] parameters)
        {
            MySqlDataReader reader = null;
            var connection = GetOpenConnection();
            using var command = new MySqlCommand(sql, connection);
            try
            {
                if (connection.State == ConnectionState.Open)
                {
                    command.Parameters.AddRange(parameters);
                    reader = command.ExecuteReader();
                    return reader;
                }
            }
            catch (MySqlException ex)
            {
                // throw new Exception(ex.Message);
            }
            return reader;
        }

        protected object ExecuteScalar(string sql, MySqlParameter[] parameters)
        {
            object result = null;
            var connection = GetOpenConnection();
            using var command = new MySqlCommand(sql, connection);
            try
            {
                if (connection.State == ConnectionState.Open)
                {
                    command.Parameters.AddRange(parameters);
                    result = command.ExecuteScalar();
                    return result;
                }
            }
            catch (MySqlException ex)
            {
                // throw new Exception(ex.Message);
            }
            return result;
        }

        protected MySqlDataAdapter ExecuteAdapter(string sql, MySqlParameter[] parameters)
        {
            var connection = GetOpenConnection();
            using var command = new MySqlCommand(sql, connection);
            try
            {
                command.Parameters.AddRange(parameters);
                return new MySqlDataAdapter(command);
            }
            catch (MySqlException ex)
            {
                // throw new Exception(ex.Message);
                return null;
            }
        }

        protected DataTable ExecuteDataTable(string sql, MySqlParameter[] parameters)
        {
            var connection = GetOpenConnection();
            DataTable dataTable = new DataTable();
            using var command = new MySqlCommand(sql, connection);
            try
            {
                command.Parameters.AddRange(parameters);
                using var adapter = new MySqlDataAdapter(command);
                adapter.Fill(dataTable);
                return dataTable;
            }
            catch (MySqlException ex)
            {
                // throw new Exception(ex.Message);
                return dataTable;
            }
        }

        public List<string> GetFieldEnum(string tableName, string fieldName)
        {
            string sql = $"SHOW COLUMNS FROM {tableName} WHERE FIELD = '{fieldName}'";
            MySqlParameter[] parameters = new MySqlParameter[0];
            DataTable dt = ExecuteDataTable(sql, parameters);

            List<string> enumValues = new List<string>();
            foreach (DataRow row in dt.Rows)
            {
                string enumValue = row["Type"].ToString();
                enumValue = enumValue.Replace("enum(", "");
                enumValue = enumValue.Replace(")", "");
                enumValue = enumValue.Replace("'", "");
                enumValues = enumValue.Split(',').ToList();
            }

            return enumValues;
        }

        protected string GetFreeId(string nama_tabel, string prefix, int digit)
        {
            string sql = $"SELECT id_{nama_tabel} FROM {nama_tabel} ORDER BY id_{nama_tabel} DESC LIMIT 1";
            MySqlParameter[] parameters = { };
            DataTable dt = ExecuteDataTable(sql, parameters);
            string id = "";
            if (dt.Rows.Count > 0)
            {
                id = dt.Rows[0][0].ToString();
            }
            string newId = "";
            if (id == "")
            {
                newId = prefix + "1".PadLeft(digit, '0');
            }
            else
            {
                string number = Regex.Match(id, @"\d+").Value;
                int newNumber = int.Parse(number) + 1;
                newId = prefix + newNumber.ToString().PadLeft(digit, '0');
            }
            return newId;
        }

        protected string FormattedSqlDateTime(DateTime dateTime)
        {
            CultureInfo culture = new CultureInfo("id-ID");
            string formattedDateTime = dateTime.ToString("dd MMMM yyyy, HH:mm", culture);
            return formattedDateTime;
        }
        
        protected string FormattedDate(DateTime dateTime)
        {
            CultureInfo culture = new CultureInfo("id-ID");
            string formattedDateTime = dateTime.ToString("dd MMMM yyyy", culture);
            return formattedDateTime;
        }

        protected string FormattedSqlDate(DateTime dateTime)
        {
            CultureInfo culture = new CultureInfo("id-ID");
            string formattedDateTime = dateTime.ToString("yyyy-MM-dd", culture);
            return formattedDateTime;
        }

        protected string FormattedCurrency(float amount)
        {
            CultureInfo culture = new CultureInfo("id-ID");
            return amount.ToString("C", culture);
        }

        protected float CurrencyToFloat(string amount)
        {
            string cleanAmount = amount.Replace("Rp", "").Replace(".", "").Replace(",", "").Trim();
            return float.Parse(cleanAmount);
        }

        protected int ParseInt(string value)
        {
            int result = 0;
            int.TryParse(value, out result);
            return result;
        }
    }
}
