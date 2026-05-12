using System.Windows;
using test2.Views.Customer;

namespace test2.Views.Model_Box
{
    public partial class EditCustomerWindow : Window
    {
        public CustomerModel CurrentCustomer { get; set; }

        // Sửa lỗi CS1729: Thêm constructor nhận 1 đối số
        public EditCustomerWindow(CustomerModel customer)
        {
            InitializeComponent();
            CurrentCustomer = customer;

            // Đổ dữ liệu vào TextBox (Sửa lỗi CS0103 bằng cách khớp x:Name trong XAML)
            txtEditName.Text = customer.HoVaTen;
            txtEditPhone.Text = customer.SoDienThoai;
            txtEditAddress.Text = customer.DiaChi;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            CurrentCustomer.HoVaTen = txtEditName.Text;
            CurrentCustomer.SoDienThoai = txtEditPhone.Text;
            CurrentCustomer.DiaChi = txtEditAddress.Text;

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