using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace test2
{
    public partial class LoginWindow : Window
    {
        bool isSwapped = false;

        public LoginWindow()
        {
            InitializeComponent();
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

                // 🔥 ĐẢO BO GÓC
                LeftPanel.CornerRadius = new CornerRadius(0, 15, 15, 0);
                    
                txtFormTitle.Text = "Register";
                txtTitle.Text = "Welcome Back !";
                txtDesc.Text = "Already have an account ?";
                btnSwitch.Content = "Log In";
            }
            else
            {
                leftAnim.To = 0;
                rightAnim.To = 0;

                // 🔥 TRẢ LẠI BO GÓC BAN ĐẦU
                LeftPanel.CornerRadius = new CornerRadius(15, 0, 0, 15);

                txtFormTitle.Text = "Login";
                txtTitle.Text = "Hello, Welcome!";
                txtDesc.Text = "Don't have an account ?";
                btnSwitch.Content = "Register";
            }

            leftTransform.BeginAnimation(TranslateTransform.XProperty, leftAnim);
            rightTransform.BeginAnimation(TranslateTransform.XProperty, rightAnim);

            isSwapped = !isSwapped;
        }

        private void txtUsername_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            phUsername.Visibility = string.IsNullOrEmpty(txtUsername.Text)
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