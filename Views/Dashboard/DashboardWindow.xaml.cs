using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using test2.Models;

namespace test2.Views.Dashboard
{
    public class InvoiceModel
    {
        public int InvoiceID { get; set; }
        public int RepairOrderID { get; set; }
        public DateTime InvoiceDate { get; set; }
        public decimal LaborCost { get; set; }
        public decimal PartsTotal { get; set; }
        public decimal TotalAmount => LaborCost + PartsTotal;
        public string Status { get; set; } = string.Empty;
    }

    public class RepairOrderDashboardModel
    {
        public string RepairCode { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string LicensePlate { get; set; } = string.Empty;
        public string ServiceName { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public string CreatedDateText => CreatedDate.ToString("dd/MM/yyyy");
    }

    public class RevenueChartModel
    {
        public string DayName { get; set; } = string.Empty;
        public double BarHeight { get; set; }
    }

    public class ServiceRateModel
    {
        public string ServiceName { get; set; } = string.Empty;
        public int Count { get; set; }
        public string Rate { get; set; } = string.Empty;
    }

    public partial class DashboardWindow : Window
    {
        private List<InvoiceModel> _invoices = new List<InvoiceModel>();
        private List<RepairOrderDashboardModel> _repairOrders = new List<RepairOrderDashboardModel>();

        public DashboardWindow()
        {
            InitializeComponent();
            LoadDataFromSql();
            LoadDashboard();
        }

        private static string _connectionString = "Server=ADMIN-PC\\SQLEXPRESS;Database=GarageManagement;Trusted_Connection=True;TrustServerCertificate=True;";

        private void LoadDataFromSql()
    {
        _invoices.Clear();
        _repairOrders.Clear();

        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            conn.Open();

            string invoiceQuery = @"
            SELECT InvoiceID, RepairOrderID, InvoiceDate, LaborCost, PartsTotal, Status
            FROM Invoices";

            using (SqlCommand cmd = new SqlCommand(invoiceQuery, conn))
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    _invoices.Add(new InvoiceModel
                    {
                        InvoiceID = Convert.ToInt32(reader["InvoiceID"]),
                        RepairOrderID = Convert.ToInt32(reader["RepairOrderID"]),
                        InvoiceDate = Convert.ToDateTime(reader["InvoiceDate"]),
                        LaborCost = Convert.ToDecimal(reader["LaborCost"]),
                        PartsTotal = Convert.ToDecimal(reader["PartsTotal"]),
                        Status = reader["Status"].ToString() ?? ""
                    });
                }
            }

            string repairQuery = @"
            SELECT 
                ro.RepairOrderID,
                c.FullName,
                v.LicensePlate,
                ISNULL(s.ServiceName, N'Chưa có dịch vụ') AS ServiceName,
                ro.Status,
                ro.EntryDate
            FROM RepairOrders ro
            LEFT JOIN Vehicles v ON ro.VehicleID = v.VehicleID
            LEFT JOIN Customers c ON v.CustomerID = c.CustomerID
            OUTER APPLY (
                SELECT TOP 1 sv.ServiceName
                FROM RepairOrderServices ros
                JOIN Services sv ON ros.ServiceID = sv.ServiceID
                WHERE ros.RepairOrderID = ro.RepairOrderID
            ) s
            ORDER BY ro.EntryDate DESC";

            using (SqlCommand cmd = new SqlCommand(repairQuery, conn))
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    int repairOrderID = Convert.ToInt32(reader["RepairOrderID"]);

                    _repairOrders.Add(new RepairOrderDashboardModel
                    {
                        RepairCode = "PS" + repairOrderID.ToString("D4"),
                        CustomerName = reader["FullName"].ToString() ?? "",
                        LicensePlate = reader["LicensePlate"].ToString() ?? "",
                        ServiceName = reader["ServiceName"].ToString() ?? "",
                        Status = reader["Status"].ToString() ?? "",
                        CreatedDate = Convert.ToDateTime(reader["EntryDate"])
                    });
                }
            }
        }
    }

    /*private void LoadSampleData()
    {
        _invoices = new List<InvoiceModel>
        {
            new InvoiceModel { InvoiceID = 1, RepairOrderID = 1, InvoiceDate = DateTime.Now.AddDays(-6), LaborCost = 8000000, PartsTotal = 6000000, Status = "Đã thanh toán" },
            new InvoiceModel { InvoiceID = 2, RepairOrderID = 2, InvoiceDate = DateTime.Now.AddDays(-5), LaborCost = 12000000, PartsTotal = 16000000, Status = "Đã thanh toán" },
            new InvoiceModel { InvoiceID = 3, RepairOrderID = 3, InvoiceDate = DateTime.Now.AddDays(-4), LaborCost = 10000000, PartsTotal = 15000000, Status = "Đã thanh toán" },
            new InvoiceModel { InvoiceID = 4, RepairOrderID = 4, InvoiceDate = DateTime.Now.AddDays(-3), LaborCost = 20000000, PartsTotal = 15000000, Status = "Đã thanh toán" },
            new InvoiceModel { InvoiceID = 5, RepairOrderID = 5, InvoiceDate = DateTime.Now.AddDays(-2), LaborCost = 15000000, PartsTotal = 14000000, Status = "Chưa thanh toán" },
            new InvoiceModel { InvoiceID = 6, RepairOrderID = 6, InvoiceDate = DateTime.Now.AddDays(-1), LaborCost = 25000000, PartsTotal = 20000000, Status = "Đã thanh toán" }
        };

        _repairOrders = new List<RepairOrderDashboardModel>
        {
            new RepairOrderDashboardModel { RepairCode = "PS0005", CustomerName = "Nguyễn Văn A", LicensePlate = "30A-123.45", ServiceName = "Bảo dưỡng", Status = "Đang sửa", CreatedDate = DateTime.Now.AddDays(-1) },
            new RepairOrderDashboardModel { RepairCode = "PS0004", CustomerName = "Trần Thị B", LicensePlate = "29C-567.89", ServiceName = "Thay lốp", Status = "Chờ phụ tùng", CreatedDate = DateTime.Now.AddDays(-2) },
            new RepairOrderDashboardModel { RepairCode = "PS0003", CustomerName = "Lê Văn C", LicensePlate = "51F-678.90", ServiceName = "Sửa máy", Status = "Hoàn thành", CreatedDate = DateTime.Now.AddDays(-3) },
            new RepairOrderDashboardModel { RepairCode = "PS0002", CustomerName = "Phạm Văn D", LicensePlate = "60A-111.22", ServiceName = "Thay thế phụ tùng", Status = "Hoàn thành", CreatedDate = DateTime.Now.AddDays(-4) },
            new RepairOrderDashboardModel { RepairCode = "PS0001", CustomerName = "Hoàng Thị E", LicensePlate = "43B-333.44", ServiceName = "Sửa chữa chung", Status = "Đang sửa", CreatedDate = DateTime.Now.AddDays(-5) }
        };
    }*/

    private void LoadDashboard()
        {
            List<InvoiceModel> filteredInvoices = GetFilteredInvoices();

            txtRepairOrders.Text = _repairOrders.Count.ToString();
            txtCustomers.Text = _repairOrders.Select(x => x.CustomerName).Distinct().Count().ToString();

            decimal totalRevenue = filteredInvoices
                .Where(x => x.Status == "Đã thanh toán")
                .Sum(x => x.TotalAmount);

            txtRevenue.Text = Math.Round(totalRevenue / 1000000, 0).ToString();
            txtCarsInGarage.Text = _repairOrders.Count(x => x.Status == "Đang sửa" || x.Status == "Chờ phụ tùng").ToString();

            LoadRevenueChart(filteredInvoices);
            LoadServiceRate();
            RecentRepairDataGrid.ItemsSource = _repairOrders
                .OrderByDescending(x => x.CreatedDate)
                .ToList();
        }

        private List<InvoiceModel> GetFilteredInvoices()
        {
            if (cbTimeFilter.SelectedItem is ComboBoxItem item)
            {
                string selected = item.Content.ToString();

                if (selected == "7 ngày qua")
                    return _invoices.Where(x => x.InvoiceDate >= DateTime.Now.AddDays(-7)).ToList();

                if (selected == "30 ngày qua")
                    return _invoices.Where(x => x.InvoiceDate >= DateTime.Now.AddDays(-30)).ToList();
            }

            return _invoices;
        }

        private void LoadRevenueChart(List<InvoiceModel> invoices)
        {
            var data = invoices
                .GroupBy(x => x.InvoiceDate.Date)
                .OrderBy(x => x.Key)
                .Select(x => new
                {
                    Date = x.Key,
                    Revenue = x.Where(i => i.Status == "Đã thanh toán").Sum(i => i.TotalAmount) / 1000000
                })
                .ToList();

            decimal maxRevenue = data.Count > 0 ? data.Max(x => x.Revenue) : 1;

            RevenueChart.ItemsSource = data.Select(x => new RevenueChartModel
            {
                DayName = GetVietnameseDayName(x.Date),
                BarHeight = maxRevenue == 0 ? 10 : (double)(x.Revenue / maxRevenue) * 180
            }).ToList();
        }

        private string GetVietnameseDayName(DateTime date)
        {
            switch (date.DayOfWeek)
            {
                case DayOfWeek.Monday: return "T2";
                case DayOfWeek.Tuesday: return "T3";
                case DayOfWeek.Wednesday: return "T4";
                case DayOfWeek.Thursday: return "T5";
                case DayOfWeek.Friday: return "T6";
                case DayOfWeek.Saturday: return "T7";
                case DayOfWeek.Sunday: return "CN";
                default: return "";
            }
        }

        private void LoadServiceRate()
        {
            int total = _repairOrders.Count;

            ServiceRateGrid.ItemsSource = _repairOrders
                .GroupBy(x => x.ServiceName)
                .Select(g => new ServiceRateModel
                {
                    ServiceName = g.Key,
                    Count = g.Count(),
                    Rate = total == 0 ? "0%" : $"{Math.Round((double)g.Count() / total * 100, 1)}%"
                })
                .ToList();
        }

        private void cbTimeFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (txtRevenue != null)
                LoadDashboard();
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Đăng xuất thành công!");
            this.Close();
        }

        private void MenuSideBar_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MenuSideBar.SelectedItem is not ListBoxItem selectedItem) return;

            string? content = (selectedItem.Content as StackPanel)?
                .Children
                .OfType<TextBlock>()
                .LastOrDefault()?
                .Text;

            if (content == "Quản lý khách hàng")
            {
                var customerWin = new test2.Views.Customer.CustomerWindow();
                customerWin.Show();
                this.Close();
            }
            else if (content == "Xe")
            {
                var carWin = new test2.Views.Car.CarWindow();
                carWin.Show();
                this.Close();
            }
        }
    }
}