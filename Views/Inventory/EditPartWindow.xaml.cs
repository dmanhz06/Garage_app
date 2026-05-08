using System;
using System.Windows;

namespace test2.Views.Inventory
{
    public partial class EditPartWindow : Window
    {
        public InventoryModel CurrentPart { get; set; }

        public EditPartWindow(InventoryModel part)
        {
            InitializeComponent();
            CurrentPart = part;

            // Đổ dữ liệu cũ vào form. Xử lý xóa dấu phẩy ở Giá Bán để dễ nhập liệu lại
            txtEditPartName.Text = part.TenPhuTung;
            txtEditUnit.Text = part.ĐVT;
            txtEditPrice.Text = part.GiaBan?.Replace(",", "").Replace(".", "");
            txtEditQuantity.Text = part.TonKho.ToString();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtEditPartName.Text))
            {
                MessageBox.Show("Tên phụ tùng không được để trống!", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(txtEditQuantity.Text, out int quantity))
            {
                MessageBox.Show("Số lượng tồn kho không hợp lệ!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Chỉ cập nhật những thuộc tính hiển thị trên Form
            CurrentPart.TenPhuTung = txtEditPartName.Text;
            CurrentPart.ĐVT = txtEditUnit.Text;
            CurrentPart.GiaBan = txtEditPrice.Text; // Sẽ được parse lại ở hàm BtnEdit_Click bên kia
            CurrentPart.TonKho = quantity;

            this.DialogResult = true;
            this.Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}