using System;
using System.Collections.Generic;
using System.Text;

using System.Data.SqlClient; 

public static class DatabaseHelper
{
   
    private static string _connectionString = "Server=ASUS-ZEPHERUS-G\\SQLEXPRESS;Database=GarageManagement;Trusted_Connection=True;TrustServerCertificate=True;";

    public static SqlConnection GetConnection()
    {
        SqlConnection conn = new SqlConnection(_connectionString);
  
        return conn;
    }
}