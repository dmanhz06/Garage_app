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
            // Tự động đọc chuỗi kết nối từ file appsettings.json
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            _connectionString = configuration.GetConnectionString("GarageManagement");
        }

        // 1. Hàm dùng để lấy dữ liệu (Dành cho SELECT - trả về DataTable để đổ vào DataGrid)
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

        // 2. Hàm dùng để Thêm, Sửa, Xóa (Dành cho INSERT, UPDATE, DELETE)
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

        // 3. Hàm lấy một giá trị duy nhất (Dành cho SUM, COUNT, MAX)
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