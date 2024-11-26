namespace ShopAPI.Models
{
    public class Order
    {
        public int id { get; set; }
        public int userId { get; set; }
        public int cartId   { get; set; }
        public double totalAmount { get; set; }
        public DateTime createdAt { get; set; }
    }
}
