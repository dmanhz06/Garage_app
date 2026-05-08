using System;
using System.Windows;
using System.Windows.Controls;
using System.Xaml;
using test2.Models;
using test2.Repositories.ServiceRepository;

namespace test2.Views.Service
{
    public partial class ServiceView : Window
    {
        private SR _repo = new SR();
        private List<test2.Models.ServiceOrder> _allServices = new List<test2.Models.ServiceOrder>();

        public ServiceView()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            // Cập nhật danh sách dữ liệu thực tế hơn cho Gara
            _allServices = new List<ServiceOrder>
    {
        new ServiceOrder {
            STT = 1,
            LicensePlate = "51H-123.45",
            CustomerName = "Nguyễn Văn Anh",
            ServiceName = "Bảo dưỡng 10.000km",
            Price = 1200000,
            Time = "08/05/2026 08:00",
            Status = "Đã xong"
        },
        new ServiceOrder {
            STT = 2,
            LicensePlate = "29A-678.90",
            CustomerName = "Lê Thị Lan",
            ServiceName = "Sơn phủ Ceramic",
            Price = 5500000,
            Time = "08/05/2026 09:15",
            Status = "Đang làm"
        },
        new ServiceOrder {
            STT = 3,
            LicensePlate = "60B-111.22",
            CustomerName = "Trần Minh Quang",
            ServiceName = "Thay lốp Michelin (4 bánh)",
            Price = 8400000,
            Time = "08/05/2026 10:30",
            Status = "Chờ phụ tùng"
        },
        new ServiceOrder {
            STT = 4,
            LicensePlate = "43C-555.66",
            CustomerName = "Phạm Hồng Thái",
            ServiceName = "Vệ sinh khoang máy",
            Price = 800000,
            Time = "08/05/2026 13:45",
            Status = "Chờ xử lý"
        },
        new ServiceOrder {
            STT = 5,
            LicensePlate = "72A-999.99",
            CustomerName = "Hoàng Gia Bảo",
            ServiceName = "Thay nhớt & Lọc nhớt",
            Price = 1550000,
            Time = "08/05/2026 14:20",
            Status = "Đang làm"
        }
    };

            // Gán dữ liệu vào bảng
            dgServices.ItemsSource = _allServices;
        }

        private void btnAddService_Click(object sender, RoutedEventArgs e)
        {
            test2.Views.Service.AddServiceWindow addWin = new test2.Views.Service.AddServiceWindow();
            addWin.Owner = this;

            if (addWin.ShowDialog() == true)
            {
                // Lấy dữ liệu mới
                test2.Models.ServiceOrder newService = addWin.NewService;

                // Thêm vào danh sách
                _allServices.Add(newService);
                UpdateSTT();
                RefreshGrid();
            }
        }

        // TÍNH NĂNG SỬA (GỌI CỬA SỔ SỬA)
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is ServiceOrder selectedItem)
            {
                // Gọi cửa sổ Sửa và ném dữ liệu cũ qua cho nó
                test2.Views.Service.EditServiceWindow editWin = new test2.Views.Service.EditServiceWindow(selectedItem);
                editWin.Owner = this;

                if (editWin.ShowDialog() == true)
                {
                    // Chạy hàm vẽ lại bảng vì dữ liệu bên trong Object đã được đổi
                    RefreshGrid();
                }
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is ServiceOrder selectedItem)
            {
                // Gọi cửa sổ Xóa tự làm
                test2.Views.Service.DeleteServiceWindow deleteWin = new test2.Views.Service.DeleteServiceWindow(selectedItem.LicensePlate);
                deleteWin.Owner = this;

                if (deleteWin.ShowDialog() == true)
                {
                    _allServices.Remove(selectedItem);
                    UpdateSTT();
                    RefreshGrid();
                }
            }
        }

        // --- CHỨC NĂNG TÌM KIẾM ---
        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Tránh lỗi khi danh sách chưa được tải
            if (txtSearch == null || _allServices == null) return;

            // Lấy chữ người dùng vừa gõ, chuyển hết thành chữ thường và xóa khoảng trắng thừa
            string searchText = txtSearch.Text.ToLower().Trim();

            // Nếu ô tìm kiếm trống, hiển thị lại toàn bộ danh sách ban đầu
            if (string.IsNullOrEmpty(searchText))
            {
                dgServices.ItemsSource = _allServices;
            }
            else
            {
                // Lọc danh sách: Tìm theo Biển số, Tên khách hoặc Tên dịch vụ
                var filteredList = _allServices.Where(s =>
                    (s.LicensePlate != null && s.LicensePlate.ToLower().Contains(searchText)) ||
                    (s.CustomerName != null && s.CustomerName.ToLower().Contains(searchText)) ||
                    (s.ServiceName != null && s.ServiceName.ToLower().Contains(searchText))
                ).ToList();

                // Đổ danh sách đã lọc vào bảng
                dgServices.ItemsSource = filteredList;
            }
        }

        private void MenuSideBar_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Tránh lỗi vặt khi ListBox chưa chọn gì
            if (MenuSideBar.SelectedIndex == -1) return;

            int viTriBams = MenuSideBar.SelectedIndex;

            try
            {
                // Đếm từ trên xuống: 0: Dashboard, 1: Khách hàng, 2: Xe, 3: Dịch vụ
                if (viTriBams == 1) // Bấm vào Quản lý khách hàng
                {
                    test2.Views.Customer.CustomerWindow cusWin = new test2.Views.Customer.CustomerWindow();
                    cusWin.Show();
                    this.Close();
                }
                else if (viTriBams == 2) // Bấm vào Xe
                {
                    test2.Views.Car.CarWindow carWin = new test2.Views.Car.CarWindow();
                    carWin.Show();
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Nổ lỗi rồi: " + ex.Message, "Báo lỗi");
            }

            // Mẹo: Tự động bỏ bôi xanh để lần sau bấm lại nó vẫn ăn
            MenuSideBar.SelectedIndex = -1;
        }
        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        // --- HÀM 1: Cập nhật lại số thứ tự 1, 2, 3... sau khi thêm/xóa ---
        private void UpdateSTT()
        {
            if (_allServices == null) return;
            for (int i = 0; i < _allServices.Count; i++)
            {
                _allServices[i].STT = i + 1;
            }
        }

        // --- HÀM 2: Tải lại bảng để nó hiện dữ liệu mới ---
        private void RefreshGrid()
        {
            dgServices.ItemsSource = null;
            dgServices.ItemsSource = _allServices;
        }

    }
}