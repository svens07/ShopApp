using ShopApp.Services;
using System.Windows.Controls;

namespace ShopApp.Views.Controls;

public partial class ShoppingCartControl : UserControl
{
    public ShoppingCartControl(DashWindow dashWindow)
    {
        InitializeComponent();
        DashWindow = dashWindow;
    }

    public DashWindow DashWindow
    {
        get;
        set;
    }

    public void UpdateTotalPrice()
    {
        double total = 0;

        foreach (var child in cartItemsContent.Children)
        {
            var cartItem = (CartItemControl)child;

            if (cartItem.productInfo != null && cartItem.info != null)
            {
                total += cartItem.productInfo.price * cartItem.info.quantity;
            };
        }

        total = Math.Ceiling(total * 100) / 100;

        totalPrice.Content = total.ToString() + "€";
    }

    public async void UpdateList()
    {
        var result = await APIService.GetCartItems();

        if (result.success == true)
        {
            cartItemsContent.Children.Clear();
            foreach (var cartItem in result.data)
            {
                var cartItemControl = new CartItemControl(cartItem, this);
                cartItemsContent.Children.Add(cartItemControl);
            }
        }

        UpdateTotalPrice();
    }

    private async void cartItemsContent_Loaded(object sender, System.Windows.RoutedEventArgs e)
    {
        UpdateList();
    }

    private async void orderBtn_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        var result = await APIService.CreateOrder();

        if (result.success == true)
        {
            DashWindow.MoveToOrders();
        }
    }
}