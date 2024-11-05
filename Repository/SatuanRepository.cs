using MinimarketApp.Model;
using MinimarketApp.Utilities;
using MySql.Data.MySqlClient;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;

namespace MinimarketApp.Repository
{
    public class SatuanRepository : RepositoryBase
    {
        public int Insert(SatuanModel model)
        {
            int result = 0;
            string sql = "INSERT INTO satuan (id_satuan, nama_satuan) VALUES (@IdSatuan, @NamaSatuan)";
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@IdSatuan", model.id_satuan),
                new MySqlParameter("@NamaSatuan", model.nama_satuan)
            };
            result = ExecuteNonQuery(sql, parameters);
            return result;
        }

        public int Update(SatuanModel model)
        {
            int result = 0;
            string sql = "UPDATE satuan SET nama_satuan = @NamaSatuan WHERE id_satuan = @IdSatuan";
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@IdSatuan", model.id_satuan),
                new MySqlParameter("@NamaSatuan", model.nama_satuan)
            };
            result = ExecuteNonQuery(sql, parameters);
            return result;
        }

        public int Delete(SatuanModel model)
        {
            int result = 0;
            string sql = "DELETE FROM satuan WHERE id_satuan = @IdSatuan";
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@IdSatuan", model.id_satuan)
            };
            result = ExecuteNonQuery(sql, parameters);
            return result;
        }

        public DataTable GetAll()
        {
            string sql = "SELECT * FROM satuan ORDER BY id_satuan";
            MySqlParameter[] parameters = [];
            DataTable dt = ExecuteDataTable(sql, parameters);
            return dt;
        }

        public DataTable GetProdukSatuanPenjualan(string id_produk)
        {
            DataTable dt = new();
            string sql = "SELECT satuan.id_satuan, satuan.nama_satuan, produk.harga_jual, produk.stok FROM satuan RIGHT JOIN produk ON produk.id_satuan = satuan.id_satuan WHERE produk.id_produk = @IdProduk UNION SELECT satuan.id_satuan, satuan.nama_satuan, grup_produk.harga_jual_unit AS harga_jual, FLOOR(produk.stok / grup_produk.kuantitas_produk) AS stok FROM satuan RIGHT JOIN grup_produk ON grup_produk.id_satuan = satuan.id_satuan LEFT JOIN produk ON produk.id_produk = grup_produk.id_produk WHERE grup_produk.id_produk = @IdProduk;";
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@IdProduk", id_produk)
            };
            dt = ExecuteDataTable(sql, parameters);
            return dt;
        }

        public DataTable GetProdukSatuanPembelian(string id_produk)
        {
            DataTable dt = new();
            string sql = "SELECT satuan.id_satuan, satuan.nama_satuan FROM satuan RIGHT JOIN produk ON produk.id_satuan = satuan.id_satuan WHERE produk.id_produk = @IdProduk UNION SELECT satuan.id_satuan, satuan.nama_satuan FROM satuan RIGHT JOIN grup_produk ON grup_produk.id_satuan = satuan.id_satuan LEFT JOIN produk ON produk.id_produk = grup_produk.id_produk WHERE grup_produk.id_produk = @IdProduk;";
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@IdProduk", id_produk)
            };
            dt = ExecuteDataTable(sql, parameters);
            return dt;
        }

        public int GrupProdukKuantitas(string id_produk, string id_satuan)
        {
            int result = 0;
            string sql = "SELECT kuantitas_produk FROM grup_produk WHERE id_produk = @IdProduk AND id_satuan = @IdSatuan";
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@IdProduk", id_produk),
                new MySqlParameter("@IdSatuan", id_satuan)
            };
            DataTable dt = ExecuteDataTable(sql, parameters);
            if (dt.Rows.Count > 0)
            {
                result = (int)dt.Rows[0][0];
            }
            return result;
        }

        public bool IsExist(string id)
        {
            string sql = "SELECT * FROM satuan WHERE id_satuan = @IdSatuan";
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@IdSatuan", id)
            };
            DataTable dt = ExecuteDataTable(sql, parameters);
            return dt.Rows.Count > 0;
        }

        public string freeId
        {
            get => GetFreeId("satuan", "STN", 3);
        }
    }
}
