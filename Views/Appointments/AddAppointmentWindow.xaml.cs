using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Data.SqlClient;
using test2.Helpers;

namespace test2.Views.Appointments
{
    public partial class AddAppointmentWindow : Window
    {
        public AppointmentModel NewAppointment { get; private set; }

        public AddAppointmentWindow()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtHoTen.Text) ||
                string.IsNullOrWhiteSpace(txtBienSo.Text) ||
                string.IsNullOrWhiteSpace(txtHangXe.Text) ||
                string.IsNullOrWhiteSpace(txtThoiGian.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin lịch hẹn!");
                return;
            }

            if (!DateTime.TryParse(txtThoiGian.Text, out DateTime thoiGian))
            {
                MessageBox.Show("Thời gian không hợp lệ! Ví dụ đúng: 20/05/2024 08:30");
                return;
            }

            string sql = @"
                INSERT INTO Customers (FullName, PhoneNumber, Address, Email)
                VALUES (@FullName, @PhoneNumber, NULL, NULL);

                DECLARE @CustomerID INT = SCOPE_IDENTITY();

                INSERT INTO Vehicles (LicensePlate, Brand, Model, CustomerID)
                VALUES (@LicensePlate, @Brand, NULL, @CustomerID);

                DECLARE @VehicleID INT = SCOPE_IDENTITY();

                INSERT INTO RepairOrders (VehicleID, EntryDate, Status, Note)
                VALUES (@VehicleID, @EntryDate, @Status, NULL);
            ";

            SqlParameter[] parameters =
            {
                new SqlParameter("@FullName", txtHoTen.Text.Trim()),
                new SqlParameter("@PhoneNumber", "0000000000"),
                new SqlParameter("@LicensePlate", txtBienSo.Text.Trim()),
                new SqlParameter("@Brand", txtHangXe.Text.Trim()),
                new SqlParameter("@EntryDate", thoiGian),
                new SqlParameter("@Status", "Đang chờ")
            };

            test2.Helpers.DatabaseHelper.ExecuteNonQuery(sql, parameters);

            NewAppointment = new AppointmentModel
            {
                CustomerName = txtHoTen.Text.Trim(),
                LicensePlate = txtBienSo.Text.Trim(),
                CarBrand = txtHangXe.Text.Trim(),
                AppointmentTime = thoiGian,
                Status = "Đang chờ"
            };

            MessageBox.Show("Thêm lịch hẹn thành công!");

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