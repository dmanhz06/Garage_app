using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using test2.Models;

namespace test2.Views.Service
{
    public partial class EditServiceWindow : Window
    {
        // Biến này để giữ cái dữ liệu cũ truyền từ bảng sang
        private ServiceOrder _serviceToEdit;

        public EditServiceWindow(ServiceOrder service)
        {
            InitializeComponent();
            _serviceToEdit = service;

            // Đổ dữ liệu cũ lên các ô TextBox để người ta thấy mà sửa
            txtEditBienSo.Text = service.LicensePlate;
            txtEditChuXe.Text = service.CustomerName;
            txtEditTenDichVu.Text = service.ServiceName;
            txtEditGiaTien.Text = service.Price.ToString();
            txtEditThoiGian.Text = service.Time;
            cmbEditTinhTrang.Text = service.Status;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            // Cập nhật dữ liệu mới vào Object
            _serviceToEdit.LicensePlate = txtEditBienSo.Text;
            _serviceToEdit.CustomerName = txtEditChuXe.Text;
            _serviceToEdit.ServiceName = txtEditTenDichVu.Text;

            decimal price = 0;
            decimal.TryParse(txtEditGiaTien.Text, out price);
            _serviceToEdit.Price = price;

            _serviceToEdit.Time = txtEditThoiGian.Text;
            _serviceToEdit.Status = cmbEditTinhTrang.Text;

            // Báo thành công và đóng
            this.DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}