using AuthApi.Models.Requests;
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
using ShopApp.Services;

namespace ShopApp.views
{
    /// <summary>
    /// Interaction logic for RegisterControl.xaml
    /// </summary>
    public partial class RegisterControl : UserControl
    {
        private AuthWindow authWindow;

        public RegisterControl(AuthWindow authWindow_)
        {
            InitializeComponent();
            authWindow = authWindow_;
        }

        private void HasAccount_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            authWindow.FadeToLogin();
        }

        private async void registerBtn_Click(object sender, RoutedEventArgs e)
        {
            RegisterRequest request = new()
            {
                username = username.Text,
                email = email.Text,
                password = password.Password
            };

            var result = await APIService.RegisterAsync(request);
            if (result.success)
            {
                DashWindow window = new();
                window.Show();
                authWindow.Hide();
            }
            else
            {
                MessageBox.Show($"Failed to register!\nError: {result.error}");
            }
        }
    }
}