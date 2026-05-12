using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Data;
using test2.Helpers;
using Microsoft.Data.SqlClient;

namespace test2.Views.Appointments
{
    public class AppointmentModel
    {
        public string CustomerName { get; set; } = "";
        public string LicensePlate { get; set; } = "";
        public string CarBrand { get; set; } = "";
        public DateTime AppointmentTime { get; set; }
        public string Status { get; set; } = "";

        public string Note { get; set; }

        

        public string AppointmentTimeText
        {
            get { return AppointmentTime.ToString("dd/MM/yyyy HH:mm"); }
        }
    }

    public partial class AppointmentWindow : Window
    {
        private List<AppointmentModel> _allAppointments = new List<AppointmentModel>();

        private DataView? _appointmentView;

        public AppointmentWindow()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            string sql = @"
        SELECT 
            ro.RepairOrderID AS RepairOrderID,
            c.FullName AS CustomerName,
            v.LicensePlate,
            v.Brand AS CarBrand,
            ro.EntryDate AS AppointmentTime,
            ro.Note,
            ro.Status
        FROM RepairOrders ro
        JOIN Vehicles v ON ro.VehicleID = v.VehicleID
        JOIN Customers c ON v.CustomerID = c.CustomerID
    ";

            DataTable dt = test2.Helpers.DatabaseHelper.ExecuteQuery(sql);
            _appointmentView = dt.DefaultView;
            AppointmentDataGrid.ItemsSource = _appointmentView;
        }

        /*private void LoadData()
        {
            _allAppointments = new List<AppointmentModel>
            {
                new AppointmentModel
                {
                    CustomerName = "Nguyễn Văn A",
                    LicensePlate = "30A-123.45",
                    CarBrand = "Toyota Vios",
                    AppointmentTime = new DateTime(2024, 5, 20, 8, 0, 0),
                    Status = "Đang chờ"
                },
                new AppointmentModel
                {
                    CustomerName = "Trần Thị B",
                    LicensePlate = "29C-567.89",
                    CarBrand = "Honda City",
                    AppointmentTime = new DateTime(2024, 5, 20, 9, 30, 0),
                    Status = "Đang sửa"
                },
                new AppointmentModel
                {
                    CustomerName = "Lê Văn C",
                    LicensePlate = "51F-678.90",
                    CarBrand = "Hyundai Accent",
                    AppointmentTime = new DateTime(2024, 5, 20, 11, 0, 0),
                    Status = "Hoàn thành"
                }
            };

            AppointmentDataGrid.ItemsSource = _allAppointments;
        }*/

        private void btnAddAppointment_Click(object sender, RoutedEventArgs e)
        {
            AddAppointmentWindow addWindow = new AddAppointmentWindow();
            addWindow.Owner = this;

            if (addWindow.ShowDialog() == true)
            {
                LoadData();
            }
        }

        private void AppointmentDataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.Column.Header.ToString() != "Trạng thái")
                return;

            if (e.Row.Item is DataRowView row)
            {
                int repairOrderID = Convert.ToInt32(row["RepairOrderID"]);

                if (e.EditingElement is ComboBox comboBox)
                {
                    string newStatus = comboBox.Text;

                    string sql = @"
                UPDATE RepairOrders
                SET Status = @Status
                WHERE RepairOrderID = @RepairOrderID
            ";

                    var parameters = new Microsoft.Data.SqlClient.SqlParameter[]
                    {
                new Microsoft.Data.SqlClient.SqlParameter("@Status", newStatus),
                new Microsoft.Data.SqlClient.SqlParameter("@RepairOrderID", repairOrderID)
                    };

                    test2.Helpers.DatabaseHelper.ExecuteNonQuery(sql, parameters);
                }
            }
        }

        private void StatusComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox comboBox &&
                comboBox.DataContext is DataRowView row &&
                comboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                int repairOrderID = Convert.ToInt32(row["RepairOrderID"]);
                string newStatus = selectedItem.Content.ToString();

                string sql = @"
            UPDATE RepairOrders
            SET Status = @Status
            WHERE RepairOrderID = @RepairOrderID
        ";

                var parameters = new Microsoft.Data.SqlClient.SqlParameter[]
                {
            new Microsoft.Data.SqlClient.SqlParameter("@Status", newStatus),
            new Microsoft.Data.SqlClient.SqlParameter("@RepairOrderID", repairOrderID)
                };

                test2.Helpers.DatabaseHelper.ExecuteNonQuery(sql, parameters);
            }
        }

        private void txtSearchTime_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_appointmentView == null) return;

            string keyword = txtSearchTime.Text.Trim();

            if (string.IsNullOrWhiteSpace(keyword))
            {
                _appointmentView.RowFilter = "";
                return;
            }

            keyword = keyword.Replace("'", "''");

            _appointmentView.RowFilter =
                $"CONVERT(AppointmentTime, 'System.String') LIKE '%{keyword}%'";
        }

        private void btnClearSearch_Click(object sender, RoutedEventArgs e)
        {
            txtSearchTime.Text = "";

            if (_appointmentView != null)
                _appointmentView.RowFilter = "";
        }

        private void MenuSideBar_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Tạm thời để trống, sau này muốn chuyển trang thì code ở đây.
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Đăng xuất");
        }

        private void RefreshGrid()
        {
            AppointmentDataGrid.ItemsSource = null;
            AppointmentDataGrid.ItemsSource = _allAppointments;
        }

        private void btnDeleteAppointment_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is DataRowView row)
            {
                int repairOrderID = Convert.ToInt32(row["RepairOrderID"]);

                MessageBoxResult result = MessageBox.Show(
                    "Bạn có chắc muốn xóa lịch hẹn này không?",
                    "Xác nhận xóa",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question
                );

                if (result == MessageBoxResult.Yes)
                {
                    string sql = "DELETE FROM RepairOrders WHERE RepairOrderID = @RepairOrderID";

                    SqlParameter[] parameters =
                    {
                new SqlParameter("@RepairOrderID", repairOrderID)
            };

                    test2.Helpers.DatabaseHelper.ExecuteNonQuery(sql, parameters);

                    MessageBox.Show("Xóa lịch hẹn thành công!");

                    LoadData();
                }
            }
        }
    }
}