using System.Data;
using System.Windows;
using System.Windows.Controls;
using test2.Repositories;
using test2.Views.Customer;

namespace test2.Views.Service
{
    public partial class ServiceView : Window
    {
        private ServiceRepository _repo = new ServiceRepository();

        public ServiceView()
        {
            InitializeComponent();
            LoadData();
        }

        public void LoadData(string search = "")
        {
            DataTable dt = _repo.GetAllServices(search);
            dgServices.ItemsSource = dt.DefaultView;
        }

        private void btnAddService_Click(object sender, RoutedEventArgs e)
        {
            AddServiceWindow addWin = new AddServiceWindow(this);
            addWin.ShowDialog();
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            DataRowView row = (DataRowView)dgServices.SelectedItem;
            if (row != null)
            {
                int id = (int)row["ServiceID"];
                string name = row["ServiceName"].ToString();
                decimal price = (decimal)row["Price"];

                EditServiceWindow editWin = new EditServiceWindow(this, id, name, price);
                editWin.ShowDialog();
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            DataRowView row = (DataRowView)dgServices.SelectedItem;
            if (row != null)
            {
                int id = (int)row["ServiceID"];
                DeleteServiceWindow delWin = new DeleteServiceWindow(this, id);
                delWin.ShowDialog();
            }
        }
        private void btnGoToReception_Click(object sender, RoutedEventArgs e)
        {
            // Khởi tạo màn hình Tiếp nhận
            test2.Views.Reception.ReceptionView receptionWin = new test2.Views.Reception.ReceptionView();

            // Hiện màn hình mới
            receptionWin.Show();

            // Đóng màn hình hiện tại
            this.Close();
        }
        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            LoadData(txtSearch.Text);
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e) { this.Close(); }
        private void MenuSideBar_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Index 1: Quản lý khách hàng
            if (MenuSideBar.SelectedIndex == 1)
            {
                var receptionWin = new test2.Views.Customer.CustomerWindow(); 
                receptionWin.Show();
                this.Close();
            }
            // Index 2: Xe (Tiếp nhận xe)
            else if (MenuSideBar.SelectedIndex == 2)
            {
                var receptionWin = new test2.Views.Car.CarWindow();
                receptionWin.Show();
                this.Close();
            }
            // Index 3 là Dịch vụ (là chính nó rồi nên không cần code)
        }
    }
}