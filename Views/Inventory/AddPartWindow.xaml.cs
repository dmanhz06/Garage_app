using System;
using System.Windows;
using test2.Models;

namespace test2.Views.Inventory
{
    public partial class AddPartWindow : Window
    {
        public Part NewPart { get; set; }

        public AddPartWindow()
        {
            InitializeComponent();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtAddPartName.Text))
            {
                MessageBox.Show("Vui lòng nhập tên phụ tùng!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Kiểm tra tính hợp lệ của tất cả các trường dữ liệu số
            if (!decimal.TryParse(txtAddImportPrice.Text, out decimal importPrice) ||
                !decimal.TryParse(txtAddPrice.Text, out decimal price) ||
                !int.TryParse(txtAddQuantity.Text, out int quantity))
            {
                MessageBox.Show("Giá nhập, Giá bán và Số lượng tồn phải là kiểu số hợp lệ!", "Lỗi nhập liệu", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            NewPart = new Part
            {
                PartName = txtAddPartName.Text,
                Unit = txtAddUnit.Text,
                ImportPrice = importPrice,
                Price = price,
                StockQuantity = quantity,
                MinimumStock = 5 // Giá trị mặc định theo yêu cầu của DB
            };

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