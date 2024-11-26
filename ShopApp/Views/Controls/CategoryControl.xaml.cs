using System.Windows;
using System.Windows.Controls;
using ShopAPI.Models;

namespace ShopApp.Views.Controls;

public partial class CategoryControl : UserControl
{
    private string categoryName;
    private List<Product> products;

    public CategoryControl(string categoryName_, List<Product> products_)
    {
        InitializeComponent();
        categoryName = categoryName_;
        products = products_;

        categoryTitle.Content = categoryName;
        foreach (Product product in products)
        {
            var productControl = new ProductControl(product);
            productsContent.Children.Add(productControl);
        }
    }
}