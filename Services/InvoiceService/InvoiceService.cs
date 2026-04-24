using Microsoft.Data.SqlClient;
using System;

namespace test2.Services
{
    public class InvoiceService
    {
        // Nhớ thay chuỗi kết nối của bạn vào đây
        private string _connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=GarageManagement;Integrated Security=True;TrustServerCertificate=True";
        
        public void CreateInvoice(int repairId, decimal labor)
        {
            // 1. Lấy tổng tiền phụ tùng từ bảng chi tiết phiếu sửa chữa
            string sqlGetParts = "SELECT SUM(Quantity * PriceAtTime) FROM RepairOrderParts WHERE RepairOrderID = @id";

            // 2. Chèn vào bảng Invoices mới
            string sqlInsert = "INSERT INTO Invoices (RepairOrderID, LaborCost, PartsTotal) VALUES (@id, @labor, @parts)";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmdParts = new SqlCommand(sqlGetParts, conn);
                cmdParts.Parameters.AddWithValue("@id", repairId);
                object result = cmdParts.ExecuteScalar();
                decimal partsTotal = (result == null || result == DBNull.Value) ? 0 : Convert.ToDecimal(result);
                SqlCommand cmdInsert = new SqlCommand(sqlInsert, conn);
                cmdInsert.Parameters.AddWithValue("@id", repairId);
                cmdInsert.Parameters.AddWithValue("@labor", labor);
                cmdInsert.Parameters.AddWithValue("@parts", partsTotal);
                cmdInsert.ExecuteNonQuery();
            }
        }

        public List<dynamic> GetAllInvoices()
        {
            var list = new List<dynamic>();

            string sql = @"SELECT i.InvoiceID AS MaHD, 
                          c.FullName AS TenKH, 
                          v.LicensePlate AS BienSo, 
                          i.TotalAmount AS TongTien,
                          i.Status
                   FROM Invoices i
                   JOIN RepairOrders ro ON i.RepairOrderID = ro.RepairOrderID
                   JOIN Vehicles v ON ro.VehicleID = v.VehicleID
                   JOIN Customers c ON v.CustomerID = c.CustomerID";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(new
                    {
                        MaHD = reader["MaHD"],
                        TenKH = reader["TenKH"],
                        BienSo = reader["BienSo"],
                        TongTien = string.Format("{0:N0}đ", reader["TongTien"] == DBNull.Value ? 0 : reader["TongTien"]),
                        Status = reader["Status"]
                    });
                }
            }
            return list;
        }
    }
}