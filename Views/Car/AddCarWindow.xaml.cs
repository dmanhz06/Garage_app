using System.Windows;

namespace test2.Views.Car
{
    public partial class AddCarWindow : Window
    {
        // Thuộc tính để chứa dữ liệu xe mới tạo
        public CarModel NewCar { get; private set; }

        public AddCarWindow()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            // Kiểm tra dữ liệu đầu vào cơ bản
            if (string.IsNullOrWhiteSpace(txtBienSo.Text) || string.IsNullOrWhiteSpace(txtChuXe.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ Biển số và Tên chủ xe!");
                return;
            }

            // Gán dữ liệu vào đối tượng NewCar
            NewCar = new CarModel
            {
                BienSo = txtBienSo.Text,
                HangXe = txtHangXe.Text,
                DongXe = txtDongXe.Text,
                NamSX = txtNamSX.Text,
                TenKhachHang = txtChuXe.Text
            };

            this.DialogResult = true; // Đóng cửa sổ và trả về kết quả thành công
            this.Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}