using System.ComponentModel.DataAnnotations;

namespace BBURGERClone.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        [Required]
        public decimal Price { get; set; }

        public string ImageUrl { get; set; } = "/images/placeholder.png";
    }
}
