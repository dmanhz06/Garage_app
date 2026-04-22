using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace test2
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // CLASS dữ liệu
        public class Person
        {
            public string Name { get; set; } = "";
            public string Gender { get; set; } = "";
            public string Service { get; set; } = "";
            public string City { get; set; } = "";
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = e.Uri.AbsoluteUri,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không mở được link!\n" + ex.Message);
            }

            e.Handled = true;
        }

        private void Reload_Click(object sender, RoutedEventArgs e)
        {
            lvData.Items.Clear();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            // có thể để trống
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string name = txtHoDem.Text + " " + txtTen.Text;

            //string gender = ((ComboBoxItem)cboGioiTinh.SelectedItem).Content.ToString();
            string gender = "";
            if (cboGioiTinh.SelectedItem != null)
                gender = ((ComboBoxItem)cboGioiTinh.SelectedItem).Content.ToString();

            string service = "";
            if (chkDuong.IsChecked == true) service += "Bảo Dưỡng Định Kỳ\n";
            if (chkSua.IsChecked == true) service += "Sửa Chữa Thiết bị\n";
            if (chkNang.IsChecked == true) service += "Nâng Cấp Phụ Tùng\n";

            string city = "";
            if (lstQue.SelectedItem != null)
                city = ((ListBoxItem)lstQue.SelectedItem).Content.ToString();

            Person p = new Person()
            {
                Name = name,
                Gender = gender,
                Service = service,
                City = city
            };

            lvData.Items.Add(p);
        }
    }
}