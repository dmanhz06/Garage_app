using System;
using System.Collections.Generic;
using System.Windows;
using test2.Models; // Gọi cái Model ServiceOrder ra để xài

namespace test2.Views.Service
{
    public partial class AddServiceWindow : Window
    {
        // Biến này sẽ chứa dữ liệu dịch vụ mới tạo ra để truyền về cho màn hình chính
        public ServiceOrder NewService { get; private set; }

        public AddServiceWindow()
        {
            InitializeComponent();

            // Gọi hàm đổ dữ liệu khách hàng vào ComboBox ngay khi vừa mở cửa sổ
            LoadCustomerList();
        }

        // HÀM ĐỔ DỮ LIỆU KHÁCH HÀNG VÀO COMBOBOX
        // Đừng quên thêm dòng using này ở tuốt trên cùng của file để gọi được khuôn của Người 2 nhé:
        // using test2.Views.Customer; 

        // HÀM ĐỔ DỮ LIỆU KHÁCH HÀNG VÀO COMBOBOX (PHIÊN BẢN AN TOÀN)
        private void LoadCustomerList()
        {
            try
            {
                // Bước 1: Tạo ngầm cửa sổ Khách Hàng của Người 2 (Nó sẽ chạy LoadData của ổng)
                // Lưu ý: Mình không gọi .Show() nên màn hình này sẽ tàng hình
                var windowNguoi2 = new test2.Views.Customer.CustomerWindow();

                // Bước 2: "Chôm" dữ liệu từ cái bảng CustomerDataGrid của ổng
                var danhSachTuNguoi2 = windowNguoi2.CustomerDataGrid.ItemsSource;

                // Bước 3: Đổ thẳng vào ComboBox của Khánh
                cmbCustomer.ItemsSource = danhSachTuNguoi2;

                // Vẫn hiện thuộc tính HoVaTen như bình thường
                cmbCustomer.DisplayMemberPath = "HoVaTen";
                cmbCustomer.SelectedValuePath = "HoVaTen";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không lấy được dữ liệu Người 2: " + ex.Message);
            }
        }
        // HÀM LƯU THÔNG TIN (Chỉ có duy nhất 1 hàm này thôi nhé)
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            // Kiểm tra xem đã nhập đủ thông tin quan trọng chưa
            if (string.IsNullOrWhiteSpace(txtBienSo.Text) || string.IsNullOrWhiteSpace(txtTenDichVu.Text))
            {
                MessageBox.Show("Vui lòng nhập ít nhất Biển số xe và Tên dịch vụ!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Ép kiểu Giá tiền sang dạng số
            decimal price = 0;
            decimal.TryParse(txtGiaTien.Text, out price);

            // Gom dữ liệu vào khuôn, bao gồm cả Tên khách từ ComboBox và Tình trạng xe
            NewService = new ServiceOrder
            {
                LicensePlate = txtBienSo.Text,
                CustomerName = cmbCustomer.Text,           // Lấy từ danh sách thả xuống
                ServiceName = txtTenDichVu.Text,
                Price = price,
                Time = txtThoiGian.Text,
                VehicleCondition = txtVehicleCondition.Text, // Ghi chú xước xát lúc nhận xe
                Status = cmbTinhTrang.Text
            };

            // Báo cho màn hình chính biết là đã Lưu thành công và tự đóng cửa sổ
            this.DialogResult = true;
        }

        // HÀM HỦY BỎ
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            // Đóng cửa sổ mà không lưu gì cả
            this.DialogResult = false;
        }
    }
}