using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System.IO;

namespace MinimarketApp.Data
{
    internal class DbConfig
    {
        public required string Server { get; set; }
        public required string Database { get; set; }
        public required string User { get; set; }
        public required string Password { get; set; }
        
        public static string? ConnString
        {
            get
            {
                DbConfig? config = ReadConfig();
                if (config == null) return null;
                return $"server={config.Server};database={config.Database};user={config.User};password={config.Password}";
            }   
        }

        public static DbConfig? ReadConfig()
        {
            string path = "D:\\Sekolah\\USK\\MinimarketApp\\.env.json";
            try
            {
                string json = File.ReadAllText(path);
                return JsonConvert.DeserializeObject<DbConfig>(json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading config file: {ex.Message}");
                return null;
            }
        }
    }
}
