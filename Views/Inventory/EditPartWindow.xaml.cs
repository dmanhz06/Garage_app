using System;
using System.Windows;

namespace test2.Views.Inventory
{
    public partial class EditPartWindow : Window
    {
        // Property để hứng dữ liệu truyền từ cửa sổ chính sang
        public InventoryModel CurrentPart { get; set; }

        public EditPartWindow(InventoryModel part)
        {
            InitializeComponent();
            CurrentPart = part;

            // Đổ dữ liệu cũ vào các TextBox để Thuận sửa
            // Phải đảm bảo các x:Name trong XAML khớp hoàn toàn với ở đây
            txtEditPartName.Text = part.TenPhuTung;
            txtEditUnit.Text = part.ĐVT;
            txtEditPrice.Text = part.GiaBan;
            txtEditQuantity.Text = part.TonKho.ToString();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try 
            {
                // Cập nhật lại đối tượng với dữ liệu mới từ TextBox
                CurrentPart.TenPhuTung = txtEditPartName.Text;
                CurrentPart.ĐVT = txtEditUnit.Text;
                CurrentPart.GiaBan = txtEditPrice.Text;
                
                // Chuyển đổi an toàn từ chuỗi sang số
                if (int.TryParse(txtEditQuantity.Text, out int quantity))
                {
                    CurrentPart.TonKho = quantity;
                }

                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi xảy ra khi lưu dữ liệu: " + ex.Message);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}