using System;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace test2.Helpers 
{
    public static class DatabaseHelper
    {
        private static string _connectionString;

        static DatabaseHelper()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            _connectionString = @"Server=127.0.0.1;Database=GarageManagement;Integrated Security=True;TrustServerCertificate=True;";
        }

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }

        public static DataTable ExecuteQuery(string sql, SqlParameter[] parameters = null)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    if (parameters != null) cmd.Parameters.AddRange(parameters);
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }
            }
            return dt;
        }

        public static int ExecuteNonQuery(string sql, SqlParameter[] parameters = null)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    if (parameters != null) cmd.Parameters.AddRange(parameters);
                    conn.Open();
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        public static object ExecuteScalar(string sql, SqlParameter[] parameters = null)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    if (parameters != null) cmd.Parameters.AddRange(parameters);
                    conn.Open();
                    object result = cmd.ExecuteScalar();
                    return (result == DBNull.Value) ? null : result;
                }
            }
        }
    }
}


//VD cách láy danh sách xe
//string sql = "SELECT * FROM Vehicles";
//DataTable dt = DatabaseHelper.ExecuteQuery(sql);
//dgvVehicles.ItemsSource = dt.DefaultView;

//cách tính tiền
//string sql = "SELECT SUM(Price) FROM RepairOrderParts WHERE ID = @id";
//var params = new SqlParameter[] { new SqlParameter("@id", 121) };
//object result = DatabaseHelper.ExecuteScalar(sql, params);
//decimal total = result == null ? 0 : Convert.ToDecimal(result);