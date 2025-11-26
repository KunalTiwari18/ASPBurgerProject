using System.Collections.Generic;
using System.Linq;
using BBURGERClone.Models;

namespace BBURGERClone.Services
{
    // Simple in-memory product repository used for development/demo.
    // Replace with EF Core or another store for production.
    public class ProductService : IProductService
    {
        // readonly so it can't be reassigned later
        private readonly List<Product> _products;

        public ProductService()
        {
            // seed 9 example products (replace images as needed)
            _products = new List<Product>
            {
                new Product { Id = 1, Name = "Classic Burger", Description = "Beef patty, lettuce, tomato, house sauce", Price = 199.00m, ImageUrl = "/images/burger1.jpg" },
                new Product { Id = 2, Name = "Cheese Blast", Description = "Double cheese, crispy onion", Price = 249.00m, ImageUrl = "/images/burger2.jpg" },
                new Product { Id = 3, Name = "Veggie Delight", Description = "Grilled veg patty, avocado", Price = 179.00m, ImageUrl = "/images/burger3.jpg" },
                new Product { Id = 4, Name = "Spicy Fire", Description = "Spicy patty, jalapeños, pepper jack", Price = 229.00m, ImageUrl = "/images/burger4.jpg" },
                new Product { Id = 5, Name = "BBQ Royale", Description = "Smoky BBQ sauce, onion rings", Price = 259.00m, ImageUrl = "/images/burger5.jpg" },
                new Product { Id = 6, Name = "Mushroom Melt", Description = "Sautéed mushrooms, swiss cheese", Price = 219.00m, ImageUrl = "/images/burger6.jpg" },
                new Product { Id = 7, Name = "Chicken Supreme", Description = "Grilled chicken, lettuce, mayo", Price = 209.00m, ImageUrl = "/images/burger7.jpg" },
                new Product { Id = 8, Name = "Paneer Tikka Burger", Description = "Indian-spiced paneer, mint chutney", Price = 189.00m, ImageUrl = "/images/burger8.jpg" },
                new Product { Id = 9, Name = "Fish Crisp", Description = "Crispy fish fillet, tartar", Price = 229.00m, ImageUrl = "/images/burger9.jpg" }
            };
        }

        // Return a copy (or IEnumerable) so callers can't modify our internal list
        public IEnumerable<Product> GetAll() => _products.AsReadOnly();

        public Product? GetById(int id) => _products.FirstOrDefault(p => p.Id == id);
    }
}
