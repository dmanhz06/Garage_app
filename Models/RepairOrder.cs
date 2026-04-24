using System;
using System.Collections.Generic;
using System.Text;

namespace test2.Models
{
    public class RepairOrder
    {
        public int RepairOrderID { get; set; }
        public int VehicleID { get; set; }
        public DateTime EntryDate { get; set; }
        public string Status { get; set; } // Ví dụ: "Đang chờ", "Đang sửa", "Hoàn thành"
        public string Note { get; set; }
    }
}
