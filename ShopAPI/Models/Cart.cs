namespace ShopAPI.Models
{
    public class Cart
    {
        public int id { get; set; }
        public int userId { get; set; }
        public double totalAmount { get; set; }
        public bool isActive { get; set; }
        public DateTime createdAt { get; set; }
    }
}
