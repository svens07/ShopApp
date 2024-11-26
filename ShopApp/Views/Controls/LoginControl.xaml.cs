using AuthApi.Models.Requests;
using HandyControl.Tools.Extension;
using ShopApp.Services;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ShopApp.views
{
    /// <summary>
    /// Interaction logic for LoginControl.xaml
    /// </summary>
    public partial class LoginControl : UserControl
    {
        private AuthWindow authWindow;

        public LoginControl(AuthWindow authWindow_)
        {
            InitializeComponent();
            authWindow = authWindow_;
        }

        private void NoAccountLabel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            authWindow.FadeToRegister();
        }

        private async void loginBtn_Click(object sender, RoutedEventArgs e)
        {
            LoginRequest request = new()
            {
                username = username.Text,
                password = password.Password
            };

            var result = await APIService.LoginAsync(request);
            if (result.success)
            {
                DashWindow window = new();
                window.Show();
                authWindow.Hide();
            }
            else
            {
                MessageBox.Show($"Failed to login!\nError: {result.error}");
            }
        }
    }
}