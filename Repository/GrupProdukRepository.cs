using MinimarketApp.Model;
using MinimarketApp.Utilities;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;

namespace MinimarketApp.Repository
{
    public class GrupProdukRepository : RepositoryBase
    {
        public int Insert(GrupProdukModel grupProduk)
        {
            string sql = "INSERT INTO grup_produk (id_grup_produk, id_produk, id_satuan, barcode_unit, kuantitas_produk, harga_jual_unit) VALUES (@id_grup_produk, @id_produk, @id_satuan, @barcode_unit, @kuantitas_produk, @harga_jual_unit)";
            MySqlParameter[] parameters =
            {
                new("@id_grup_produk", grupProduk.id_grup_produk),
                new("@id_produk", grupProduk.produk.id_produk),
                new("@id_satuan", grupProduk.satuan.id_satuan),
                new("@barcode_unit", grupProduk.barcode_unit),
                new("@kuantitas_produk", grupProduk.kuantitas_produk),
                new("@harga_jual_unit", grupProduk.harga_jual_unit)
            };
            return ExecuteNonQuery(sql, parameters);
        }

        public int Update(GrupProdukModel grupProduk)
        {
            string sql = "UPDATE grup_produk SET id_produk = @id_produk, id_satuan = @id_satuan, barcode_unit = @barcode_unit, kuantitas_produk = @kuantitas_produk, harga_jual_unit = @harga_jual_unit WHERE id_grup_produk = @id_grup_produk";
            MySqlParameter[] parameters =
            {
                new("@id_grup_produk", grupProduk.id_grup_produk),
                new("@id_produk", grupProduk.produk.id_produk),
                new("@id_satuan", grupProduk.satuan.id_satuan),
                new("@barcode_unit", grupProduk.barcode_unit),
                new("@kuantitas_produk", grupProduk.kuantitas_produk),
                new("@harga_jual_unit", grupProduk.harga_jual_unit)
            };
            return ExecuteNonQuery(sql, parameters);
        }

        public int Delete(GrupProdukModel grupProduk)
        {
            string sql = "DELETE FROM grup_produk WHERE id_grup_produk = @id_grup_produk";
            MySqlParameter[] parameters =
            {
                new("@id_grup_produk", grupProduk.id_grup_produk)
            };
            return ExecuteNonQuery(sql, parameters);
        }

        public DataTable GetAll()
        {
            DataTable dt = new();
            string sql = "SELECT *, produk_satuan.nama_satuan produk_nama_satuan, grup_satuan.nama_satuan grup_nama_satuan FROM grup_produk LEFT JOIN produk ON produk.id_produk = grup_produk.id_produk LEFT JOIN satuan grup_satuan ON grup_satuan.id_satuan = grup_produk.id_satuan LEFT JOIN satuan produk_satuan ON produk_satuan.id_satuan = produk.id_satuan";
            MySqlParameter[] parameters = { };

            dt = ExecuteDataTable(sql, parameters);
            DataColumn formatted_tanggal_transaksi = new("formatted_harga_jual_unit", typeof(string));
            DataColumn formatted_kuantitas_produk = new("formatted_kuantitas_produk", typeof(string));
            dt.Columns.Add(formatted_tanggal_transaksi);
            dt.Columns.Add(formatted_kuantitas_produk);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    row["formatted_harga_jual_unit"] = FormattedCurrency(row.Field<float>("harga_jual_unit"));
                    row["formatted_kuantitas_produk"] = row.Field<int>("kuantitas_produk").ToString() + " " + row.Field<string>("produk_nama_satuan");
                }
            }
            return dt;
        }

        public bool IsExist(string id_grup_produk)
        {
            DataTable dt = new();
            string sql = "SELECT * FROM grup_produk WHERE id_grup_produk = @id_grup_produk";
            MySqlParameter[] parameters =
            {
                new("@id_grup_produk", id_grup_produk)
            };
            dt = ExecuteDataTable(sql, parameters);
            return dt.Rows.Count > 0;
        }

        public bool IsBarcodeExist(string barcode_unit, string id_grup_produk)
        {
            DataTable dt = new();
            string sql = "SELECT * FROM grup_produk WHERE barcode_unit = @barcode_unit AND id_grup_produk != @id_grup_produk";
            MySqlParameter[] parameters =
            {
                new("@barcode_unit", barcode_unit),
                new("@id_grup_produk", id_grup_produk)
            };
            dt = ExecuteDataTable(sql, parameters);
            return dt.Rows.Count > 0;
        }

        public string freeId
        {
            get => GetFreeId("grup_produk", "GPRD", 7);
        }
    }
}
