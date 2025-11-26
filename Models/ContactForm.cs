using System.ComponentModel.DataAnnotations;

namespace BBURGERClone.Models
{
    public class ContactForm
    {
        public int Id { get; set; } // optional if you later save to DB

        [Required(ErrorMessage = "Name is required")]
        [StringLength(80, ErrorMessage = "Name must be 80 characters or less")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        [StringLength(120)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter a message")]
        [StringLength(2000, ErrorMessage = "Message is too long")]
        public string Message { get; set; } = string.Empty;
    }
}
