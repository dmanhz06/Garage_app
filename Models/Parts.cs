using System;
using System.Collections.Generic;
using System.Text;

namespace test2.Models
{
    public class Part
    {
        public int PartID { get; set; }
        public string PartName { get; set; }
        public decimal Price { get; set; }        // Giá bán
        public int StockQuantity { get; set; }    // Số lượng tồn
        public string Unit { get; set; }          // Đơn vị tính
        public decimal ImportPrice { get; set; }  // Giá nhập
        public int MinimumStock { get; set; }     // Tồn tối thiểu
    }
}
