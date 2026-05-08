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


namespace test2.Views.Service
{
    public partial class DeleteServiceWindow : Window
    {
        public DeleteServiceWindow(string bienSo)
        {
            InitializeComponent();
            // Cập nhật câu hỏi cho đúng biển số xe đang chọn
            txtMessage.Text = $"Bạn có chắc chắn muốn xóa phiếu dịch vụ của xe biển số {bienSo} không?";
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true; // Báo là đã chọn Yes
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false; // Báo là chọn No
        }
    }
}
