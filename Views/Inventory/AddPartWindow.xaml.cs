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

        public void Save_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtAddPartName.Text))
            {
                MessageBox.Show("Vui lòng nhập tên phụ tùng!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try 
            {
                NewPart = new Part
                {
                    PartName = txtAddPartName.Text,
                    Unit = txtAddUnit.Text,
                    ImportPrice = decimal.Parse(txtAddImportPrice.Text),
                    Price = decimal.Parse(txtAddPrice.Text),
                    StockQuantity = int.Parse(txtAddQuantity.Text),
                    MinimumStock = 5 
                };

                this.DialogResult = true; 
                this.Close();
            }
            catch (FormatException)
            {
                MessageBox.Show("Giá tiền và Số lượng phải là số!", "Lỗi nhập liệu", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi: " + ex.Message);
            }
        }

        public void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}