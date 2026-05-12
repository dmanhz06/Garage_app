using System.Windows;

namespace test2.Views.Model_Box
{
    public partial class LogoutConfirm : Window
    {
        public LogoutConfirm()
        {
            InitializeComponent();
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