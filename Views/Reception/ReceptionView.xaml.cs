using System.Data;
using System.Windows;
using System.Windows.Controls;
// Nhớ thêm dòng này để gọi DatabaseHelper cho gọn
using test2.Helpers;

namespace test2.Views.Reception
{
    public partial class ReceptionView : Window
    {
        public ReceptionView()
        {
            InitializeComponent();
            LoadData(); // Load ngay khi mở màn hình
        }
        private void btnBackToService_Click(object sender, RoutedEventArgs e)
        {
            test2.Views.Service.ServiceView serviceWin = new test2.Views.Service.ServiceView();
            serviceWin.Show();
            this.Close();
        }
        // Hàm gọi dữ liệu kết hợp 3 bảng
        public void LoadData(string search = "")
        {
            // Kết hợp Phiếu Sửa Chữa + Xe + Khách Hàng để hiển thị
            string sql = @"
                SELECT ro.RepairOrderID as STT, v.LicensePlate, c.FullName as CustomerName, 
                       FORMAT(ro.EntryDate, 'dd/MM/yyyy HH:mm') as EntryDate, 
                       ro.Note as VehicleCondition, ro.Status
                FROM RepairOrders ro
                JOIN Vehicles v ON ro.VehicleID = v.VehicleID
                JOIN Customers c ON v.CustomerID = c.CustomerID
                WHERE v.LicensePlate LIKE @search OR c.FullName LIKE @search
                ORDER BY ro.EntryDate DESC";

            var parameters = new Microsoft.Data.SqlClient.SqlParameter[]
            {
                new Microsoft.Data.SqlClient.SqlParameter("@search", "%" + search + "%")
            };

            // Gọi thẳng tên hàm vì đã có using test2.Helpers; ở trên
            DataTable dt = test2.Helpers.DatabaseHelper.ExecuteQuery(sql, parameters);
            dgReceptions.ItemsSource = dt.DefaultView;
        }

        // Bấm nút Thêm Mới -> Mở Popup
        private void btnAddReception_Click(object sender, RoutedEventArgs e)
        {
            AddReceptionWindow addWin = new AddReceptionWindow();
            addWin.ShowDialog(); // Đợi người dùng tắt popup này

            LoadData(); // Tắt popup xong thì gọi load lại danh sách xe mới
        }

        // Gõ vào ô tìm kiếm -> Tìm theo biển số hoặc tên khách
        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            LoadData(txtSearch.Text);
        }
    }
}