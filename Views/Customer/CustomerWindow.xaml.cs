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

        // --- LOGIC THÊM KHÁCH HÀNG ---
        private void btnAddCustomer_Click(object sender, RoutedEventArgs e)
        {
            var addWin = new AddCustomerWindow();
            addWin.Owner = this;

            if (addWin.ShowDialog() == true)
            {
                CustomerModel newMember = addWin.NewCustomer;
                _allCustomers.Add(newMember);

                // Cập nhật lại toàn bộ STT để đảm bảo tính liên tục
                UpdateSTT();
                RefreshGrid();
            }
        }

        // --- LOGIC TÌM KIẾM ---
        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtSearch == null || _allCustomers == null) return;
            string searchText = txtSearch.Text.ToLower().Trim();

            if (string.IsNullOrEmpty(searchText))
            {
                CustomerDataGrid.ItemsSource = _allCustomers;
            }
            else
            {
                var filteredList = _allCustomers.Where(c =>
                    (c.HoVaTen != null && c.HoVaTen.ToLower().Contains(searchText)) ||
                    (c.SoDienThoai != null && c.SoDienThoai.Contains(searchText))
                ).ToList();
                CustomerDataGrid.ItemsSource = filteredList;
            }
        }

        // --- LOGIC CHỈNH SỬA ---
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

        // --- LOGIC XÓA (ĐÃ CẬP NHẬT TỰ ĐỘNG ĐÁNH LẠI STT) ---
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is CustomerModel customer)
            {
                var deleteWin = new DeleteConfirmWindow(customer.HoVaTen);
                deleteWin.Owner = this;

                if (deleteWin.ShowDialog() == true)
                {
                    _allCustomers.Remove(customer);

                    // Gọi hàm đánh lại số thứ tự
                    UpdateSTT();

                    RefreshGrid();
                }
            }
        }

        // --- HÀM CẬP NHẬT SỐ THỨ TỰ (STT) ---
        private void UpdateSTT()
        {
            for (int i = 0; i < _allCustomers.Count; i++)
            {
                _allCustomers[i].STT = i + 1;
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

        private void RefreshGrid()
        {
            // Reset ItemsSource để DataGrid nhận diện sự thay đổi dữ liệu bên trong list
            CustomerDataGrid.ItemsSource = null;
            CustomerDataGrid.ItemsSource = _allCustomers;
        }
        private void MenuSideBar_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listBox = sender as ListBox;
            if (listBox == null || listBox.SelectedItem == null) return;

            var selectedItem = listBox.SelectedItem as ListBoxItem;
            var content = (selectedItem.Content as StackPanel)?.Children.OfType<TextBlock>().LastOrDefault()?.Text;

            if (content == "Xe")
            {
                // Mở cửa sổ Xe và đóng cửa sổ hiện tại
                Car.CarWindow carWin = new Car.CarWindow();
                carWin.Show();
                this.Close();
            }
        }
    }
}