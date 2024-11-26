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
using ShopAPI.Models;
using ShopApp.Services;

namespace ShopApp.Views.Controls
{
    /// <summary>
    /// Interaction logic for ShopControl.xaml
    /// </summary>
    public partial class ShopControl : UserControl
    {
        public ShopControl(DashWindow dashWindow)
        {
            InitializeComponent();
            DashWindow = dashWindow;
        }

        public DashWindow DashWindow
        {
            get;
        }

        private async void ShopControl_OnLoaded(object sender, RoutedEventArgs e)
        {
            var result = await APIService.GetProducts();

            if (result.success == true && result.data != null)
            {
                List<Product> allProducts = result.data;

                var groupedProducts = allProducts
                    .GroupBy(p => p.category)
                    .ToList();

                foreach (var group in groupedProducts)
                {
                    var categoryName = group.Key;
                    var productsInCategory = group.ToList();

                    var categoryControl = new CategoryControl(categoryName, productsInCategory);

                    categoryStackPanel.Children.Add(categoryControl);
                }

            }
            else
            {
                MessageBox.Show("Something went wrong:\n" + result.error);
            }

        }
    }
}