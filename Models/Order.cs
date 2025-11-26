namespace BBURGERClone.Models
{
    public class Order
    {
        public string OrderId { get; set; }
        public string CustomerName { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public List<CartItem> Items { get; set; }
        public decimal Total { get; set; }
    }
}