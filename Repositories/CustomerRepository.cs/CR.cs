using Dapper;
using test2.Models;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using test2.Models;

public class CustomerRepository
{
    public List<Customer> GetAllCustomers()
    {
        using (IDbConnection db = DatabaseHelper.GetConnection())
        {
            // Chỉ cần 1 dòng lệnh này là lấy được toàn bộ danh sách
            return db.Query<Customer>("SELECT * FROM Customers").ToList();
        }
    }

    public void AddCustomer(Customer customer)
    {
        using (IDbConnection db = DatabaseHelper.GetConnection())
        {
            string sql = "INSERT INTO Customers (FullName, PhoneNumber, Address) VALUES (@FullName, @PhoneNumber, @Address)";
            db.Execute(sql, customer);
        }
    }
}   