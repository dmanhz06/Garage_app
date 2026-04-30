using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using test2.Views.Model_Box;

namespace test2.Views.Car
{
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
        private List<CarModel> _allCars = new List<CarModel>();

        public CarWindow()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            // Dữ liệu mẫu dựa trên hình ảnh image_0018a0.png
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

        private void UpdateSTT()
        {
            for (int i = 0; i < _allCars.Count; i++)
            {
                _allCars[i].STT = i + 1;
            }
        }

        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtSearch == null || _allCars == null) return;
            string searchText = txtSearch.Text.ToLower().Trim();

            if (string.IsNullOrEmpty(searchText))
                CarDataGrid.ItemsSource = _allCars;
            else
            {
                var filtered = _allCars.Where(c =>
                    c.BienSo.ToLower().Contains(searchText) ||
                    c.TenKhachHang.ToLower().Contains(searchText)
                ).ToList();
                CarDataGrid.ItemsSource = filtered;
            }
        }

        private void btnAddCar_Click(object sender, RoutedEventArgs e)
        {
            // Logic mở cửa sổ thêm xe
            MessageBox.Show("Mở cửa sổ thêm xe mới!");
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is CarModel car)
            {
                MessageBox.Show($"Chỉnh sửa xe: {car.BienSo}");
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is CarModel car)
            {
                if (MessageBox.Show($"Xác nhận xóa xe {car.BienSo}?", "Thông báo", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    _allCars.Remove(car);
                    UpdateSTT();
                    RefreshGrid();
                }
            }
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void RefreshGrid()
        {
            CarDataGrid.ItemsSource = null;
            CarDataGrid.ItemsSource = _allCars;
        }
    }
}