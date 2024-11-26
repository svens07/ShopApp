namespace ShopAPI.Models
{
    public class Product
    {
        public int id { get; set; }
        public string name { get; set; }
        public string image { get; set; }
        public string category { get; set; }
        public string description { get; set; }
        public double price { get; set; }
        public bool isEnabled { get; set; }
        public DateTime createdAt { get; set; }
    }
}
