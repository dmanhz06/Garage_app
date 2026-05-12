using System.Windows;

namespace test2.Views.Customer
{
    public partial class AddCustomerWindow : Window
    {
        public CustomerModel NewCustomer { get; private set; }

        public AddCustomerWindow()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            // Kiểm tra validate sơ bộ
            if (string.IsNullOrWhiteSpace(txtName.Text) || string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                MessageBox.Show("Vui lòng nhập ít nhất Tên và Số điện thoại!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            NewCustomer = new CustomerModel
            {
                HoVaTen = txtName.Text,
                SoDienThoai = txtPhone.Text,
                DiaChi = txtAddress.Text,
                Email = txtEmail.Text
            };

            this.DialogResult = true;
            this.Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}