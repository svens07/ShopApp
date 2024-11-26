using ShopAPI.Models;

namespace AuthApi.Models.Responses
{
    public class OrderInfo
    {
        public OrderInfo() { }

        public int id { get; set; }
        public double totalAmount { get; set; }
        public DateTime createdAt { get; set; }
        public List<CartItemMin> cartItems { get; set; }
    }
}
