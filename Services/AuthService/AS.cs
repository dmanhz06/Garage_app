using Dapper;
using System;
using System.Collections.Generic;
using System.Text;
using test2.Models;

namespace test2.Services.AuthService
{
    public class AuthService
    {
        public bool Login(string username, string password)
        {
            using (var db = DatabaseHelper.GetConnection())
            {
                // Truy vấn kiểm tra username và password trong bảng Users
                var user = db.QueryFirstOrDefault<User>(
                    "SELECT * FROM Users WHERE Username = @User AND Password = @Pass",
                    new { User = username, Pass = password });

                return user != null;
            }
        }
    }
}
