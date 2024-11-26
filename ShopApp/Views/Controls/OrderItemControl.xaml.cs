using AuthApi.Models.Responses;
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
    /// Interaction logic for OrderItemControl.xaml
    /// </summary>
    public partial class OrderItemControl : UserControl
    {
        OrderInfo info;

        public OrderItemControl(OrderInfo info_)
        {
            InitializeComponent();
            info = info_;
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            orderTitle.Content += info.id.ToString();
            orderDate.Content = info.createdAt.ToString();

            foreach (var cartItem in info.cartItems)
            {
                var result = await APIService.GetProductInfo(cartItem.productId);

                if (result.success == true)
                {
                    double price = Math.Ceiling((result.data.price * cartItem.quantity) * 100) / 100;

                    orderDescription.Content += $"{cartItem.quantity.ToString()}x {result.data.name} ({price.ToString()}€)\n";
                }
            }

            var totalAmount = Math.Ceiling((info.totalAmount) * 100) / 100;
            productPrice.Content = totalAmount.ToString() + "€";
        }
    }
}