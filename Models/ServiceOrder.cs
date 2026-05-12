using System;
using System.Collections.Generic;
using System.Text;

namespace test2.Models
{
    public class ServiceOrder
    {
        public int STT { get; set; }
        public string LicensePlate { get; set; } // Biển số
        public string CustomerName { get; set; } // Tên khách
        public string ServiceName { get; set; }  // Dịch vụ
        public decimal Price { get; set; }       // Giá
        public string Status { get; set; }       // Tình trạng
        public string Time { get; set; }         // Thời gian (Đã thêm!)
        public string VehicleCondition { get; set; } // THÊM MỚI: Tình trạng xe lúc nhận (ví dụ: Trầy cửa, bể đèn...)
    }
}
