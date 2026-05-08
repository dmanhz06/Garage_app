using Dapper;
using test2.Models;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.Data.SqlClient;
using test2.Helpers;
using System;
using System.Windows; 

public class PartRepository
{
    public List<Part> GetAllParts()
    {
        try {
            using (IDbConnection db = DatabaseHelper.GetConnection())
            {
                return db.Query<Part>("SELECT * FROM Parts").ToList();
            }
        } catch (Exception ex) {
            MessageBox.Show("Lỗi lấy dữ liệu: " + ex.Message);
            return new List<Part>();
        }
    }

    public void AddPart(Part part)
    {
        try {
            using (IDbConnection db = DatabaseHelper.GetConnection())
            {
                string sql = @"INSERT INTO Parts (PartName, Price, StockQuantity, Unit, ImportPrice, MinimumStock) 
                            VALUES (@PartName, @Price, @StockQuantity, @Unit, @ImportPrice, @MinimumStock)";
                db.Execute(sql, part);
            }
        } catch (Exception ex) {
            MessageBox.Show("Lỗi khi THÊM: " + ex.Message); // Nó sẽ báo lỗi cụ thể ở đây
        }
    }

    public void UpdatePart(Part part)
    {
        try {
            using (IDbConnection db = DatabaseHelper.GetConnection())
            {
                // Cập nhật đầy đủ các cột đã nâng cấp
                string sql = @"UPDATE Parts SET 
                                PartName=@PartName, 
                                Unit=@Unit, 
                                Price=@Price, 
                                StockQuantity=@StockQuantity, 
                                ImportPrice=@ImportPrice, 
                                MinimumStock=@MinimumStock 
                              WHERE PartID=@PartID";
                db.Execute(sql, part);
            }
        } catch (Exception ex) {
            MessageBox.Show("Lỗi khi SỬA: " + ex.Message);
        }
    }

    public void DeletePart(int partId)
    {
        try {
            using (IDbConnection db = DatabaseHelper.GetConnection())
            {
                db.Execute("DELETE FROM Parts WHERE PartID = @PartID", new { PartID = partId });
            }
        } catch (Exception ex) {
            MessageBox.Show("Lỗi khi XÓA: " + ex.Message);
        }
    }

    public List<Part> GetLowStockParts()
{
    using (IDbConnection db = DatabaseHelper.GetConnection())
    {
        // Lấy những phụ tùng có số lượng tồn kho nhỏ hơn hoặc bằng mức tối thiểu
        string sql = "SELECT * FROM Parts WHERE StockQuantity <= MinimumStock";
        return db.Query<Part>(sql).ToList();
    }
}
}