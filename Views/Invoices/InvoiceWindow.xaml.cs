using System;
using System.Windows;
using test2.Services;

namespace test2.Views.Invoices 
{
    public partial class InvoiceWindow : Window
    {   

        private InvoiceService _invoiceService = new InvoiceService();

        public InvoiceWindow()
        {
            InitializeComponent(); 
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                // Lấy danh sách từ Service và đổ vào DataGrid đã đặt tên trong XAML
                var data = _invoiceService.GetAllInvoices();
                InvoiceDataGrid.ItemsSource = data;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể tải dữ liệu: " + ex.Message);
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Kiểm tra dữ liệu đầu vào để tránh lỗi crash
                if (string.IsNullOrEmpty(txtRepairID.Text) || string.IsNullOrEmpty(txtLabor.Text))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin!");
                    return;
                }

                int id = int.Parse(txtRepairID.Text);
                decimal labor = decimal.Parse(txtLabor.Text);

                _invoiceService.CreateInvoice(id, labor);

                MessageBox.Show("Thanh toán thành công!");

                // Xóa sạch ô nhập liệu sau khi lưu
                txtRepairID.Clear();
                txtLabor.Clear();

                LoadData(); 
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }
    }
}