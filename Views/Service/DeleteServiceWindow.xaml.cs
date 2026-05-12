using System;
using System.Windows;
using test2.Repositories;

namespace test2.Views.Service
{
    public partial class DeleteServiceWindow : Window
    {
        private ServiceView _parent;
        private int _serviceId;
        private ServiceRepository _repo = new ServiceRepository();

        public DeleteServiceWindow(ServiceView parent, int id)
        {
            InitializeComponent();
            _parent = parent;
            _serviceId = id;
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _repo.DeleteService(_serviceId);

                MessageBox.Show("Đã xóa dịch vụ!", "Thông báo");
                _parent.LoadData();
                this.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Không thể xóa dịch vụ này vì đã có dữ liệu liên quan!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}