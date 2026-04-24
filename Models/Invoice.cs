using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Microsoft.Data.SqlClient;

namespace test2.Models
{
    public class Invoice
    {
        public int InvoiceID { get; set; }
        public int RepairOrderID { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentMethod { get; set; } // Ví dụ: "Tiền mặt", "Chuyển khoản"
    }

}

