using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using test2.Helpers;
namespace test2.Views.Reception
{
    public partial class AddReceptionWindow : Window
    {
        public AddReceptionWindow()
        {
            InitializeComponent();
        }

        // Khi vừa mở cửa sổ lên, tự động lấy danh sách Khách hàng đổ vào ComboBox
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string sql = "SELECT CustomerID, FullName FROM Customers";
            DataTable dtCustomers = test2.Helpers.DatabaseHelper.ExecuteQuery(sql);
            cmbCustomer.ItemsSource = dtCustomers.DefaultView;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            // 1. Kiểm tra nhập liệu
            if (cmbCustomer.SelectedValue == null || string.IsNullOrWhiteSpace(txtBienSo.Text))
            {
                MessageBox.Show("Vui lòng chọn khách hàng và nhập biển số xe!", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                int customerId = Convert.ToInt32(cmbCustomer.SelectedValue);
                string licensePlate = txtBienSo.Text.Trim();
                string note = txtVehicleCondition.Text;
                string status = ((ComboBoxItem)cmbTinhTrang.SelectedItem).Content.ToString();

                // 2. Kiểm tra xem Xe này đã có trong hệ thống chưa, nếu chưa thì Thêm vào bảng Vehicles
                string checkCarSql = "SELECT VehicleID FROM Vehicles WHERE LicensePlate = @plate";
                var checkParam = new Microsoft.Data.SqlClient.SqlParameter[] { new Microsoft.Data.SqlClient.SqlParameter("@plate", licensePlate) };
                object carIdResult = test2.Helpers.DatabaseHelper.ExecuteScalar(checkCarSql, checkParam);

                int vehicleId;

                if (carIdResult == null) // Xe mới toanh
                {
                    string insertCarSql = "INSERT INTO Vehicles (LicensePlate, CustomerID) OUTPUT INSERTED.VehicleID VALUES (@plate, @cusId)";
                    var insertCarParams = new Microsoft.Data.SqlClient.SqlParameter[]
                    {
                        new Microsoft.Data.SqlClient.SqlParameter("@plate", licensePlate),
                        new Microsoft.Data.SqlClient.SqlParameter("@cusId", customerId)
                    };
                    vehicleId = Convert.ToInt32(test2.Helpers.DatabaseHelper.ExecuteScalar(insertCarSql, insertCarParams));
                }
                else // Xe đã từng sửa ở đây rồi
                {
                    vehicleId = Convert.ToInt32(carIdResult);
                }

                // 3. Tạo phiếu sửa chữa (Lưu vào bảng RepairOrders)
                string insertOrderSql = "INSERT INTO RepairOrders (VehicleID, Status, Note, EntryDate) VALUES (@vId, @status, @note, GETDATE())";
                var orderParams = new Microsoft.Data.SqlClient.SqlParameter[]
                {
                    new Microsoft.Data.SqlClient.SqlParameter("@vId", vehicleId),
                    new Microsoft.Data.SqlClient.SqlParameter("@status", status),
                    new Microsoft.Data.SqlClient.SqlParameter("@note", note)
                };

                test2.Helpers.DatabaseHelper.ExecuteNonQuery(insertOrderSql, orderParams);

                MessageBox.Show("Đã tạo Phiếu tiếp nhận xe thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi CSDL: " + ex.Message);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}