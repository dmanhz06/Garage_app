using System.Windows;

namespace test2.Views.Model_Box
{
    public partial class DeleteCarWindow : Window
    {
        public DeleteCarWindow(string message)
        {
            InitializeComponent();
            txtMessage.Text = $"Bạn có chắc chắn muốn xóa {message} không?";
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