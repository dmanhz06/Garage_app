using System.Windows;
using test2.Views.Customer;

namespace test2.Views.Model_Box
{
    public partial class EditCustomerWindow : Window
    {
        public CustomerModel CurrentCustomer { get; set; }

        public EditCustomerWindow(CustomerModel customer)
        {
            InitializeComponent();
            CurrentCustomer = customer;

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