using AuthApi.Models.Responses;
using HandyControl.Tools.Extension;
using ShopAPI.Models;
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
    /// Interaction logic for CartItemControl.xaml
    /// </summary>
    public partial class CartItemControl : UserControl
    {
        public CartItemMin? info;
        public Product? productInfo;
        private ShoppingCartControl cartControl;

        public CartItemControl(CartItemMin info_, ShoppingCartControl cartControl_)
        {
            InitializeComponent();
            info = info_;
            cartControl = cartControl_;
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var result = await APIService.GetProductInfo(info.productId);

            if (result.success == true)
            {
                productInfo = result.data;

                productTitle.Content = productInfo.name;
                productImage.Source = new BitmapImage(new Uri("pack://application:,,," + productInfo.image));
                productDescription.Content = productInfo.description;
                productAmount.Value = info.quantity;
                productPrice.Content = (Math.Ceiling((productInfo.price * info.quantity) * 100) / 100).ToString() + "€";
            }
            else
            {
                productTitle.Content = "Error loading...";
            }
        }

        private int previousQuantity;
        private async void productAmount_ValueChanged(object sender, HandyControl.Data.FunctionEventArgs<double> e)
        {
            if (info != null)
            {
                previousQuantity = info.quantity;
                info.quantity = (int)e.Info;
                productPrice.Content = (Math.Ceiling((productInfo.price * info.quantity) * 100) / 100).ToString() + "€";

                var result = await APIService.ModifyCartItem(info);

                if (!result.success)
                {
                    info.quantity = previousQuantity;
                    productAmount.Value = previousQuantity;
                    productPrice.Content = (Math.Ceiling((productInfo.price * info.quantity) * 100) / 100).ToString() + "€";
                }
                else
                {
                    cartControl.UpdateTotalPrice();
                }
            }
        }

        private async void deleteBtn_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (info != null)
            {
                var result = await APIService.RemoveCartItem(info.productId);

                if (result.success)
                {
                    cartControl.UpdateList();

                }
            }
        }
    }
}