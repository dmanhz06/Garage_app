using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using test2.Views.Model_Box;

namespace test2.Views.Customer
{
    public class CustomerModel
    {
        public int STT { get; set; }
        public string HoVaTen { get; set; } = string.Empty;
        public string SoDienThoai { get; set; } = string.Empty;
        public string DiaChi { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }

    public partial class CustomerWindow : Window
    {
        private List<CustomerModel> _allCustomers = new List<CustomerModel>();

        public CustomerWindow()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            // Dữ liệu 8 khách hàng giữ nguyên
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
            CustomerDataGrid.ItemsSource = _allCustomers;
        }

        // --- LOGIC TÌM KIẾM KHÁCH HÀNG ---
        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Kiểm tra xem TextBox có tồn tại và danh sách gốc có dữ liệu không
            if (txtSearch == null || _allCustomers == null) return;

            string searchText = txtSearch.Text.ToLower().Trim();

            // Nếu ô tìm kiếm trống, hiển thị lại toàn bộ danh sách
            if (string.IsNullOrEmpty(searchText))
            {
                CustomerDataGrid.ItemsSource = _allCustomers;
            }
            else
            {
                // Lọc danh sách: Tìm theo Tên HOẶC Số điện thoại
                var filteredList = _allCustomers.Where(c =>
                    (c.HoVaTen != null && c.HoVaTen.ToLower().Contains(searchText)) ||
                    (c.SoDienThoai != null && c.SoDienThoai.Contains(searchText))
                ).ToList();

                // Cập nhật lại ItemsSource để DataGrid làm mới giao diện
                CustomerDataGrid.ItemsSource = filteredList;
            }
        }

        // --- LOGIC CHỈNH SỬA VỚI WINDOW TÙY CHỈNH ---
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is CustomerModel customer)
            {
                var editWin = new EditCustomerWindow(customer);
                editWin.Owner = this;
                if (editWin.ShowDialog() == true)
                {
                    RefreshGrid();
                }
            }
        }

        // --- LOGIC XÓA VỚI WINDOW TÙY CHỈNH ---
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is CustomerModel customer)
            {
                // Gọi DeleteConfirmWindow mới tạo thay vì MessageBox mặc định
                var deleteWin = new DeleteConfirmWindow(customer.HoVaTen);
                deleteWin.Owner = this;

                if (deleteWin.ShowDialog() == true)
                {
                    _allCustomers.Remove(customer);
                    RefreshGrid();
                }
            }
        }

        // --- LOGIC ĐĂNG XUẤT ---
        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new LogoutConfirm();
            dialog.Owner = this;
            if (dialog.ShowDialog() == true)
            {
                new LoginWindow().Show();
                this.Close();
            }
        }

        private void btnAccountSettings_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Chức năng đang phát triển!");
        }

        private void RefreshGrid()
        {
            CustomerDataGrid.ItemsSource = null;
            CustomerDataGrid.ItemsSource = _allCustomers;
        }
    }
}