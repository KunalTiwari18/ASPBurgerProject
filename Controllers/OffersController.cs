using Microsoft.AspNetCore.Mvc;

namespace BBURGERClone.Controllers
{
    public class OffersController : Controller
    {
        public IActionResult Index()
        {
            // sample offers - you can replace with DB later
            var offers = new[]
            {
                new { Title = "Flat 20% off on orders above ₹300", Code = "SAVE20" },
                new { Title = "Buy 1 Get 1 on Classic Burger (Tue/Thu)", Code = "B1G1" },
                new { Title = "Free fries on first order", Code = "WELCOME" }
            };
            return View(offers);
        }
    }
}
