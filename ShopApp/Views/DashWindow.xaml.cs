using ShopApp.Views.Controls;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace ShopApp
{
    /// <summary>
    /// Interaction logic for DashWindow.xaml
    /// </summary>
    public partial class DashWindow : Window
    {
        public DashWindow()
        {
            InitializeComponent();
            shopControl = new ShopControl(this)
            {
                Visibility = Visibility.Visible
            };
            shoppingCartControl = new ShoppingCartControl(this)
            {
                Visibility = Visibility.Hidden
            };
            ordersControl = new OrdersConrol(this)
            {
                Visibility = Visibility.Hidden
            };
            this.ContentControl.Children.Add(shopControl);
            this.ContentControl.Children.Add(shoppingCartControl);
            this.ContentControl.Children.Add(ordersControl);
        }

        public ShopControl shopControl
        {
            get;
            set;
        }
        public ShoppingCartControl shoppingCartControl
        {
            get;
            set;
        }
        public OrdersConrol ordersControl
        {
            get;
            set;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void Minimize(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Close(object sender, MouseButtonEventArgs e)
        {
            Environment.Exit(-1);
        }

        private void menuBtn_Click(object sender, RoutedEventArgs e)
        {
            this.productsBtn.Foreground = Brushes.White;
            this.ordersBtn.Foreground = Brushes.White;
            this.cartBtn.Foreground = Brushes.White;
            this.accountBtn.Foreground = Brushes.White;

            shopControl.Visibility = Visibility.Hidden;
            ordersControl.Visibility = Visibility.Hidden;
            shoppingCartControl.Visibility = Visibility.Hidden;

            if (sender == this.productsBtn)
            {
                this.productsBtn.Foreground = Brushes.Yellow;
                shopControl.Visibility = Visibility.Visible;
            }
            else if (sender == this.ordersBtn)
            {
                this.ordersBtn.Foreground = Brushes.Yellow;
                ordersControl.UpdateList();
                ordersControl.Visibility = Visibility.Visible;
            }
            else if (sender == this.cartBtn)
            {
                this.cartBtn.Foreground = Brushes.Yellow;
                shoppingCartControl.UpdateList();
                shoppingCartControl.Visibility = Visibility.Visible;
            }

        }

        public void MoveToOrders()
        {
            this.productsBtn.Foreground = Brushes.White;
            this.ordersBtn.Foreground = Brushes.White;
            this.cartBtn.Foreground = Brushes.White;
            this.accountBtn.Foreground = Brushes.White;

            shopControl.Visibility = Visibility.Hidden;
            ordersControl.Visibility = Visibility.Hidden;
            shoppingCartControl.Visibility = Visibility.Hidden;

            this.ordersBtn.Foreground = Brushes.Yellow;
            ordersControl.UpdateList();
            ordersControl.Visibility = Visibility.Visible;

        }
    }
}