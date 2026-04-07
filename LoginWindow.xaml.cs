using System;
using System.Windows;
using System.Windows.Media.Animation;
using System.Collections.Generic;

namespace test2
{
    public partial class LoginWindow : Window
    {
        bool isSwapped = false;

        // Lưu tài khoản tạm
        List<User> users = new List<User>();

        public LoginWindow()
        {
            InitializeComponent();
        }

        public class User
        {
            public string Username { get; set; }
            public string Password { get; set; }
            public string Email { get; set; }
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private void Switch_Click(object sender, RoutedEventArgs e)
        {
            double leftWidth = 350;
            double totalWidth = 750;

            DoubleAnimation leftAnim = new DoubleAnimation();
            DoubleAnimation rightAnim = new DoubleAnimation();

            leftAnim.Duration = TimeSpan.FromMilliseconds(500);
            rightAnim.Duration = TimeSpan.FromMilliseconds(500);

            leftAnim.EasingFunction = new CubicEase { EasingMode = EasingMode.EaseInOut };
            rightAnim.EasingFunction = new CubicEase { EasingMode = EasingMode.EaseInOut };

            if (!isSwapped)
            {
                leftAnim.To = totalWidth - leftWidth;
                rightAnim.To = -leftWidth;

                LeftPanel.CornerRadius = new CornerRadius(0, 15, 15, 0);

                // 👉 HIỆN REGISTER
                gridRegisterUsername.Visibility = Visibility.Visible;

                txtFormTitle.Text = "Register";
                txtTitle.Text = "Welcome Back !";
                txtDesc.Text = "Already have an account ?";
                btnSwitch.Content = "Log In";
                btnLogin.Content = "Register";

                // ✅ THU NHỎ FONT + UI
                MainFormStack.Margin = new Thickness(0, 5, 0, 5);
                gridUsername.Margin = new Thickness(0, 0, 0, 10);
                gridRegisterUsername.Margin = new Thickness(0, 0, 0, 10);
                gridPassword.Margin = new Thickness(0, 0, 0, 5);
                txtFormTitle.FontSize = 26;
                txtUsername.FontSize = 13;
                txtRegisterUsername.FontSize = 13;
                txtPassword.FontSize = 13;

                // 👇 THU GỌN SOCIAL
                txtOrSocial.Margin = new Thickness(0, 0, 0, 8);
                panelSocial.Margin = new Thickness(0, 5, 0, 0);

                // 👉 ẨN forgot password
                txtForgot.Visibility = Visibility.Collapsed;
            }
            else
            {
                leftAnim.To = 0;
                rightAnim.To = 0;

                LeftPanel.CornerRadius = new CornerRadius(15, 0, 0, 15);

                // 👉 ẨN REGISTER
                gridRegisterUsername.Visibility = Visibility.Collapsed;

                txtFormTitle.Text = "Login";
                txtTitle.Text = "Hello, Welcome!";
                txtDesc.Text = "Don't have an account ?";
                btnLogin.Content = "Login";
                btnSwitch.Content = "Register";
                MainFormStack.Margin = new Thickness(0, 0, 0, 0);

                // ✅ TRẢ VỀ FONT BAN ĐẦU
                txtRegisterUsername.FontSize = 14;
                gridUsername.Margin = new Thickness(0, 0, 0, 15);
                gridRegisterUsername.Margin = new Thickness(0, 0, 0, 15);
                gridPassword.Margin = new Thickness(0, 0, 0, 10);
                txtFormTitle.FontSize = 32;
                txtUsername.FontSize = 14;
                txtPassword.FontSize = 14;

                // 👇 TRẢ LẠI SOCIAL
                txtOrSocial.Margin = new Thickness(0, 0, 0, 15);
                panelSocial.Margin = new Thickness(0, 10, 0, 5);

                // 👉 HIỆN lại forgot password
                txtForgot.Visibility = Visibility.Visible;
            }

            leftTransform.BeginAnimation(System.Windows.Media.TranslateTransform.XProperty, leftAnim);
            rightTransform.BeginAnimation(System.Windows.Media.TranslateTransform.XProperty, rightAnim);

            isSwapped = !isSwapped;
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Password.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!");
                return;
            }

            // 👉 REGISTER MODE
            if (isSwapped)
            {
                string email = txtRegisterUsername.Text.Trim();

                if (string.IsNullOrEmpty(email))
                {
                    MessageBox.Show("Vui lòng nhập email!");
                    return;
                }

                if (!IsValidEmail(email))
                {
                    MessageBox.Show("Email không hợp lệ!");
                    return;
                }

                // kiểm tra trùng username
                var existingUser = users.Find(u => u.Username == username);

                if (existingUser != null)
                {
                    MessageBox.Show("Tài khoản đã tồn tại!");
                    return;
                }

                // tạo user mới
                users.Add(new User
                {
                    Username = username,
                    Password = password,
                    Email = email
                });

                MessageBox.Show("Đăng ký thành công!");

                // quay về login
                Switch_Click(null, null);
            }
            else
            {
                // 👉 LOGIN MODE
                var user = users.Find(u => u.Username == username && u.Password == password);

                if (user != null)
                {
                    MessageBox.Show("Đăng nhập thành công!");

                    MainWindow main = new MainWindow();
                    main.Show();

                    this.Close();
                }
                else
                {
                    MessageBox.Show("Sai tài khoản hoặc mật khẩu!");
                }
            }
        }

        private void txtUsername_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            phUsername.Visibility = string.IsNullOrEmpty(txtUsername.Text)
                ? Visibility.Visible
                : Visibility.Hidden;
        }

        private void txtRegisterUsername_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            phRegisterUsername.Visibility = string.IsNullOrEmpty(txtRegisterUsername.Text)
                ? Visibility.Visible
                : Visibility.Hidden;
        }

        private void txtPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            phPassword.Visibility = string.IsNullOrEmpty(txtPassword.Password)
                ? Visibility.Visible
                : Visibility.Hidden;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}