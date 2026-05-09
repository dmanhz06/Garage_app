using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Data.SqlClient;
using test2.Helpers;

namespace test2.Views.Appointments
{
    public partial class AddAppointmentWindow : Window
    {
        public AppointmentModel NewAppointment { get; private set; }

        private int selectedVehicleID = -1;

        public AddAppointmentWindow()
        {
            InitializeComponent();
        }

        private void cboHoTen_KeyUp(object sender, KeyEventArgs e)
        {
            string keyword = cboHoTen.Text.Trim();

            selectedVehicleID = -1;
            cboBienSo.ItemsSource = null;
            cboBienSo.Text = "";
            cboBienSo.IsEnabled = false;
            txtHangXe.Text = "";

            if (string.IsNullOrWhiteSpace(keyword))
            {
                cboHoTen.ItemsSource = null;
                return;
            }

            string sql = @"
                SELECT CustomerID, FullName
                FROM Customers
                WHERE FullName LIKE @Keyword";

            SqlParameter[] parameters =
            {
                new SqlParameter("@Keyword", "%" + keyword + "%")
            };

            DataTable dt = test2.Helpers.DatabaseHelper.ExecuteQuery(sql, parameters);

            cboHoTen.ItemsSource = dt.DefaultView;
            cboHoTen.DisplayMemberPath = "FullName";
            cboHoTen.SelectedValuePath = "CustomerID";
            cboHoTen.IsDropDownOpen = dt.Rows.Count > 0;
        }

        private void cboHoTen_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cboHoTen.SelectedItem == null) return;

            LoadBienSoTheoKhachHang();
        }

        private void LoadBienSoTheoKhachHang()
        {
            DataRowView customerRow = cboHoTen.SelectedItem as DataRowView;
            if (customerRow == null) return;

            int customerID = Convert.ToInt32(customerRow["CustomerID"]);

            string sql = @"
                SELECT VehicleID, LicensePlate, Brand
                FROM Vehicles
                WHERE CustomerID = @CustomerID";

            SqlParameter[] parameters =
            {
                new SqlParameter("@CustomerID", customerID)
            };

            DataTable dt = test2.Helpers.DatabaseHelper.ExecuteQuery(sql, parameters);

            cboBienSo.ItemsSource = dt.DefaultView;
            cboBienSo.DisplayMemberPath = "LicensePlate";
            cboBienSo.SelectedValuePath = "VehicleID";

            cboBienSo.IsEnabled = dt.Rows.Count > 0;
            cboBienSo.IsDropDownOpen = dt.Rows.Count > 0;

            txtHangXe.Text = "";
            selectedVehicleID = -1;

            if (dt.Rows.Count == 1)
            {
                cboBienSo.SelectedIndex = 0;
            }
        }

        private void cboBienSo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataRowView vehicleRow = cboBienSo.SelectedItem as DataRowView;
            if (vehicleRow == null) return;

            selectedVehicleID = Convert.ToInt32(vehicleRow["VehicleID"]);
            txtHangXe.Text = vehicleRow["Brand"].ToString();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (cboHoTen.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn khách hàng có trong danh sách!");
                return;
            }

            if (selectedVehicleID == -1)
            {
                MessageBox.Show("Vui lòng chọn biển số xe có trong danh sách!");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtThoiGian.Text))
            {
                MessageBox.Show("Vui lòng nhập thời gian hẹn!");
                return;
            }

            if (!DateTime.TryParse(txtThoiGian.Text, out DateTime thoiGian))
            {
                MessageBox.Show("Thời gian không hợp lệ! Ví dụ đúng: 20/05/2024 08:30");
                return;
            }

            string sql = @"
                INSERT INTO RepairOrders (VehicleID, EntryDate, Status, Note)
                VALUES (@VehicleID, @EntryDate, @Status, NULL);";

            SqlParameter[] parameters =
            {
                new SqlParameter("@VehicleID", selectedVehicleID),
                new SqlParameter("@EntryDate", thoiGian),
                new SqlParameter("@Status", "Đang chờ")
            };

            test2.Helpers.DatabaseHelper.ExecuteNonQuery(sql, parameters);

            NewAppointment = new AppointmentModel
            {
                CustomerName = cboHoTen.Text.Trim(),
                LicensePlate = cboBienSo.Text.Trim(),
                CarBrand = txtHangXe.Text.Trim(),
                AppointmentTime = thoiGian,
                Status = "Đang chờ"
            };

            MessageBox.Show("Thêm lịch hẹn thành công!");

            DialogResult = true;
            Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}