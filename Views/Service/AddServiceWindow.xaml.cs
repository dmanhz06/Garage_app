using System;
using System.Windows;
using test2.Repositories;

namespace test2.Views.Service
{
    public partial class AddServiceWindow : Window
    {
        private ServiceView _parent;
        private ServiceRepository _repo = new ServiceRepository(); // Gọi kho chứa ra

        public AddServiceWindow(ServiceView parent)
        {
            InitializeComponent();
            _parent = parent;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTenDichVu.Text) || string.IsNullOrWhiteSpace(txtGiaTien.Text))
            {
                MessageBox.Show("Vui lòng nhập đủ Tên và Giá!", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                // Gọi hàm AddService từ Repository thay vì viết câu lệnh SQL ở đây
                _repo.AddService(txtTenDichVu.Text.Trim(), Convert.ToDecimal(txtGiaTien.Text));

                MessageBox.Show("Thêm dịch vụ thành công!", "Thông báo");
                _parent.LoadData(); // Load lại bảng bên ngoài
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}