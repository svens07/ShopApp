using ShopApp.views;
using System.CodeDom;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ShopApp
{
    /// <summary>
    /// Interaction logic for AuthWindow.xaml
    /// </summary>
    public partial class AuthWindow : Window
    {
        public LoginControl loginControl;
        public RegisterControl registerControl;

        public AuthWindow()
        {
            InitializeComponent();

            loginControl = new LoginControl(this);
            registerControl = new RegisterControl(this);

            registerControl.Visibility = Visibility.Hidden;
            loginControl.Visibility = Visibility.Visible;

            authContent.Children.Add(loginControl);
            authContent.Children.Add(registerControl);
        }

        private void FadeElement(UIElement element, double fromOpacity, double toOpacity, double seconds, Action? onCompleted = null)
        {
            var opacityAnimation = new DoubleAnimation
            {
                From = fromOpacity,
                To = toOpacity,
                Duration = new Duration(TimeSpan.FromSeconds(seconds))
            };

            var storyboard = new Storyboard();
            storyboard.Children.Add(opacityAnimation);

            Storyboard.SetTarget(opacityAnimation, element);
            Storyboard.SetTargetProperty(opacityAnimation, new PropertyPath(UIElement.OpacityProperty));

            storyboard.Completed += (sender, e) => onCompleted?.Invoke();
            storyboard.Begin();
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            FadeElement(this, 0, 1, 0.25);
        }

        public void FadeToRegister()
        {
            FadeElement(authContent, 1, 0, 0.25, () => {
                loginControl.Visibility = Visibility.Hidden;
                registerControl.Visibility = Visibility.Visible;
                this.Title = "ShopApp - Register";
                FadeElement(authContent, 0, 1, 0.25);
            });
        }

        public void FadeToLogin()
        {
            FadeElement(authContent, 1, 0, 0.25, () => {
                registerControl.Visibility = Visibility.Hidden;
                loginControl.Visibility = Visibility.Visible;
                this.Title = "ShopApp - Register";
                FadeElement(authContent, 0, 1, 0.25);
            });
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }
        private void closeLabel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
    }
}