namespace BBURGERClone.Models
{
    public class CartItem
    {
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!; // will be set when adding to cart
        public int Quantity { get; set; }
        public decimal LineTotal => Product?.Price * Quantity ?? 0m;
    }
}
