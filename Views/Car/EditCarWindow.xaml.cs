using System.Windows;
using test2.Views.Car; // Đảm bảo CarModel nằm trong namespace này

namespace test2.Views.Model_Box
{
    public partial class EditCarWindow : Window
    {
        private CarModel _currentCar;

        public EditCarWindow(CarModel car)
        {
            InitializeComponent();
            _currentCar = car;

            if (_currentCar != null)
            {
                txtEditBienSo.Text = _currentCar.BienSo;
                txtEditHang.Text = _currentCar.HangXe;
                txtEditDong.Text = _currentCar.DongXe;
                txtEditChuXe.Text = _currentCar.TenKhachHang;
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (_currentCar != null)
            {
                _currentCar.BienSo = txtEditBienSo.Text.Trim();
                _currentCar.HangXe = txtEditHang.Text.Trim();
                _currentCar.DongXe = txtEditDong.Text.Trim();
                _currentCar.TenKhachHang = txtEditChuXe.Text.Trim();

                this.DialogResult = true;
                this.Close();
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}