namespace ShopAPI.Models
{
    public class CartItem
    {
        public int id { get; set; }
        public int cartId { get; set; }
        public int productId { get; set; }
        public int quantity { get; set; }
        public DateTime updatedAt { get; set; }
        public DateTime createdAt { get; set; }
    }
}
