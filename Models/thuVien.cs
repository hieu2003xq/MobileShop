using ado.Areas.Admin.Controllers;
using ado.caching;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ado.Models
{
    public class thuVien
    {
        private readonly string connectionString;
        private readonly ConnectionMultiplexer _redis;
        public thuVien()
        {
            connectionString = ConfigurationManager.ConnectionStrings["banSanPham"].ConnectionString;
            _redis = stackExchange.Connection;
        }

        public DataTable ExecuteQuery(string query, SqlParameter[] parameters = null)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        return dataTable;
                    }
                }
            }
        }


        public int ExecuteNonQuery(string query, SqlParameter[] parameters = null)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    return command.ExecuteNonQuery();
                }
            }
        }

        // Thêm dữ liệu cho bảng cụ thể
        public int them(string tableName, SqlParameter[] parameters)
        {
            string columns = String.Join(", ", parameters.Select(p => p.ParameterName.Replace("@", "")));
            string values = String.Join(", ", parameters.Select(p => p.ParameterName));
            string query = $"INSERT INTO {tableName} ({columns}) VALUES ({values})";
            removeCahe(tableName);
            return ExecuteNonQuery(query, parameters);
        }

        // Sửa dữ liệu cho bảng cụ thể
        public int sua(string tableName, SqlParameter[] parameters)
        {
            string columns = String.Join(", ", parameters.Select(p => p.ParameterName.Replace("@", "")));
            string values = String.Join(", ", parameters.Select(p => p.ParameterName));
            string query = $"UPDATE {tableName} SET {ganGT(parameters)} WHERE {ganDK(parameters)}";
            removeCahe(tableName);
            return ExecuteNonQuery(query, parameters);
        }

        // Xóa dữ liệu cho bảng cụ thể
        public int xoa(string tableName, SqlParameter[] parameters)
        {
            string query = $"DELETE FROM {tableName} WHERE {ganDK(parameters)}";
            removeCahe(tableName);
            return ExecuteNonQuery(query, parameters);
        }
        private string ganGT(SqlParameter[] parameters)
        {
            List<string> values = new List<string>();
            foreach(SqlParameter parameter in parameters)
            {
                values.Add($"{parameter.ParameterName.Replace("@", "")} = {parameter.ParameterName}");
            }
            return string.Join(", ", values);
        }
        private string ganDK(SqlParameter[] parameters)
        {
            var x = parameters[0];
            return $"{x.ParameterName.Replace("@", "")} = {x.ParameterName}";
        }
        private void removeCahe(string tableName)
        {
            if (tableName == "products")
            {
                IDatabase cache = _redis.GetDatabase();
                string keyPattern = $"/QLsanPham/*";
                var keys = _redis.GetServer(_redis.GetEndPoints()[0]).Keys(database: cache.Database, pattern: keyPattern);
                foreach (var key in keys)
                {
                    cache.KeyDelete(key);
                }


            }
            else if (tableName == "chiTietDon" || tableName== "khachHang" || tableName=="donDat1")
            {
                IDatabase cache = _redis.GetDatabase();
                string keyPattern = $"/QLdonHang/*";
                var keys = _redis.GetServer(_redis.GetEndPoints()[0]).Keys(database: cache.Database, pattern: keyPattern);
                foreach (var key in keys)
                {
                    cache.KeyDelete(key);
                }
            }
        }
        public int themORsua(string tableName, SqlParameter[] parameters,string[] capNhat)
        {
            var x = parameters[0];
            string columns = String.Join(", ", parameters.Select(p => p.ParameterName.Replace("@", "")));
            string values = String.Join(", ", parameters.Select(p => p.ParameterName));
            string check =  $"target.{x.ParameterName.Replace("@", "")} = source.{x.ParameterName.Replace("@", "")}";
            string update = String.Join(", ", capNhat.Select(column => $"target.{column} = target.{column} + source.{column}"));
            string query = $@"
                MERGE INTO {tableName} AS target
                USING (
                    SELECT {values}
                ) AS source ({columns})
                ON {check}
                WHEN MATCHED THEN
                    UPDATE SET {update}
                WHEN NOT MATCHED THEN
                    INSERT ({columns})
                    VALUES ({values});
            ";
            return ExecuteNonQuery(query,parameters);
        }
    }
}