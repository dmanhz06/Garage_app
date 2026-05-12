using System;
using System.Windows;
using test2.Repositories;

namespace test2.Views.Service
{
    public partial class EditServiceWindow : Window
    {
        private ServiceView _parent;
        private int _serviceId;
        private ServiceRepository _repo = new ServiceRepository();

        public EditServiceWindow(ServiceView parent, int id, string name, decimal price)
        {
            InitializeComponent();
            _parent = parent;
            _serviceId = id;

            // Đổ dữ liệu cũ lên màn hình
            txtEditTenDichVu.Text = name;
            txtEditGiaTien.Text = price.ToString("G29");
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _repo.UpdateService(_serviceId, txtEditTenDichVu.Text.Trim(), Convert.ToDecimal(txtEditGiaTien.Text));

                MessageBox.Show("Cập nhật thành công!", "Thông báo");
                _parent.LoadData();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}