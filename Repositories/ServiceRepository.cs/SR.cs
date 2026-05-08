using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;

namespace test2.Repositories.ServiceRepository
{
    public class SR
    {
        public List<Models.Services> GetAllServices()
        {
            var list = new List<Models.Services>();
            string sql = "SELECT * FROM Services";

            // Dùng tên đầy đủ để máy không lú
            DataTable dt = Helpers.DatabaseHelper.ExecuteQuery(sql);

            foreach (DataRow row in dt.Rows)
            {
                list.Add(new Models.Services
                {
                    ServiceID = Convert.ToInt32(row["ServiceID"]),
                    ServiceName = row["ServiceName"]?.ToString() ?? "",
                    Price = row["Price"] != DBNull.Value ? Convert.ToDecimal(row["Price"]) : 0
                });
            }
            return list;
        }

        public bool AddService(Models.Services service)
        {
            string sql = "INSERT INTO Services (ServiceName, Price) VALUES (@Name, @Price)";
            SqlParameter[] parameters = {
                new SqlParameter("@Name", service.ServiceName),
                new SqlParameter("@Price", service.Price)
            };
            return Helpers.DatabaseHelper.ExecuteNonQuery(sql, parameters) > 0;
        }

        public bool DeleteService(int id)
        {
            string sql = "DELETE FROM Services WHERE ServiceID = @Id";
            SqlParameter[] parameters = { new SqlParameter("@Id", id) };
            return Helpers.DatabaseHelper.ExecuteNonQuery(sql, parameters) > 0;
        }
    }
}