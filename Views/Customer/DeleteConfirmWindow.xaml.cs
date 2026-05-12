using System.Windows;

namespace test2.Views.Model_Box
{
    public partial class DeleteConfirmWindow : Window
    {
        public DeleteConfirmWindow(string customerName)
        {
            InitializeComponent();
            txtMessage.Text = $"Bạn có chắc chắn muốn xóa khách hàng {customerName} không? Hành động này không thể hoàn tác.";
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}