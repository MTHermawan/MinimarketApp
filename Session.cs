namespace MinimarketApp
{
    public static class Session
    {
        public static string? Username { get; set; }
        public static string? Level { get; set; }
        public static string? Id { get; set; }

        public static void Destroy()
        {
            Username = null;
            Level = null;
            Id = null;
        }

        public static string EditedIdTransaksi = "";
        public static bool isLoadedTransaksi = false;

        public static string EditedIdTransaksiPembelian = "";
        public static bool isLoadedTransaksiPembelian = false;

        public static bool IsAdmin() => Level == "1";

        public static bool IsKasir() => Level == "2";

        public static bool IsLoggedIn()
        {
            return !string.IsNullOrEmpty(Username);
        }

    }
}
