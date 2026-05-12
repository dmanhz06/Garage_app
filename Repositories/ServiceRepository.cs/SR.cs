using System.Data;
using Microsoft.Data.SqlClient;
using test2.Helpers;

namespace test2.Repositories
{
    public class ServiceRepository
    {
        // 1. Lấy danh sách dịch vụ
        public DataTable GetAllServices(string search = "")
        {
            string sql = "SELECT ServiceID, ServiceName, Price FROM Services WHERE ServiceName LIKE @search";
            var parameters = new SqlParameter[] { new SqlParameter("@search", "%" + search + "%") };
            return test2.Helpers.DatabaseHelper.ExecuteQuery(sql, parameters);
        }

        // 2. Thêm dịch vụ mới
        public void AddService(string name, decimal price)
        {
            string sql = "INSERT INTO Services (ServiceName, Price) VALUES (@name, @price)";
            var parameters = new SqlParameter[] {
                new SqlParameter("@name", name),
                new SqlParameter("@price", price)
            };
            test2.Helpers.DatabaseHelper.ExecuteNonQuery(sql, parameters);
        }

        // 3. Sửa dịch vụ
        public void UpdateService(int id, string name, decimal price)
        {
            string sql = "UPDATE Services SET ServiceName = @name, Price = @price WHERE ServiceID = @id";
            var parameters = new SqlParameter[] {
                new SqlParameter("@name", name),
                new SqlParameter("@price", price),
                new SqlParameter("@id", id)
            };
            test2.Helpers.DatabaseHelper.ExecuteNonQuery(sql, parameters);
        }

        // 4. Xóa dịch vụ
        public void DeleteService(int id)
        {
            string sql = "DELETE FROM Services WHERE ServiceID = @id";
            var param = new SqlParameter[] { new SqlParameter("@id", id) };
            test2.Helpers.DatabaseHelper.ExecuteNonQuery(sql, param);
        }
    }
}