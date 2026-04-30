using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using test2.Views.Model_Box; // Chứa EditCarWindow và DeleteCarWindow
using test2.Views.Car;       // Chứa AddCarWindow và CarModel

namespace test2.Views.Car
{
    /// <summary>
    /// Model đại diện cho dữ liệu Xe
    /// </summary>
    public class CarModel
    {
        public int STT { get; set; }
        public string BienSo { get; set; } = string.Empty;
        public string HangXe { get; set; } = string.Empty;
        public string DongXe { get; set; } = string.Empty;
        public string NamSX { get; set; } = string.Empty;
        public string TenKhachHang { get; set; } = string.Empty;
    }

    public partial class CarWindow : Window
    {
        // Danh sách gốc chứa toàn bộ dữ liệu xe
        private List<CarModel> _allCars = new List<CarModel>();

        public CarWindow()
        {
            InitializeComponent();
            LoadData();
        }

        /// <summary>
        /// Khởi tạo dữ liệu mẫu ban đầu
        /// </summary>
        private void LoadData()
        {
            _allCars = new List<CarModel>
            {
                new CarModel { BienSo = "30A-123.45", HangXe = "Toyota", DongXe = "Vios", NamSX = "2020", TenKhachHang = "Nguyễn Văn A" },
                new CarModel { BienSo = "29C-567.89", HangXe = "Honda", DongXe = "City", NamSX = "2019", TenKhachHang = "Trần Thị B" },
                new CarModel { BienSo = "51F-678.90", HangXe = "Hyundai", DongXe = "Accent", NamSX = "2021", TenKhachHang = "Lê Văn C" },
                new CarModel { BienSo = "90A-111.22", HangXe = "Ford", DongXe = "Ranger", NamSX = "2018", TenKhachHang = "Phạm Văn D" },
                new CarModel { BienSo = "65A-222.33", HangXe = "Kia", DongXe = "Morning", NamSX = "2022", TenKhachHang = "Hoàng Thị E" },
                new CarModel { BienSo = "43A-333.44", HangXe = "Mazda", DongXe = "3", NamSX = "2020", TenKhachHang = "Vũ Văn F" },
                new CarModel { BienSo = "15K-444.55", HangXe = "VinFast", DongXe = "Fadil", NamSX = "2021", TenKhachHang = "Đặng Thị G" }
            };
            UpdateSTT();
            CarDataGrid.ItemsSource = _allCars;
        }

        /// <summary>
        /// Đánh lại số thứ tự từ 1 cho danh sách
        /// </summary>
        private void UpdateSTT()
        {
            for (int i = 0; i < _allCars.Count; i++)
            {
                _allCars[i].STT = i + 1;
            }
        }

        /// <summary>
        /// Làm mới giao diện DataGrid
        /// </summary>
        private void RefreshGrid()
        {
            CarDataGrid.ItemsSource = null;
            CarDataGrid.ItemsSource = _allCars;
        }

        // --- SỰ KIỆN TÌM KIẾM ---
        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtSearch == null || _allCars == null) return;

            string searchText = txtSearch.Text.ToLower().Trim();

            if (string.IsNullOrEmpty(searchText))
            {
                CarDataGrid.ItemsSource = _allCars;
            }
            else
            {
                // Tìm kiếm theo Biển số, Tên khách hàng hoặc Hãng xe
                var filtered = _allCars.Where(c =>
                    c.BienSo.ToLower().Contains(searchText) ||
                    c.TenKhachHang.ToLower().Contains(searchText) ||
                    c.HangXe.ToLower().Contains(searchText)
                ).ToList();

                CarDataGrid.ItemsSource = filtered;
            }
        }

        // --- SỰ KIỆN THÊM XE ---
        private void btnAddCar_Click(object sender, RoutedEventArgs e)
        {
            AddCarWindow addWin = new AddCarWindow();
            addWin.Owner = this;

            if (addWin.ShowDialog() == true && addWin.NewCar != null)
            {
                _allCars.Add(addWin.NewCar);
                UpdateSTT();
                RefreshGrid();
            }
        }

        // --- SỰ KIỆN SỬA XE ---
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is CarModel car)
            {
                EditCarWindow editWin = new EditCarWindow(car);
                editWin.Owner = this;

                if (editWin.ShowDialog() == true)
                {
                    // Dữ liệu trong 'car' đã được cập nhật tham chiếu trực tiếp trong EditCarWindow
                    RefreshGrid();
                }
            }
        }

        // --- SỰ KIỆN XÓA XE ---
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is CarModel car)
            {
                // ĐÃ SỬA: Sử dụng DeleteCarWindow thay vì DeleteConfirmWindow để tránh lỗi trùng lặp class
                DeleteCarWindow deleteWin = new DeleteCarWindow($"xe biển số {car.BienSo}");
                deleteWin.Owner = this;

                if (deleteWin.ShowDialog() == true)
                {
                    _allCars.Remove(car);
                    UpdateSTT();
                    RefreshGrid();
                }
            }
        }

        // --- ĐIỀU HƯỚNG SIDEBAR ---
        private void MenuSideBar_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listBox = sender as ListBox;
            if (listBox == null || listBox.SelectedItem == null) return;

            var selectedItem = listBox.SelectedItem as ListBoxItem;
            var content = (selectedItem.Content as StackPanel)?.Children.OfType<TextBlock>().LastOrDefault()?.Text;

            if (content == "Quản lý khách hàng")
            {
                // Giả định namespace của CustomerWindow là đúng
                test2.Views.Customer.CustomerWindow customerWin = new test2.Views.Customer.CustomerWindow();
                customerWin.Show();
                this.Close();
            }
        }

        // --- ĐĂNG XUẤT ---
        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new LogoutConfirm();
            dialog.Owner = this;
            if (dialog.ShowDialog() == true)
            {
                new LoginWindow().Show();
                this.Close();
            }
        }
    }
}