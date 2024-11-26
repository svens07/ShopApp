using AuthApi.Models.Responses;
using ShopAPI.Models;
using ShopApp.Services;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ShopApp.Views.Controls
{
    public partial class ProductControl : UserControl
    {
        private Product product;

        public ProductControl(Product product_)
        {
            InitializeComponent();
            product = product_;
            productTitle.Content = product.name;
            productPrice.Content = product.price.ToString() + "€";
            backgroundImage.Source = new BitmapImage(new Uri("pack://application:,,," + product.image));
        }

        private async void cartBtn_Click(object sender, RoutedEventArgs e)
        {
            CartItemMin item = new()
            {
                productId = product.id,
                quantity = 1
            };
            await APIService.AddCartItem(item);

        }
    }
}