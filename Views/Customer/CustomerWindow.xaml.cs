using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace test2.Views.Customer
{
    // Lớp model cho dữ liệu khách hàng
    public class CustomerModel
    {
        public int STT { get; set; }
        public string HoVaTen { get; set; }
        public string SoDienThoai { get; set; }
        public string DiaChi { get; set; }
        public string Email { get; set; }
    }

    public partial class CustomerWindow : Window
    {
        // Lưu danh sách gốc để phục vụ việc lọc/tìm kiếm
        private List<CustomerModel> _allCustomers;

        public CustomerWindow()
        {
            InitializeComponent();
            LoadData();

            // Gán sự kiện tìm kiếm nếu bạn đã đặt tên TextBox là txtSearch trong XAML
            if (txtSearch != null)
            {
                txtSearch.TextChanged += TxtSearch_TextChanged;
            }
        }

        private void LoadData()
        {
            // Khởi tạo dữ liệu mẫu giống như trong thiết kế
            _allCustomers = new List<CustomerModel>
            {
                new CustomerModel { STT = 1, HoVaTen = "Nguyễn Văn A", SoDienThoai = "0901234567", DiaChi = "Hà Nội", Email = "a@gmail.com" },
                new CustomerModel { STT = 2, HoVaTen = "Trần Thị B", SoDienThoai = "0912345678", DiaChi = "Đà Nẵng", Email = "b@gmail.com" },
                new CustomerModel { STT = 3, HoVaTen = "Lê Văn C", SoDienThoai = "0987654321", DiaChi = "TP. HCM", Email = "c@gmail.com" },
                new CustomerModel { STT = 4, HoVaTen = "Phạm Văn D", SoDienThoai = "0912345679", DiaChi = "Hải Phòng", Email = "d@gmail.com" },
                new CustomerModel { STT = 5, HoVaTen = "Hoàng Thị E", SoDienThoai = "0934567890", DiaChi = "Cần Thơ", Email = "e@gmail.com" },
                new CustomerModel { STT = 6, HoVaTen = "Vũ Văn F", SoDienThoai = "0911223344", DiaChi = "Hà Nội", Email = "f@gmail.com" },
                new CustomerModel { STT = 7, HoVaTen = "Đặng Thị G", SoDienThoai = "0809988776", DiaChi = "Bình Dương", Email = "g@gmail.com" },
                new CustomerModel { STT = 8, HoVaTen = "Bùi Văn H", SoDienThoai = "0918877665", DiaChi = "Đồng Nai", Email = "h@gmail.com" }
            };

            // Gán dữ liệu vào DataGrid
            CustomerDataGrid.ItemsSource = _allCustomers;
        }

        // Logic tìm kiếm khách hàng
        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = txtSearch.Text.ToLower();

            // Lọc danh sách theo tên hoặc số điện thoại
            var filteredList = _allCustomers.Where(c =>
                c.HoVaTen.ToLower().Contains(searchText) ||
                c.SoDienThoai.Contains(searchText)
            ).ToList();

            // Cập nhật lại giao diện
            CustomerDataGrid.ItemsSource = filteredList;
        }
    }
}