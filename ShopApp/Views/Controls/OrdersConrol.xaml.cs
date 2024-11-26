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

namespace ShopApp.Views.Controls
{
    /// <summary>
    /// Interaction logic for OrdersConrol.xaml
    /// </summary>
    public partial class OrdersConrol : UserControl
    {
        public OrdersConrol(DashWindow dashWindow_)
        {
            InitializeComponent();
            dashWindow = dashWindow_;
        }

        public DashWindow dashWindow
        {
            get;
        }
        private void StackPanel_Loaded(object sender, RoutedEventArgs e)
        {

        }

        public async void UpdateList()
        {
            var result = await APIService.GetOrders();

            if (result.success)
            {
                orderItemsContent.Children.Clear();
                foreach (var order in result.data)
                {
                    OrderItemControl control = new(order);
                    orderItemsContent.Children.Add(control);
                }
            }
            else
            {
                MessageBox.Show(result.error);
            }
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateList();
        }
    }
}